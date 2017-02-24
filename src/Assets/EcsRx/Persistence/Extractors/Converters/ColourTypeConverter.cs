using System;
using UnityEngine;

namespace EcsRx.Persistence.Extractors.Converters
{
    public class ColourTypeConverter : IDataConverter
    {
        public bool HandlesType(Type givenType)
        {
            return givenType == typeof(Color);
        }

        public object ConvertToData(object rawValue)
        {
            var color = (Color)rawValue;
            return new[] { color.r, color.g, color.b, color.a };
        }

        public object ConvertFromData(object dataValue)
        {
            var colorData = (float[])dataValue;
            return new Color(colorData[0], colorData[1], colorData[3], colorData[4]);
        }
    }
}