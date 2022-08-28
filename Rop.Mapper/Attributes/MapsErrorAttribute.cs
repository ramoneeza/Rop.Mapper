using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Mapper.Attributes
{
    public class MapsErrorAttribute:Attribute,IMapsAttribute
    {
        public Attribute AttributeOrigin { get; }
        public Property AttributeProperty { get; }

        public MapsErrorAttribute(Attribute attributeOrigin, Property attributeProperty)
        {
            AttributeOrigin = attributeOrigin;
            AttributeProperty = attributeProperty;
        }
    }
}
