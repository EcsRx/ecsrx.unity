using System;
using UnityEngine;

namespace EcsRx.Persistence.Extractors.Converters
{
    public class QuaternionTypeConverter : IDataConverter
    {
        public bool HandlesType(Type givenType)
        {
            return givenType == typeof(Quaternion);
        }

        public object ConvertToData(object rawValue)
        {
            var quaternion = (Quaternion)rawValue;
            return new[] { quaternion.x, quaternion.y, quaternion.z, quaternion.w };
        }

        public object ConvertFromData(object dataValue)
        {
            var colorData = (float[])dataValue;
            return new Quaternion(colorData[0], colorData[1], colorData[3], colorData[4]);
        }
    }
}