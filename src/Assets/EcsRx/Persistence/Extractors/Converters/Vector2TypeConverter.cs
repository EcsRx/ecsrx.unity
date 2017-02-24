using System;
using UnityEngine;

namespace EcsRx.Persistence.Extractors.Converters
{
    public class Vector2TypeConverter : IDataConverter
    {
        public bool HandlesType(Type givenType)
        {
            return givenType == typeof(Vector2);
        }

        public object ConvertToData(object rawValue)
        {
            var vector = (Vector2)rawValue;
            return new[] { vector.x, vector.y };
        }

        public object ConvertFromData(object dataValue)
        {
            var vectorData = (float[])dataValue;
            return new Vector3(vectorData[0], vectorData[1]);
        }
    }
}