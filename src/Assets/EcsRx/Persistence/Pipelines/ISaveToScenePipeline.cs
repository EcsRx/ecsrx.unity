using EcsRx.Persistence.Endpoints;
using Persistity.Pipelines;
using Persistity.Serialization.Json;

namespace EcsRx.Persistence.Pipelines
{
    public class SaveApplicationToFilePipeline : SendDataPipeline
    {
        public SaveApplicationToFilePipeline(IJsonSerializer serializer, IApplicationConfigFileEndpoint endpoint)
            : base(serializer, endpoint)
        {
        }
    }
}