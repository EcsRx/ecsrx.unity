using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Network
{
    public abstract class HttpResponseMessage<T> : IHttpResponseMessage where T : struct
    {
        protected Dictionary<int, string> errorMessages; 
        public abstract T Data { get; }
        public abstract bool IsOK { get; }
        public abstract string ErrorMessage { get; }
        public abstract int ErrorCode { get; }
    }
}
