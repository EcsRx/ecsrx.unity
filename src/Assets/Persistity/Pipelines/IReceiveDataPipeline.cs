using System;

namespace Persistity.Pipelines
{
    public interface IReceiveDataPipeline
    {
        void Execute<T>(object state, Action < T> onSuccess, Action<Exception> onError);
    }
}