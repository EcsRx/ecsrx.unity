using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using cocosocket4unity;
using EcsRx.Crypto;
using EcsRx.Serialize;

namespace EcsRx.Network
{
    public class SocketProtocol : ISocketProtocol
    {
        public ISerialize Serialize { get; set; }
        public IDeserialize Deserialize { get; set; }
        public ICrypto Crypto { get; set; }

        public SocketProtocol(ISerialize serialize, IDeserialize deserialize, ICrypto crypto)
        {
            Serialize = serialize;
            Deserialize = deserialize;
            Crypto = crypto;
        }

        public Frame EncodeMessage<TIn>(SocketRequestMessage<TIn> message) where TIn : struct
        {
            var serialize = Serialize as IProtobufSerialize;
            byte[] bs = serialize.Serialize(message.Data);
            Frame f = new Frame(512);
            f.PutShort(MessageQueueHandler.GetProtocolCMD(message.Data.GetType()));
            byte[] encryptedData = Crypto.Encryption(Encoding.UTF8.GetString(bs));
            f.PutBytes(encryptedData);
            f.End();
            return f;
        }

        public SocketResponseMessage<TOut> DecodeMessage<TOut, TResponse>(Type type, Stream stream) where TOut : struct where TResponse : SocketResponseMessage<TOut>, new()
        {
            var desrialize = Deserialize as IProtobufDeserialize;
            object obj = desrialize.Deserialize(type, stream);

        }
    }
}