using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public interface IRequestMessage<out T> : INetworkMessage
    {
        T Data { get; }
    }
}
