using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vontobel.Middleware.IBT.Common
{
    public class Log<T>
    {
        public static void Error(Exception exception)
        {
            Console.WriteLine(exception);
        }

        public static void Info(string message)
        {
            Console.WriteLine(message);
        }

        public static void Debug(string message)
        {

        }
    }
}
