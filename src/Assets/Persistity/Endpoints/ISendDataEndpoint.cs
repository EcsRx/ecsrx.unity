using System;

namespace Persistity.Endpoints
{
    public interface ISendDataEndpoint
    {
        void Execute(byte[] data, Action<object> onSuccess, Action<Exception> onError);
    }
}