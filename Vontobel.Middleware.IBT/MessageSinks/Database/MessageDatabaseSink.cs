using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vontobel.Middleware.IBT.Common;
using Vontobel.Middleware.IBT.Contracts.Model;
using Vontobel.Middleware.IBT.Contracts.Repositories;
using Vontobel.Middleware.IBT.Core;
using Vontobel.Middleware.IBT.Transformation;

namespace Vontobel.Middleware.IBT.MessageSinks.Database
{
    public class MessageDatabaseSink : IMessageSink<DataMessage, XsltTransformation>
    {
        IMessageRepository repository;
        public MessageDatabaseSink(IMessageRepository repository)
        {
            this.repository = repository;
        }

        public void BeginTransaction()
        {
        }

        public void Commit()
        {
        }

        public void Rollback()
        {
        }

        public void Write(DataMessage message, XsltTransformation transformation)
        {
            Log<MessageDatabaseSink>.Info($"Writing Message to Database: {message.Id}");
            repository.Save(message);
        }
    }
}
