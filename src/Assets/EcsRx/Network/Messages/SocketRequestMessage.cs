using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Network
{
    public abstract class SocketRequestMessage<T> : IRequestMessage<T>
    {
        public T Data { get; set; }

        protected SocketRequestMessage(T data)
        {
            Data = data;
        }

    }
}
