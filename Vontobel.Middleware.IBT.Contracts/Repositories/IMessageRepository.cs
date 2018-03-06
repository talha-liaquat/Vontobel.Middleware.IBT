using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vontobel.Middleware.IBT.Contracts.Model;

namespace Vontobel.Middleware.IBT.Contracts.Repositories
{
    public interface IMessageRepository
    {
        void Save(DataMessage message);
        DataMessage GetUnpickedMessage();
        void MarkAsPicked(string messageId);
    }
}
