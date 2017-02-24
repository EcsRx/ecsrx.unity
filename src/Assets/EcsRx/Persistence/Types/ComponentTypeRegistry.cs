using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EcsRx.Components;
using EcsRx.Persistence.Attributes;
using EcsRx.Persistence.Extensions;

namespace EcsRx.Persistence.Types
{
    public class ComponentTypeRegistry
    {
        public Type IComponentType { get; private set; }
        public IList<Type> AllComponentTypes { get; private set; }
        public IDictionary<Type, ComponentDataDescriptor> AllComponentDescriptors { get; private set; }

        public ComponentTypeRegistry()
        {
            IComponentType = typeof(IComponent);
            RefreshComponentList();
            RefreshAllComponentDescriptors();
        }

        public void RefreshComponentList()
        {
            var assemblyList = AppDomain.CurrentDomain.GetAssemblies().ToList();

            AllComponentTypes = assemblyList
                .SelectMany(x => x.GetTypes())
                .Where(x => IComponentType.IsAssignableFrom(x))
                .Where(x => x != IComponentType)
                .ToList();
        }

        public void RefreshAllComponentDescriptors()
        {
            AllComponentDescriptors = new Dictionary<Type, ComponentDataDescriptor>();

            var allPersistableComponents = AllComponentTypes.Where(x => x.HasAttribute<PersistAttribute>());
            foreach (var persistableComponent in allPersistableComponents)
            {
                var descriptor = GetDescriptorForComponent(persistableComponent);
                AllComponentDescriptors.Add(persistableComponent, descriptor);
            }
        }

        public ComponentDataDescriptor GetDescriptorForComponent(Type component)
        {
            var descriptor = new ComponentDataDescriptor {ComponentType = component};
            var dataFields = component.GetFields().Where(x => x.HasAttribute<PersistDataAttribute>());

            foreach (var field in dataFields)
            {
                var propertyDescriptor = new PropertyDataDescriptor {DataType = field.FieldType};
                propertyDescriptor.GetValue = field.GetValue;
                propertyDescriptor.SetValue = field.SetValue;
                descriptor.DataProperties.Add(field.Name, propertyDescriptor);
            }

            var dataProperties = component.GetProperties().Where(x => x.HasAttribute<PersistDataAttribute>());
            foreach (var property in dataProperties)
            {
                var localProperty = property;
                var propertyDescriptor = new PropertyDataDescriptor {DataType = property.PropertyType};
                propertyDescriptor.GetValue = (IComponent caller) => localProperty.GetValue(caller, null);
                propertyDescriptor.SetValue = (IComponent caller, object value) => localProperty.SetValue(caller, value, null);
                descriptor.DataProperties.Add(property.Name, propertyDescriptor);
            }

            return descriptor;
        }

        public Type GetComponentFromName(string name)
        {
            var sanitisedName = name.ToLower().Replace("component", "");

            // TODO: could potentially have components of same name in other namespaces
            return AllComponentTypes.SingleOrDefault(x => x.Name.ToLower().Replace("component", "") == sanitisedName);
        }

        public ComponentDataDescriptor GetDescriptorFromName(string name)
        {
            var type = GetComponentFromName(name);
            if (type == null) { return null; }

            return AllComponentDescriptors.SingleOrDefault(x => x.Key == type).Value;
        }
    }
}