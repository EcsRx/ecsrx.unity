using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Persistity.Serialization.Xml
{
    public class XmlConfiguration : SerializationConfiguration
    {
        public Encoding Encoder { get; set; }
        public IEnumerable<ITypeHandler<XElement, XElement>> TypeHandlers { get; set; }

        public static XmlConfiguration Default
        {
            get
            {
                return new XmlConfiguration
                {
                    Encoder = Encoding.Default,
                    TypeHandlers = new ITypeHandler<XElement, XElement>[0]
                };
            }
        }
    }
}