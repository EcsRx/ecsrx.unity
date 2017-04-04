using System.IO;

namespace EcsRx.Persistence.Extensions
{
    public static class BinaryIOExtensions
    {
        public static void WriteByteArray(this BinaryWriter writer, byte[] array)
        {
            writer.Write(array.Length);
            writer.Write(array);
        }

        public static byte[] ReadByteArray(this BinaryReader reader)
        {
            var size = reader.ReadInt32();
            return reader.ReadBytes(size);
        }
    }
}