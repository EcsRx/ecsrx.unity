using EcsRx.Persistence.Endpoints;
using Persistity.Pipelines;
using Persistity.Serialization.Json;

namespace EcsRx.Persistence.Pipelines
{
    public class SaveApplicationDatabaseToFilePipeline : SendDataPipeline, ISaveApplicationDatabaseToFilePipeline
    {
        public SaveApplicationDatabaseToFilePipeline(IJsonSerializer serializer, IApplicationDatabaseFileEndpoint endpoint)
            : base(serializer, endpoint)
        {
        }
    }
}