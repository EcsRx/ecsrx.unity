using System;
using EcsRx.Persistence.Database;
using Persistity;
using Persistity.Endpoints;

namespace EcsRx.Persistence.Endpoints
{
    public class ApplicationDatabaseEndpoint : ISendDataEndpoint, IReceiveDataEndpoint
    {
        public ApplicationDatabaseBehaviour ApplicationDatabaseBehaviour { get; private set; }

        public ApplicationDatabaseEndpoint(ApplicationDatabaseBehaviour applicationDatabaseBehaviour)
        {
            ApplicationDatabaseBehaviour = applicationDatabaseBehaviour;
        }

        public void Execute(DataObject data, Action<object> onSuccess, Action<Exception> onError)
        {

            //ApplicationDatabaseBehaviour.ApplicationData.EntityData.Add();
        }

        public void Execute(Action<DataObject> onSuccess, Action<Exception> onError)
        {
            throw new NotImplementedException();
        }
    }
}