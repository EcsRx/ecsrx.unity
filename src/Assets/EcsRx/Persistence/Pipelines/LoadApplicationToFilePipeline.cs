using EcsRx.Persistence.Endpoints;
using Persistity.Pipelines;
using Persistity.Serialization.Json;

namespace EcsRx.Persistence.Pipelines
{
    public class LoadApplicationToFilePipeline : ReceiveDataPipeline
    {
        public LoadApplicationToFilePipeline(IJsonDeserializer deserializer, IApplicationConfigFileEndpoint endpoint)
            : base(deserializer, endpoint)
        {
        }
    }
}