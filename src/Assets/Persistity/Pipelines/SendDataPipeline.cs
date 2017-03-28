using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Endpoints;
using Persistity.Processors;
using Persistity.Serialization;
using Persistity.Transformers;

namespace Persistity.Pipelines
{
    public class SendDataPipeline : ISendDataPipeline
    {
        public ISerializer Serializer { get; private set; }
        public IEnumerable<ITransformer> Transformers { get; private set; }
        public IEnumerable<IProcessor> Processors { get; private set; }
        public ISendDataEndpoint SendToEndpoint { get; private set; }

        public SendDataPipeline(ISerializer serializer, ISendDataEndpoint sendToEndpoint, IEnumerable<IProcessor> processors = null, IEnumerable<ITransformer> transformers = null)
        {
            Serializer = serializer;
            Processors = processors;
            Transformers = transformers;
            SendToEndpoint = sendToEndpoint;
        }

        public void Execute<T>(T data, Action<object> onSuccess, Action<Exception> onError)
        {
            object obj = data;

            if (Transformers != null)
            {
                foreach(var convertor in Transformers)
                { obj = convertor.TransformTo(obj); }
            }

            var output = Serializer.Serialize(obj);

            if (Processors != null && Processors.Any())
            {
                foreach (var processor in Processors)
                { output = processor.Process(output); }
            }

            SendToEndpoint.Execute(output, onSuccess, onError);
        }
    }
}