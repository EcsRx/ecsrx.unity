using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Crypto;
using EcsRx.ErrorHandle;
using EcsRx.Json;
using EcsRx.Serialize;
using EcsRx.Unity.DataSource;
using EcsRx.Unity.Exception;
using UniRx;
using UnityEngine;

namespace EcsRx.Network
{
    public class HttpProtocol : IHttpProtocol
    {
        public ISerialize Serialize { get; set; }
        public IDeserialize Deserialize { get; set; }
        public ICrypto Crypto { get; set; }

        public HttpProtocol(ISerialize serialize, IDeserialize deserialize, ICrypto crypto)
        {
            Serialize = serialize;
            Deserialize = deserialize;
            Crypto = crypto;
        }

        public virtual string EncodeMessage<TIn>(HttpRequestMessage<TIn> message) where TIn : struct
        {
            string data = Serialize.Serialize(message.Data);
            byte[] encryptedData = Crypto.Encryption(data);
            data = Convert.ToBase64String(encryptedData);
            return Serialize.Serialize(data);
        }

        public virtual HttpResponseMessage<TOut> DecodeMessage<TOut, TResponse>(string data) where TOut : struct where TResponse : HttpResponseMessage<TOut>, new()
        {
            var response = new TResponse();
            var base64Data = Convert.FromBase64String(data);
            var decryptionData = Crypto.Decryption(base64Data);
            response = Deserialize.Deserialize<TResponse>(decryptionData);
            return response;
        }

    }
}
