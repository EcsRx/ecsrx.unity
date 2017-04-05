using System;
using EcsRx.Persistence.Database;
using Persistity;
using Persistity.Endpoints;

namespace EcsRx.Persistence.Endpoints
{
    public class ApplicationDatabaseEndpoint : ISendDataEndpoint, IReceiveDataEndpoint
    {
        public ApplicationDatabase ApplicationDatabase { get; private set; }

        public ApplicationDatabaseEndpoint(ApplicationDatabase applicationDatabase)
        {
            ApplicationDatabase = applicationDatabase;
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