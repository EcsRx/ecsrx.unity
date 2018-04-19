using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace EcsRx.Network
{
    public interface IHttpProtocol : IProtocol
    {
       string EncodeMessage<TIn>(HttpRequestMessage<TIn> message) where TIn : struct;
       HttpResponseMessage<TOut> DecodeMessage<TOut, TResponse>(string data) where TOut : struct where TResponse: HttpResponseMessage<TOut>, new();
    }
}
