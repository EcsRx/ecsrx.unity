using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Persistence.Extractors.Converters;

namespace EcsRx.Persistence.Extractors
{
    public class DataConverterRegistry
    {
        public IEnumerable<IDataConverter> Handlers { get; private set; }

        public DataConverterRegistry(IEnumerable<IDataConverter> handlers)
        {
            Handlers = handlers;
        }

        public IDataConverter GetHandlerFor(Type type)
        {
            return Handlers.SingleOrDefault(x => x.HandlesType(type));
        }
    }
}