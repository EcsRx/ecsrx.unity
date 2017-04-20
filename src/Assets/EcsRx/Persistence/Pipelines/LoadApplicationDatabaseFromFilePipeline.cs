using EcsRx.Persistence.Endpoints;
using Persistity.Pipelines;
using Persistity.Serialization.Json;

namespace EcsRx.Persistence.Pipelines
{
    public class LoadApplicationDatabaseFromFilePipeline : ReceiveDataPipeline, ILoadApplicationDatabaseFromFilePipeline
    {
        public LoadApplicationDatabaseFromFilePipeline(IJsonDeserializer deserializer, IApplicationDatabaseFileEndpoint endpoint)
            : base(deserializer, endpoint)
        {
        }
    }
}