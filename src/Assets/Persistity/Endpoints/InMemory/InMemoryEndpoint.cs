using System;

namespace Persistity.Endpoints.InMemory
{
    public class InMemoryEndpoint : IReceiveDataEndpoint, ISendDataEndpoint
    {
        private DataObject _inMemoryStore;

        public void Execute(Action<DataObject> onSuccess, Action<Exception> onError)
        { onSuccess(_inMemoryStore); }

        public void Execute(DataObject data, Action<object> onSuccess, Action<Exception> onError)
        {
            _inMemoryStore = data;
            onSuccess(null);
        }
    }
}