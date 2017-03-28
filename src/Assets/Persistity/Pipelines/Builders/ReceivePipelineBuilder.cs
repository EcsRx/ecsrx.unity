using System.Collections.Generic;
using Persistity.Endpoints;
using Persistity.Extensions;
using Persistity.Processors;
using Persistity.Serialization;
using Persistity.Transformers;

namespace Persistity.Pipelines.Builders
{
    public class ReceivePipelineBuilder
    {
        private readonly IReceiveDataEndpoint _receiveDataEndpointStep;
        private readonly IList<IProcessor> _processors;
        private readonly IList<ITransformer> _transformers;
        private IDeserializer _deserializer;

        public ReceivePipelineBuilder(IReceiveDataEndpoint receiveDataEndpointStep)
        {
            _receiveDataEndpointStep = receiveDataEndpointStep;
            _processors = new List<IProcessor>();
            _transformers = new List<ITransformer>();
        }

        public ReceivePipelineBuilder ProcessWith(IProcessor processor)
        {
            _processors.Add(processor);
            return this;
        }

        public ReceivePipelineBuilder ProcessWith(params IProcessor[] processors)
        {
            _processors.AddRange(processors);
            return this;
        }

        public ReceivePipelineBuilder TransformWith(ITransformer transformer)
        {
            _transformers.Add(transformer);
            return this;
        }

        public ReceivePipelineBuilder TransformWith(params ITransformer[] transformers)
        {
            _transformers.AddRange(transformers);
            return this;
        }

        public ReceivePipelineBuilder DeserializeWith(IDeserializer deserializer)
        {
            _deserializer = deserializer;
            return this;
        }

        public IReceiveDataPipeline Build()
        {
            return new ReceiveDataPipeline(_deserializer, _receiveDataEndpointStep, _processors, _transformers);
        }
    }
}