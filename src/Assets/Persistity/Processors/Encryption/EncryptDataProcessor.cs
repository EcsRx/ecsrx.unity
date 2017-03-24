using Persistity.Encryption;

namespace Persistity.Processors.Encryption
{
    public class EncryptDataProcessor : IProcessor
    {
        public IEncryptor Encryptor { get; private set; }

        public EncryptDataProcessor(IEncryptor encryptor)
        { Encryptor = encryptor; }

        public byte[] Process(byte[] data)
        { return Encryptor.Encrypt(data); }
    }
}