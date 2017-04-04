using System.IO;

namespace Persistity.Serialization.Binary
{
    public class BinaryConfiguration : SerializationConfiguration<BinaryWriter,BinaryReader>
    {
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