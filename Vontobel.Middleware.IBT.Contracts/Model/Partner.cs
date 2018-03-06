using System.Collections.Generic;
using Vontobel.Middleware.IBT.Contracts.Attributes;

namespace Vontobel.Middleware.IBT.Contracts.Model
{
    public class Partner : IVontobelEntity
    {
        public string Id { get; set; }
        [TransformableAttribute(name: "PartnerName")]
        public string Name { get; set; }
        public string Email { get; set; }
        public IList<Event> EventSubscriptions { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
    }
}
