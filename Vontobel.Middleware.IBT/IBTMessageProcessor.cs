using System;
using System.Collections.Generic;
using Vontobel.Middleware.IBT.Common;
using Vontobel.Middleware.IBT.Contracts.Model;
using Vontobel.Middleware.IBT.Core;

namespace Vontobel.Middleware.IBT
{
    public class IBTMessageProcessor<T,K> where T : IVontobelEntity where K : ITransformation
    {
        IMessageSource<T> source;
        IList<IMessageSink<T,K>> sinks;

        public IBTMessageProcessor(IMessageSource<T> source, IList<IMessageSink<T,K>> sinks)
        {
            this.source = source;
            this.sinks = sinks;
        }

        public void Process(K transformation)
        {
            try
            {
                T message;
                source.BeginTransaction();
                message = source.Read();

                if (message == null)
                    return;

                foreach (var sink in sinks)
                {
                    try
                    {
                        sink.BeginTransaction();
                        sink.Write(message, transformation);
                        sink.Commit();
                    }
                    catch
                    {
                        sink.Rollback();
                        throw;
                    }
                }
                source.Commit();
            }
            catch
            {
                source.Rollback();
                throw;
            }
        }
    }
}
