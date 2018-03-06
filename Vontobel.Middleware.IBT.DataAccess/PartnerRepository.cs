using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vontobel.Middleware.IBT.Contracts.Model;
using Vontobel.Middleware.IBT.Contracts.Repositories;
using Vontobel.Middleware.IBT.Entities;

namespace Vontobel.Middleware.IBT.DataAccess
{
    public class PartnerRepository : IPartnerRepository
    {
        public IList<Contracts.Model.Partner> GetSubscriptions(int eventCode, string protocolType, string messageId)
        {
            List<Contracts.Model.Partner> eligiblePartners = new List<Contracts.Model.Partner>();
            var context = new VontobelDBConnection();

            var subscribedPartners = (from p in context.Partners
                                      join ps in context.PartnerSubscriptions
                                      on p equals ps.Partner
                                      where ps.MessageProtocol.Name == protocolType &&
                                             ps.Event.Code == eventCode
                                      select ps);

            foreach (var subscribedPartner in subscribedPartners)
            {
                var successfulLog = subscribedPartner.SubscriptionMessageLogs.Where(x => x.Picked == 1);
                if (successfulLog.Select(x => x.MessageId).Contains(messageId))
                    continue;

                var eligiblePartner = new Contracts.Model.Partner()
                {
                    Name = subscribedPartner.Partner.Name,
                    Email = subscribedPartner.Partner.Email,
                    Id = subscribedPartner.Partner.Id,
                    Parameters = new Dictionary<string, string>()
                };
                foreach (var parameter in subscribedPartner.PartnerSubscriptionParameters)
                {
                    eligiblePartner.Parameters.Add(parameter.Key, parameter.Value);
                }

                eligiblePartners.Add(eligiblePartner);
            }

            return eligiblePartners;
        }

        public void UpdateSubscriptionLog(IList<string> partnerIds, int eventCode, string protocolType, string messageId)
        {
            if (partnerIds == null || string.IsNullOrEmpty(protocolType))
                return;
            try
            { 
                var context = new VontobelDBConnection();

                var subscribedPartners = (from p in context.Partners
                                          join ps in context.PartnerSubscriptions
                                          on p equals ps.Partner
                                          where ps.MessageProtocol.Name == protocolType &&
                                                 ps.Event.Code == eventCode
                                                 && partnerIds.Contains(p.Id)
                                          select ps);

                subscribedPartners.ToList().ForEach(
                    x => context.SubscriptionMessageLogs.Add(
                        new SubscriptionMessageLog
                        {
                            Id = Guid.NewGuid().ToString("N"),
                            PartnerSubscriptionId = x.Id,
                            Picked = 1,
                            MessageId = messageId
                        }));

                context.SaveChanges();
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}