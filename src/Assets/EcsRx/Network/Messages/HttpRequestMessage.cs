using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Net
{
    public abstract class HttpRequestMessage<T> : IHttpRequestMessage where T : struct 
    {
        public abstract string Url { get; set; }
        public abstract string Path { get; set; }
        public T Data { get; set; }
        public HttpRequestMessage(T data)
        {
            Data = data;
        }
    }
}
