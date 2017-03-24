using System;

namespace Persistity.Endpoints
{
    public interface IReceiveDataEndpoint
    {
        void Execute(Action<byte[]> onSuccess, Action<Exception> onError);
    }
}