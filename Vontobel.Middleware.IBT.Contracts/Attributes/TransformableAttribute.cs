using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vontobel.Middleware.IBT.Contracts.Attributes
{
    public class TransformableAttribute : Attribute
    {
        public string Name { get; set; }
        public TransformableAttribute(string name)
        {
            Name = name;
        }
    }
}
