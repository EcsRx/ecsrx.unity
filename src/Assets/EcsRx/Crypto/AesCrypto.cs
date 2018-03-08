using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Crypto
{
    public class AesCrypto : ICrypto
    {
        public string Key { get; set; }
        public string InitialVector { get; set; }

        public byte[] Encryption(string data)
        {
            return AesUtility.Encrption(data, Key, InitialVector);
        }

        public string Decryption(byte[] data)
        {
            return AesUtility.Decrption(data, Key, InitialVector);
        }
    }
}
