using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Persistity.Encryption
{
    public class AesEncryptor : IEncryptor
    {
        public int KeySize { get; private set; }
        public int Iterations { get; private set; }
        public string Password { get; private set; }

        public AesEncryptor(string password, int keySize = 256, int iterations = 1000)
        {
            Password = password;
            KeySize = keySize;
            Iterations = iterations;
        }

       public byte[] Encrypt(byte[] data)
        {
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var password = new Rfc2898DeriveBytes(Password, saltStringBytes, Iterations);
            var keyBytes = password.GetBytes(KeySize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = 256;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(data, 0, data.Length);
                            cryptoStream.FlushFinalBlock();

                            var cipherTextBytes = saltStringBytes;
                            cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                            return cipherTextBytes;
                        }
                    }
                }
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            var saltStringBytes = data.Take(KeySize / 8).ToArray();
            var ivStringBytes = data.Skip(KeySize / 8).Take(KeySize / 8).ToArray();
            var cipherTextBytes = data.Skip((KeySize / 8) * 2).Take(data.Length - ((KeySize / 8) * 2)).ToArray();

            var password = new Rfc2898DeriveBytes(Password, saltStringBytes, Iterations);
            var keyBytes = password.GetBytes(KeySize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = 256;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                {
                    using (var memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            var plainTextBytes = new byte[cipherTextBytes.Length];
                            var readCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return plainTextBytes.Take(readCount).ToArray();
                        }
                    }
                }
            }
        }

        public byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32];
            var rngCsp = new RNGCryptoServiceProvider();
            rngCsp.GetBytes(randomBytes);
            return randomBytes;
        }
    }
}