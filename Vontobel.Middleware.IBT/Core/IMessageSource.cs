using Vontobel.Middleware.IBT.Contracts.Model;

namespace Vontobel.Middleware.IBT.Core
{
    public interface IMessageSource<T> : ITransactionable where T : IVontobelEntity
    {
        T Read();
    }
}
