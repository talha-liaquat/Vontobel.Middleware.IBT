using System.Collections.Generic;
using System.Linq;
using Vontobel.Middleware.IBT.Common;
using Vontobel.Middleware.IBT.Contracts.Model;
using Vontobel.Middleware.IBT.Contracts.Repositories;
using Vontobel.Middleware.IBT.Core;
using Vontobel.Middleware.IBT.Transformation;

namespace Vontobel.Middleware.IBT.MessageSinks.Email
{
    public class PartnerEmailSink : IMessageSink<DataMessage, XsltTransformation>
    {
        IPartnerRepository repository;
        IList<string> partnerIds = null;
        int eventCode;
        readonly string protocolType = "Email";
        string messageId;

        public PartnerEmailSink(IPartnerRepository repository)
        {
            this.repository = repository;
        }

        public void BeginTransaction()
        {
            partnerIds = new List<string>();
            eventCode = -1;
            messageId = string.Empty;
        }

        public void Commit()
        {
            if (partnerIds != null)
                repository.UpdateSubscriptionLog(partnerIds, eventCode, protocolType, messageId);
        }

        public void Rollback()
        {

        }

        public void Write(DataMessage message, XsltTransformation transformation)
        {
            if (message == null || message.Parameters == null || string.IsNullOrEmpty(message.Parameters["EventType"]))
                return;
            messageId = message.Id;

            
            eventCode = int.Parse(message.Parameters["EventType"]);

            var emailSubscriptions = repository.GetSubscriptions(eventCode, protocolType, message.Id);
            transformation.Load(Resources.EmailTemplate);

            foreach (var emailSubscription in emailSubscriptions) {
                Log<PartnerEmailSink>.Info($"Sending email for Message: {message.Id} to {emailSubscription.Email}");
                var attributes = emailSubscription.GeTranformableAttributes();
                attributes.Add("ISIN", message.Parameters["ISIN"]);
                EmailComposer.SendEmail("Product Update",
                    transformation.Transform(message.Content, attributes),
                    new List<string> { emailSubscription.Email });

                partnerIds.Add(emailSubscription.Id);
            }
        }
    }
}