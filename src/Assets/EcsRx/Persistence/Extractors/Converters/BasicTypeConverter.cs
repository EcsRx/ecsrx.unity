using System;
using System.Collections.Generic;

namespace EcsRx.Persistence.Extractors.Converters
{
    public class BasicTypeConverter : IDataConverter
    {
        public bool HandlesType(Type givenType)
        {
            return givenType == typeof(int)
                || givenType == typeof(bool)
                || givenType == typeof(float)
                || givenType == typeof(double)
                || givenType == typeof(string);
        }

        public object ConvertToData(object rawValue)
        { return rawValue; }

        public object ConvertFromData(object dataValue)
        { return dataValue; }
    }
}