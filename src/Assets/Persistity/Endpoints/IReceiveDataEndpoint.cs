using System;
using Persistity.Serialization;

namespace Persistity.Endpoints
{
    public interface IReceiveDataEndpoint
    {
        void Execute(Action<DataObject> onSuccess, Action<Exception> onError);
    }
}