using System;
using UniRx;

namespace EcsRx.Persistence.Primitives.Helpers
{
    public class ReactiveValueHandler
    {
        public void SetValue(object reactiveProperty, Type genericType, object newValue)
        {
            var typedGeneric = typeof(ReactiveProperty<>).MakeGenericType(genericType);
            var propertyInfo = typedGeneric.GetProperty("Value");
            propertyInfo.SetValue(reactiveProperty, newValue, null);
        }

        public object GetValue(object reactiveProperty, Type genericType)
        {
            var typedGeneric = typeof(ReactiveProperty<>).MakeGenericType(genericType);
            var propertyInfo = typedGeneric.GetProperty("Value");
            return propertyInfo.GetValue(reactiveProperty, null);
        }
    }
}