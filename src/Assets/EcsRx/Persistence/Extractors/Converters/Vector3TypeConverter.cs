using System;
using UnityEngine;

namespace EcsRx.Persistence.Extractors.Converters
{
    public class Vector3TypeConverter : IDataConverter
    {
        public bool HandlesType(Type givenType)
        {
            return givenType == typeof(Vector3);
        }

        public object ConvertToData(object rawValue)
        {
            var vector = (Vector3)rawValue;
            return new[] { vector.x, vector.y, vector.z };
        }

        public object ConvertFromData(object dataValue)
        {
            var vectorData = (float[])dataValue;
            return new Vector3(vectorData[0], vectorData[1], vectorData[3]);
        }
    }
}