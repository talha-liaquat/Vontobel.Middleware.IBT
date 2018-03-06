using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Vontobel.Middleware.IBT.Common;
using Vontobel.Middleware.IBT.Core;

namespace Vontobel.Middleware.IBT.Transformation
{
    public class XsltTransformation : ITransformation<string>
    {
        private string xslt;
        public void Load(string xslt)
        {
            this.xslt = xslt;
        }
        public string Transform(string source, Dictionary<string, string> arguments)
        {
            try
            {
                if (arguments == null)
                    throw new ArgumentNullException("missing arguments");

                if (string.IsNullOrEmpty(xslt))
                    throw new ArgumentNullException($"xslt not Loaded in {typeof(XsltTransformation)}");

                XsltArgumentList xsltArguments = null;

                if (arguments != null)
                {
                    xsltArguments = new XsltArgumentList();
                    arguments.ToList().ForEach(
                        x => xsltArguments.AddParam(x.Key, string.Empty, x.Value)
                    );
                }

                if (string.IsNullOrWhiteSpace(xslt))
                    throw new ArgumentException(nameof(xslt));

                if (string.IsNullOrWhiteSpace(source))
                    throw new ArgumentException(nameof(source));

                var settings = new XsltSettings { EnableScript = true };
                var xTransform = new XslCompiledTransform();
                using (var sReader = new StringReader(xslt))
                using (var xReader = XmlReader.Create(sReader))
                    xTransform.Load(xReader, settings, null);

                string result;
                try
                {
                    using (var sReader = new StringReader(source))
                    using (var xReader = XmlReader.Create(sReader))
                    using (var sWriter = new Utf8StringWriter())
                    {
                        var writerSettings = new XmlWriterSettings
                        {
                            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                            Indent = false,
                            IndentChars = "  ",
                        };

                        using (var xWriter = XmlWriter.Create(sWriter, writerSettings))
                        {
                            xTransform.Transform(xReader, xsltArguments, xWriter);
                        }
                        result = sWriter.ToString();
                    }
                }
                catch
                {
                    throw;
                }

                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
