using System;

namespace Persistity.Pipelines
{
    public interface IReceiveDataPipeline
    {
        void Execute<TOut>(Action<TOut> onSuccess, Action<Exception> onError) where TOut : new();
    }
}