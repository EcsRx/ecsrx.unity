using System.Collections.Generic;
using System.IO;

namespace Persistity.Serialization.Binary
{
    public class BinaryConfiguration : SerializationConfiguration
    {
        public IEnumerable<ITypeHandler<BinaryWriter, BinaryReader>> TypeHandlers { get; set; }

        public static BinaryConfiguration Default
        {
            get
            {
                return new BinaryConfiguration
                {
                    TypeHandlers = new ITypeHandler<BinaryWriter, BinaryReader>[0]
                };
            }
        }
    }
}