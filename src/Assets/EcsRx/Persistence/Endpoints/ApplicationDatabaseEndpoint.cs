using System;
using EcsRx.Persistence.Database;
using Persistity;
using Persistity.Endpoints;

namespace EcsRx.Persistence.Endpoints
{
    public class ApplicationDatabaseEndpoint : ISendDataEndpoint, IReceiveDataEndpoint
    {
        public ApplicationDatabaseBehaviour ApplicationDatabaseBehaviour { get; set; }

        public void Execute(DataObject data, Action<object> onSuccess, Action<Exception> onError)
        {
            throw new NotImplementedException();
        }

        public void Execute(Action<DataObject> onSuccess, Action<Exception> onError)
        {
            throw new NotImplementedException();
        }
    }
}