using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Vontobel.Middleware.IBT.Common;
using Vontobel.Middleware.IBT.Contracts;
using Vontobel.Middleware.IBT.Contracts.Model;
using Vontobel.Middleware.IBT.Core;

namespace Vontobel.Middleware.IBT.MessageSources.File
{
    public class IBTXmlFileSource : IMessageSource<DataMessage>
    {
        IBTDirectoryInformation directoryInfo;
        private FileInfo file;

        public IBTXmlFileSource(IBTDirectoryInformation directoryInfo)
        {
            this.directoryInfo = directoryInfo;

            CreateDirectoryIfNotExists(directoryInfo.Failure);
            CreateDirectoryIfNotExists(directoryInfo.Success);
        }

        void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void BeginTransaction()
        {
            file = null;
        }

        public void Commit()
        {
            if (file != null)
               System.IO.File.Move(file.FullName, Path.Combine(directoryInfo.Success, file.Name));
        }

        public DataMessage Read()
        {
            file = new DirectoryInfo(directoryInfo.Root).GetFiles().OrderByDescending(x => x.CreationTime).FirstOrDefault();

            if (file == null)
                return null;

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(file.FullName);

                Log<IBTXmlFileSource>.Info($"Processing IBT Message: {file.FullName}");

                var eventType = xmlDoc.SelectNodes("/IBTTermSheet/Events/Event/EventType");
                var schemeCode = xmlDoc.SelectNodes("/IBTTermSheet/Instrument/InstrumentIds/InstrumentId/IdSchemeCode[text()='I-']");
                XmlNodeList isin = null;
                if (schemeCode != null && schemeCode.Count > 0)
                {
                    isin = schemeCode[0].ParentNode.SelectNodes("IdValue");
                }

                var message = new DataMessage
                {
                    Content = xmlDoc.OuterXml,
                    Id = Guid.NewGuid().ToString(),
                    Parameters = new Dictionary<string, string>()
                    {
                        { "EventType", eventType[0].InnerText },
                        { "ISIN", (isin == null || isin.Count == 0)  ? string.Empty : isin[0].InnerText },
                    }
                    
                };
                
                return message;
            }
            catch(Exception e)
            {
                Log<IBTXmlFileSource>.Error(e);
                throw;
            }
        }

        public void Rollback()
        {
            if (file != null)
                System.IO.File.Move(file.FullName, Path.Combine(directoryInfo.Failure,file.Name));
        }
    }
}