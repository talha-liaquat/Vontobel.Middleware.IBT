using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vontobel.Middleware.IBT.Core
{
    public interface ITransactionable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
