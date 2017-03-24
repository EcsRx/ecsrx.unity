using Persistity.Endpoints;
using Persistity.Transformers;

namespace Persistity.Pipelines.Builders
{
    public class PipelineBuilder
    {
        public SendPipelineBuilder TransformWith(ITransformer transformer)
        { return new SendPipelineBuilder(transformer); }

        public ReceivePipelineBuilder RecieveFrom(IReceiveDataEndpoint recieveDataEndpoint)
        { return new ReceivePipelineBuilder(recieveDataEndpoint); }
    }
}