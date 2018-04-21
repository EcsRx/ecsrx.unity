using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using cocosocket4unity;
using UniRx;

namespace EcsRx.Network
{
    public interface ISocketProtocol : IProtocol
    {
       Frame EncodeMessage<TIn>(SocketRequestMessage<TIn> message) where TIn : struct;
       SocketResponseMessage<TOut> DecodeMessage<TOut, TResponse>(Type type, Stream stream) where TOut : struct where TResponse: SocketResponseMessage<TOut>, new();
    }
}
