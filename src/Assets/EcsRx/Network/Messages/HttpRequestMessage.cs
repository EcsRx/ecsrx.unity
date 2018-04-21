using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Network
{
    public abstract class HttpRequestMessage<T> : IRequestMessage<T> where T : struct 
    {
        public abstract string Path { get; set; }
        public T Data { get; set; }

        protected HttpRequestMessage(T data)
        {
            Data = data;
        }

    }
}
