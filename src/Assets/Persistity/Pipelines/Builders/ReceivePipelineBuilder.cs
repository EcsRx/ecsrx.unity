using System.Collections.Generic;
using Persistity.Endpoints;
using Persistity.Extensions;
using Persistity.Processors;
using Persistity.Transformers;

namespace Persistity.Pipelines.Builders
{
    public class ReceivePipelineBuilder
    {
        private IReceiveDataEndpoint _receiveDataEndpointStep;
        private ITransformer _transformStep;
        private IList<IProcessor> _processors;

        public ReceivePipelineBuilder(IReceiveDataEndpoint receiveDataEndpointStep)
        {
            _receiveDataEndpointStep = receiveDataEndpointStep;
            _processors = new List<IProcessor>();
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
            _transformStep = transformer;
            return this;
        }

        public IReceiveDataPipeline Build()
        {
            return new ReceiveDataPipeline(_transformStep, _receiveDataEndpointStep, _processors);
        }
    }
}