using System;

namespace Persistity.Pipelines
{
    public interface ISendDataPipeline
    {
        void Execute<TIn>(TIn data, Action<object> onSuccess, Action<Exception> onError) where TIn : new();
    }
}