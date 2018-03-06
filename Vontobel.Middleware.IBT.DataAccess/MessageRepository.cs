using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vontobel.Middleware.IBT.Contracts.Model;
using Vontobel.Middleware.IBT.Contracts.Repositories;
using Vontobel.Middleware.IBT.Entities;

namespace Vontobel.Middleware.IBT.DataAccess
{
    public class MessageRepository : IMessageRepository
    {
        public DataMessage GetUnpickedMessage()
        {
            DataMessage dataMessage = null;

            var context = new VontobelDBConnection();

            var oldestUnpickedMessage = context.Messages
                .Where(x => x.Picked == 0)
                .OrderBy(x => x.CreatedOn)
                .FirstOrDefault();

            if (oldestUnpickedMessage == null)
                return null;

            dataMessage = new DataMessage
            {
                Content = oldestUnpickedMessage.MessageContent.Content,
                Id = oldestUnpickedMessage.Id
            };

            dataMessage.Parameters = new Dictionary<string, string>();
            oldestUnpickedMessage.MessageParameters.ToList().ForEach(
                x =>
                dataMessage.Parameters.Add(x.Key, x.Value)
                );

            return dataMessage;
        }

        public void MarkAsPicked(string messageId)
        {
            var context = new VontobelDBConnection();
            context.Messages.Where(x => x.Id == messageId).FirstOrDefault().Picked = 1;
            context.SaveChanges();
        }

            public void Save(DataMessage message)
        {
            try
            {
                var context = new VontobelDBConnection();
                var newMessage = new Message
                {
                    CreatedOn = DateTime.Now,
                    Id = Guid.NewGuid().ToString("N"),
                    IsDeleted = false,
                    MessageContent = new MessageContent { Content = message.Content, CreatedOn = DateTime.Now },
                };

                foreach (var param in message.Parameters)
                {
                    newMessage.MessageParameters.Add(new MessageParameter
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        IsDeleted = false,
                        Key = param.Key,
                        Value = param.Value
                    });
                }

                context.Messages.Add(newMessage);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
