using System.Collections.Generic;
using Vontobel.Middleware.IBT.Common;
using Vontobel.Middleware.IBT.Contracts;
using Vontobel.Middleware.IBT.Contracts.Model;
using Vontobel.Middleware.IBT.Core;
using Vontobel.Middleware.IBT.MessageSinks.Email;
using Vontobel.Middleware.IBT.MessageSinks.File;
using Vontobel.Middleware.IBT.MessageSources.File;
using Vontobel.Middleware.IBT.Transformation;
using System;
using Vontobel.Middleware.IBT.MessageSinks.Database;
using Vontobel.Middleware.IBT.DataAccess;
using Vontobel.Middleware.IBT.MessageSources.Database;
using System.Threading;
using System.IO;
using System.Data.SqlClient;
using System.Data.EntityClient;

namespace Vontobel.Middleware.IBT.Console
{
    class Program
    {
        private static void TimerCallback(Object o)
        {
            Process();
        }
        static void Process()
        {
            try
            {
                var source1 = new IBTXmlFileSource(
                    new IBTDirectoryInformation
                    {
                        Root = SystemConfig.Default["Root"],
                        Failure = SystemConfig.Default["Failure"],
                        Success = SystemConfig.Default["Success"]
                    });

                var sinks1 = new List<IMessageSink<DataMessage, XsltTransformation>> {
                    new MessageDatabaseSink(new MessageRepository())};

                var processor = new IBTMessageProcessor<DataMessage, XsltTransformation>(source1, sinks1);
                processor.Process((XsltTransformation)TransformationFactory.Xslt);

                var source2 = new MessageDatabaseSource(new MessageRepository());
                var sinks2 = new List<IMessageSink<DataMessage, XsltTransformation>> {
                    new PartnerEmailSink(new PartnerRepository()),
                    new PartnerXmlFileSink(new PartnerRepository())
                    };

                processor = new IBTMessageProcessor<DataMessage, XsltTransformation>(source2, sinks2);
                processor.Process((XsltTransformation)TransformationFactory.Xslt);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        static bool Initialization()
        {
            bool isSuccess = true;
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine("Vontobel Middleware");
            System.Console.WriteLine("-------------------");

            System.Console.WriteLine("Initializing...");
            var rootFolder = SystemConfig.Default["Root"];

            System.Console.Write($"Verifying root folder: {rootFolder}.");
            if(Directory.Exists(rootFolder))
            {
                System.Console.WriteLine($" Done");
            }
            else 
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($" Failed");
                System.Console.WriteLine($"Root folder not found, which will contain the message xml files e.g. IBT.xml");
                isSuccess = false;
            }

            if (isSuccess)
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["VontobelDBConnection"]?.ConnectionString;
                System.Console.Write($"Verifying DB Connectivity. Connection String: 'VontobelDBConnection' ");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    var buildder = new EntityConnectionStringBuilder(connectionString);
                    var connection = new SqlConnection(buildder.ProviderConnectionString);
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine($" Failed");
                        System.Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    System.Console.WriteLine($" Done");
                }
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine($" Failed");
                    System.Console.WriteLine($"Connection string not found in configuration file");
                    isSuccess = false;
                }
            }
            System.Console.ForegroundColor = ConsoleColor.White;
            return isSuccess;
        }

        static void Main(string[] args)
        {
            if (!Initialization())
                return;

            System.Console.WriteLine($"Listeners Started.");

            Timer t = new Timer(TimerCallback, null, 0, 10000);
            System.Console.ReadLine();
        }
    }
}