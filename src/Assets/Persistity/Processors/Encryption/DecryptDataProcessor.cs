using Persistity.Encryption;
using Persistity.Serialization;

namespace Persistity.Processors.Encryption
{
    public class DecryptDataProcessor : IProcessor
    {
        public IEncryptor Encryptor { get; private set; }

        public DecryptDataProcessor(IEncryptor encryptor)
        { Encryptor = encryptor; }

        public DataObject Process(DataObject data)
        {
            var decryptedData = Encryptor.Decrypt(data.AsBytes);
            return new DataObject(decryptedData);
        }
    }
}