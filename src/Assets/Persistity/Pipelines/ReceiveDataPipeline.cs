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

        public virtual void Execute<T>(object state, Action<T> onSuccess, Action<Exception> onError)
        {
            ReceiveFromEndpoint.Execute(x =>
            {
                var output = RunProcessors(x);
                var model = Deserializer.Deserialize(output);
                var transformedModel = RunTransformers(model);

                onSuccess((T)transformedModel);
            }, onError);
        }

        protected object RunTransformers(object data)
        {
            if (Transformers != null)
            {
                foreach (var convertor in Transformers)
                { data = convertor.TransformFrom(data); }
            }
            return data;
        }

        protected DataObject RunProcessors(DataObject data)
        {
            if (Processors != null && Processors.Any())
            {
                foreach (var processor in Processors)
                { data = processor.Process(data); }
            }
            return data;
        }
    }
}