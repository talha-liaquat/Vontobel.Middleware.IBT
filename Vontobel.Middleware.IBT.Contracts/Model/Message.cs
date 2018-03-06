using System;
using System.Collections.Generic;

namespace Vontobel.Middleware.IBT.Contracts.Model
{
    public class DataMessage : IVontobelEntity
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
    }
}
