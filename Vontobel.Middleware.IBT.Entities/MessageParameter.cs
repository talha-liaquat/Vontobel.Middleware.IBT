//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vontobel.Middleware.IBT.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class MessageParameter
    {
        public string Id { get; set; }
        public string MessageId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual Message Message { get; set; }
    }
}
