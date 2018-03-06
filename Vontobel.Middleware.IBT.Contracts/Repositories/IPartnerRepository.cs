using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Vontobel.Middleware.IBT.Contracts.Repositories
{
    public interface IPartnerRepository
    {
        IList<Contracts.Model.Partner> GetSubscriptions(int eventCode, string protocolType, string messageId);
        void UpdateSubscriptionLog(IList<string> partnerIds, int eventCode, string protocolType, string messageId);
    }
}
