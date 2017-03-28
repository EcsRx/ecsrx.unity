using System;

namespace Persistity.Pipelines
{
    public interface IReceiveDataPipeline
    {
        void Execute<T>(Action<T> onSuccess, Action<Exception> onError);
    }
}