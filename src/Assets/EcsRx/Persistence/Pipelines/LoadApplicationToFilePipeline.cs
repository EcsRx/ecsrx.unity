using EcsRx.Persistence.Endpoints;
using EcsRx.Persistence.Transformers;
using Persistity.Pipelines;
using Persistity.Serialization.Json;

namespace EcsRx.Persistence.Pipelines
{
    public class LoadApplicationToFilePipeline : ReceiveDataPipeline
    {
        public LoadApplicationToFilePipeline(IJsonDeserializer deserializer, IApplicationConfigFileEndpoint endpoint, IEntityDataTransformer transformer)
            : base(deserializer, endpoint, null, new[] { transformer })
        {
        }
    }
}