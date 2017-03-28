using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Endpoints;
using Persistity.Processors;
using Persistity.Serialization;
using Persistity.Transformers;

namespace Persistity.Pipelines
{
    public class ReceiveDataPipeline : IReceiveDataPipeline
    {
        public IDeserializer Deserializer { get; private set; }
        public IEnumerable<ITransformer> Transformers { get; private set; }
        public IEnumerable<IProcessor> Processors { get; private set; }
        public IReceiveDataEndpoint ReceiveFromEndpoint { get; private set; }

        public ReceiveDataPipeline(IDeserializer deserializer, IReceiveDataEndpoint receiveFromEndpoint, IEnumerable<IProcessor> processors = null, IEnumerable<ITransformer> transformers = null)
        {
            Deserializer = deserializer;
            Processors = processors;
            Transformers = transformers;
            ReceiveFromEndpoint = receiveFromEndpoint;
        }

        public ReceiveDataPipeline(IDeserializer deserializer, IReceiveDataEndpoint receiveFromEndpoint, params IProcessor[] processors) : this(deserializer, receiveFromEndpoint, processors, null)
        {}

        public void Execute<T>(Action<T> onSuccess, Action<Exception> onError)
        {
            ReceiveFromEndpoint.Execute(x =>
            {
                var output = x;
                if (Processors != null && Processors.Any())
                {
                    foreach (var processor in Processors)
                    { output = processor.Process(output); }
                }

                object model = Deserializer.Deserialize(output);
                if (Transformers != null)
                {
                    foreach (var convertor in Transformers)
                    { model = convertor.TransformFrom(model); }
                }

                onSuccess((T)model);
            }, onError);
        }
    }
}