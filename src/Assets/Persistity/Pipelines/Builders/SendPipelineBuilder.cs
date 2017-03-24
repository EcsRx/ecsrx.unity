using System.Collections.Generic;
using Persistity.Endpoints;
using Persistity.Extensions;
using Persistity.Processors;
using Persistity.Transformers;

namespace Persistity.Pipelines.Builders
{
    public class SendPipelineBuilder
    {
        private ITransformer _transformStep;
        private ISendDataEndpoint _sendDataEndpointStep;
        private IList<IProcessor> _processors;

        public SendPipelineBuilder(ITransformer transformStep)
        {
            _transformStep = transformStep;
            _processors = new List<IProcessor>();
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

        public SendPipelineBuilder SendTo(ISendDataEndpoint sendDataEndpoint)
        {
            _sendDataEndpointStep = sendDataEndpoint;
            return this;
        }

        public ISendDataPipeline Build()
        {
            return new SendDataPipeline(_transformStep, _sendDataEndpointStep, _processors);
        }
    }
}