using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Endpoints;
using Persistity.Processors;
using Persistity.Transformers;

namespace Persistity.Pipelines
{
    public class ReceiveDataPipeline : IReceiveDataPipeline
    {
        public ITransformer Transformer { get; private set; }
        public IEnumerable<IProcessor> Processors { get; private set; }
        public IReceiveDataEndpoint ReceiveFromEndpoint { get; private set; }

        public ReceiveDataPipeline(ITransformer transformer, IReceiveDataEndpoint receiveFromEndpoint, IEnumerable<IProcessor> processors = null)
        {
            Transformer = transformer;
            Processors = processors;
            ReceiveFromEndpoint = receiveFromEndpoint;
        }

        public ReceiveDataPipeline(ITransformer transformer, IReceiveDataEndpoint receiveFromEndpoint, params IProcessor[] processors)
        {
            Transformer = transformer;
            Processors = processors;
            ReceiveFromEndpoint = receiveFromEndpoint;
        }

        public void Execute<T>(Action<T> onSuccess, Action<Exception> onError) where T : new()
        {
            ReceiveFromEndpoint.Execute(x =>
            {
                var output = x;
                if (Processors != null && Processors.Any())
                {
                    foreach (var processor in Processors)
                    { output = processor.Process(output); }
                }

                var model = Transformer.Transform<T>(output);
                onSuccess(model);
            }, onError);
        }
    }
}