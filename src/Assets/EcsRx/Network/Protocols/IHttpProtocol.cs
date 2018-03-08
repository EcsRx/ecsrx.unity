using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace EcsRx.Net
{
    public interface IHttpProtocol : IProtocol
    {
        IObservable<TOut> Post<TIn, TOut>(HttpRequestMessage<TIn> message) where TIn : struct where TOut : struct;
    }
}
