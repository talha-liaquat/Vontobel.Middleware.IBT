using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vontobel.Middleware.IBT.Contracts.Model;
using Vontobel.Middleware.IBT.Contracts.Repositories;
using Vontobel.Middleware.IBT.Core;

namespace Vontobel.Middleware.IBT.MessageSources.Database
{
    public class MessageDatabaseSource : IMessageSource<DataMessage>
    {
        IMessageRepository repository;
        string messageId;
        public MessageDatabaseSource(IMessageRepository repository)
        {
            this.repository = repository;
        }

        public void BeginTransaction()
        {
            messageId = string.Empty;
        }

        public void Commit()
        {
            if (!string.IsNullOrEmpty(messageId))
                repository.MarkAsPicked(messageId);
        }

        public DataMessage Read()
        {
            var message = repository.GetUnpickedMessage();
            messageId = message?.Id;
            return message;
        }

        public void Rollback()
        {
        }
    }
}
