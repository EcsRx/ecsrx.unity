using Persistity.Encryption;

namespace Persistity.Processors.Encryption
{
    public class DecryptDataProcessor : IProcessor
    {
        public IEncryptor Encryptor { get; private set; }

        public DecryptDataProcessor(IEncryptor encryptor)
        { Encryptor = encryptor; }

        public byte[] Process(byte[] data)
        { return Encryptor.Decrypt(data); }
    }
}