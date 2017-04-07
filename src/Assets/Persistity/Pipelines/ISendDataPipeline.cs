using System;

namespace Persistity.Pipelines
{
    public interface ISendDataPipeline
    {
        void Execute<T>(T data, object state, Action<object> onSuccess, Action<Exception> onError);
    }
}