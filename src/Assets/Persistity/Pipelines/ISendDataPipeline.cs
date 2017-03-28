using System;

namespace Persistity.Pipelines
{
    public interface ISendDataPipeline
    {
        void Execute<T>(T data, Action<object> onSuccess, Action<Exception> onError);
    }
}