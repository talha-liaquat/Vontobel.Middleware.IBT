using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vontobel.Middleware.IBT.Core;

namespace Vontobel.Middleware.IBT.Transformation
{
    public class TransformationFactory
    {
        public static ITransformation Xslt { get { return new XsltTransformation(); } }
        public static ITransformation T4Template { get { throw new NotImplementedException(); } }
    }
}
