using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Crypto;
using EcsRx.ErrorHandle;
using EcsRx.Net;
using EcsRx.Serialize;

namespace EcsRx.Network
{
    public class DefaultHttpProtocol : HttpProtocol
    {
        public DefaultHttpProtocol(IJsonSerialize serialize, IJsonDeserialize deserialize, ICrypto crypto, IHttpErrorHandle errorHandle) : base(serialize, deserialize, crypto, errorHandle)
        {
            Header["Content-Type"] = "application/json";
        }

        protected override string EncodeMessage<TIn>(HttpRequestMessage<TIn> message)
        {
            string data = Serialize.Serialize(message.Data);
            byte[] encryptedData = Crypto.Encryption(data);
            data = Convert.ToBase64String(encryptedData);
            return Serialize.Serialize(data);
        }

        protected override HttpResponseMessage<TOut> DecodeMessage<TOut>(string data)
        {
            var response = new HttpResponseMessage<TOut>();
            var base64Data = Convert.FromBase64String(data);
            var decryptionData = Crypto.Decryption(base64Data);
            response = Deserialize.Deserialize<HttpResponseMessage<TOut>>(decryptionData);
            return response;
        }
    }
}
