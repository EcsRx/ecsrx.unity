using System;
using System.Linq;
using EcsRx.Components;
using EcsRx.Persistence.Extensions;
using EcsRx.Persistence.Types;
using Persistity.Attributes;

namespace Assets.Tests.Editor.Helpers
{
    public class ComponentDescriptorHelper
    {
        public ComponentDataDescriptor GetDescriptorForComponent(Type component)
        {
            var descriptor = new ComponentDataDescriptor { ComponentType = component };
            var dataFields = component.GetFields().Where(x => x.HasAttribute<PersistDataAttribute>());

            foreach (var field in dataFields)
            {
                var propertyDescriptor = new PropertyDataDescriptor { DataType = field.FieldType };
                propertyDescriptor.GetValue = field.GetValue;
                propertyDescriptor.SetValue = field.SetValue;
                descriptor.DataProperties.Add(field.Name, propertyDescriptor);
            }

            var dataProperties = component.GetProperties().Where(x => x.HasAttribute<PersistDataAttribute>());
            foreach (var property in dataProperties)
            {
                var localProperty = property;
                var propertyDescriptor = new PropertyDataDescriptor { DataType = property.PropertyType };
                propertyDescriptor.GetValue = (IComponent caller) => localProperty.GetValue(caller, null);
                propertyDescriptor.SetValue = (IComponent caller, object value) => localProperty.SetValue(caller, value, null);
                descriptor.DataProperties.Add(property.Name.ToLower(), propertyDescriptor);
            }

            return descriptor;
        }

        public PropertyDataDescriptor GetPropertyDataDescriptor(Type component, string propertyName)
        {
            var localProperty = component.GetProperties().Single(x => string.Equals(x.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));
            var propertyDescriptor = new PropertyDataDescriptor { DataType = localProperty.PropertyType };
            propertyDescriptor.GetValue = (IComponent caller) => localProperty.GetValue(caller, null);
            propertyDescriptor.SetValue = (IComponent caller, object value) => localProperty.SetValue(caller, value, null);
            return propertyDescriptor;
        }
    }
}
