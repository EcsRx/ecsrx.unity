using System;
using System.Collections.Generic;

namespace EcsRx.Persistence.Extractors.Converters
{
    public interface IDataConverter
    {
        bool HandlesType(Type givenType);
        object ConvertToData(object rawValue);
        object ConvertFromData(object dataValue);
    }
}