using Persistity.Encryption;
using Persistity.Serialization;

namespace Persistity.Processors.Encryption
{
    public class EncryptDataProcessor : IProcessor
    {
        public IEncryptor Encryptor { get; private set; }

        public EncryptDataProcessor(IEncryptor encryptor)
        { Encryptor = encryptor; }

        public DataObject Process(DataObject data)
        {
            var encryptedData = Encryptor.Encrypt(data.AsBytes);
            return new DataObject(encryptedData);
        }
    }
}