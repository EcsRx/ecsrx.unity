using System;
using Persistity.Serialization;

namespace Persistity.Endpoints
{
    public interface ISendDataEndpoint
    {
        void Execute(DataObject data, Action<object> onSuccess, Action<Exception> onError);
    }
}