using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EcsRx.Components;
using EcsRx.Persistence.Attributes;
using EcsRx.Persistence.Extensions;

namespace EcsRx.Persistence.Types
{
    public class ComponentDescriptorRegistry : IComponentDescriptorRegistry
    {
        public IDictionary<Type, ComponentDataDescriptor> AllComponentDescriptors { get; private set; }
        public ComponentTypeRegistry ComponentTypeRegistry { get; set; }

        public ComponentDescriptorRegistry(ComponentTypeRegistry componentTypeRegistry)
        {
            ComponentTypeRegistry = componentTypeRegistry;
            RefreshAllComponentDescriptors();
        }

        public void RefreshAllComponentDescriptors()
        {
            AllComponentDescriptors = new Dictionary<Type, ComponentDataDescriptor>();

            var allPersistableComponents = ComponentTypeRegistry.AllComponentTypes.Where(x => ReflectionExtensions.HasAttribute<PersistAttribute>((MemberInfo) x));
            foreach (var persistableComponent in allPersistableComponents)
            {
                var descriptor = GetDescriptorForComponent(persistableComponent);
                AllComponentDescriptors.Add(persistableComponent, descriptor);
            }
        }

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
                descriptor.DataProperties.Add(property.Name, propertyDescriptor);
            }

            return descriptor;
        }

        public ComponentDataDescriptor GetDescriptorFromName(string name)
        {
            var type = ComponentTypeRegistry.GetComponentFromName(name);
            if (type == null) { return null; }

            return AllComponentDescriptors.SingleOrDefault(x => x.Key == type).Value;
        }
    }
}