using System.Collections.Generic;
using System.Xml.Linq;

namespace Persistity.Serialization.Xml
{
    public class XmlConfiguration : SerializationConfiguration<XElement, XElement>
    {
        public static XmlConfiguration Default
        {
            get
            {
                return new XmlConfiguration
                {
                    TypeHandlers = new ITypeHandler<XElement, XElement>[0]
                };
            }
        }
    }
}