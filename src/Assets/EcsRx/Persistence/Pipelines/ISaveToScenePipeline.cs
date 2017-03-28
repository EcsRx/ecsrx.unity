using EcsRx.Persistence.Endpoints;
using EcsRx.Persistence.Transformers;
using Persistity.Pipelines;
using Persistity.Serialization.Json;

namespace EcsRx.Persistence.Pipelines
{
    public class SaveApplicationToFilePipeline : SendDataPipeline
    {
        public SaveApplicationToFilePipeline(IJsonSerializer serializer, IApplicationConfigFileEndpoint endpoint, IEntityDataTransformer transformer)
            : base(serializer, endpoint, null, new []{ transformer })
        {
        }
    }
}