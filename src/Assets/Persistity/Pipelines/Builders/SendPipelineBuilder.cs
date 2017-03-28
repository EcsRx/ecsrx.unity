using System.Collections.Generic;
using Persistity.Endpoints;
using Persistity.Extensions;
using Persistity.Processors;
using Persistity.Serialization;
using Persistity.Transformers;

namespace Persistity.Pipelines.Builders
{
    public class SendPipelineBuilder
    {
        private readonly ISerializer _serializer;
        private readonly IList<IProcessor> _processors;
        private readonly IList<ITransformer> _transformers;
        private ISendDataEndpoint _sendDataEndpointStep;

        public SendPipelineBuilder(ISerializer serializer)
        {
            _serializer = serializer;
            _processors = new List<IProcessor>();
            _transformers = new List<ITransformer>();
    }

        public SendPipelineBuilder ProcessWith(IProcessor processor)
        {
            _processors.Add(processor);
            return this;
        }

        public SendPipelineBuilder ProcessWith(params IProcessor[] processors)
        {
            _processors.AddRange(processors);
            return this;
        }

        public SendPipelineBuilder TransformWith(ITransformer transformer)
        {
            _transformers.Add(transformer);
            return this;
        }

        public SendPipelineBuilder TransformWith(params ITransformer[] transformers)
        {
            _transformers.AddRange(transformers);
            return this;
        }

        public SendPipelineBuilder SendTo(ISendDataEndpoint sendDataEndpoint)
        {
            _sendDataEndpointStep = sendDataEndpoint;
            return this;
        }

        public ISendDataPipeline Build()
        {
            return new SendDataPipeline(_serializer, _sendDataEndpointStep, _processors, _transformers);
        }
    }
}