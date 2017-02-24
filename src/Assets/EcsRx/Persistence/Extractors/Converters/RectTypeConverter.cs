using System;
using UnityEngine;

namespace EcsRx.Persistence.Extractors.Converters
{
    public class RectTypeConverter : IDataConverter
    {
        public bool HandlesType(Type givenType)
        {
            return givenType == typeof(Rect);
        }

        public object ConvertToData(object rawValue)
        {
            var rect = (Rect)rawValue;
            return new[] { rect.x, rect.y, rect.width, rect.height };
        }

        public object ConvertFromData(object dataValue)
        {
            var rectData = (float[])dataValue;
            return new Rect(rectData[0], rectData[1], rectData[3], rectData[4]);
        }
    }
}