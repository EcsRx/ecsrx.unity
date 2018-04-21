using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public interface IResponseMessage<out T> : INetworkMessage
    {
        T Data { get; }
    }
}
