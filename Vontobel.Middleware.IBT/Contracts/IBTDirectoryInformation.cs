using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vontobel.Middleware.IBT.Contracts
{
    public class IBTDirectoryInformation
    {
        public string Root { get; set; }
        public string Failure { get; set; }
        public string Success { get; set; }
    }
}
