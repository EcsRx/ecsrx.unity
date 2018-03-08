using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Json;

namespace EcsRx.Net
{
    public class HttpResponseMessage<T> : IResponseMessage where T : struct 
    {
        public virtual T Data { get { return new T();} }
        public virtual bool IsOK { get { return true; } }
        public virtual string ResultMessage {
            get { return ""; }
        }
        public virtual int ResultCode { get { return -1; } }
    }
}
