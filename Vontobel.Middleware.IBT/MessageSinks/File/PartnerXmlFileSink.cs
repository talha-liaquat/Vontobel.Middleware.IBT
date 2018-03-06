using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vontobel.Middleware.IBT.Common;
using Vontobel.Middleware.IBT.Contracts.Model;
using Vontobel.Middleware.IBT.Contracts.Repositories;
using Vontobel.Middleware.IBT.Core;
using Vontobel.Middleware.IBT.Transformation;

namespace Vontobel.Middleware.IBT.MessageSinks.File
{
    public class PartnerXmlFileSink : IMessageSink<DataMessage, XsltTransformation>
    {
        IPartnerRepository repository;
        IList<string> partnerIds = null;
        int eventCode;
        readonly string protocolType = "XmlFile";
        string messageId;

        public PartnerXmlFileSink(IPartnerRepository repository)
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
            if (message == null || message.Parameters == null || string.IsNullOrEmpty(message.Parameters["ISIN"]))
                return;

            

            messageId = message.Id;
            eventCode = int.Parse(message.Parameters["EventType"]);

            var xmlFileSubscriptions = repository.GetSubscriptions(int.Parse(message.Parameters["EventType"]), protocolType, message.Id);
            transformation.Load(Resources.XmlTemplate);

            foreach(var xmlFileSubscription in xmlFileSubscriptions)
            {

                var filePath = xmlFileSubscription.Parameters["FilePath"];

                Log<PartnerXmlFileSink>.Info($"Writing file for Message: {message.Id} at {filePath}");

                var transformedXml = transformation.Transform(message.Content, new Dictionary<string, string> { { "DateTime", DateTime.Now.ToString() }, { "ISIN", message.Parameters["ISIN"] } });
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                System.IO.File.WriteAllText(System.IO.Path.Combine(filePath, $"{Guid.NewGuid().ToString("N")}.xml"), transformedXml);

                partnerIds.Add(xmlFileSubscription.Id);
            }
        }
    }
}