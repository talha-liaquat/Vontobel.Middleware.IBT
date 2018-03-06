using Vontobel.Middleware.IBT.Contracts.Model;

namespace Vontobel.Middleware.IBT.Core
{
    public interface IMessageSink<T, K> : ITransactionable where T : IVontobelEntity where K : ITransformation
    {
        void Write(T message, K transformation);
    }
}
