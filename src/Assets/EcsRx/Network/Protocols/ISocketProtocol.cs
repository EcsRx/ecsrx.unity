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
        Frame EncodeMessage<TIn>(SocketRequestMessage<TIn> message);
        object DecodeMessage(Type type, MemoryStream stream);
    }
}
