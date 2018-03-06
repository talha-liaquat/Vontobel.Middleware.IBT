using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vontobel.Middleware.IBT.Common
{
    public class SystemConfig
    {
        public static NameValueCollection Default { get { return System.Configuration.ConfigurationManager.AppSettings; } }
    }
}
