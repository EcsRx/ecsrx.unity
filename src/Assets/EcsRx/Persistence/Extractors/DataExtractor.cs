using System;
using System.Collections;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Types;

namespace EcsRx.Persistence.Extractors
{
    public class DataExtractor : IDataExtractor
    {
        public ComponentTypeRegistry ComponentTypeRegistry { get; private set; }
        public DataConverterRegistry DataConvertorRegistry { get; private set; }

        public DataExtractor(ComponentTypeRegistry componentTypeRegistry, DataConverterRegistry dataConverterRegistry)
        {
            ComponentTypeRegistry = componentTypeRegistry;
            DataConvertorRegistry = dataConverterRegistry;
        }

        public EntityData Extract(IEntity entity)
        {
            var entityData = new EntityData { EntityId = entity.Id };
            foreach (var component in entity.Components)
            {
                var componentType = component.GetType();
                if (!ComponentTypeRegistry.AllComponentDescriptors.ContainsKey(componentType))
                { continue; }

                var descriptor = ComponentTypeRegistry.AllComponentDescriptors[componentType];
                foreach (var property in descriptor.DataProperties)
                {
                    var handler = DataConvertorRegistry.GetHandlerFor(property.Value.DataType);
                    if(handler == null) { throw new Exception("Not handler found for type [" + property.Value.DataType.Name + "]"); }

                    var rawValue = property.Value.GetValue(component);
                    var convertedData = handler.ConvertToData(rawValue);

                    var sanitisedComponentName = componentType.Name.ToLower().Replace("component", "");
                    var sanitisedPropertyName = property.Key.ToLower();
                    var entityDataName = string.Format("{0}.{1}", sanitisedComponentName, sanitisedPropertyName);
                    entityData.Data.Add(entityDataName, convertedData);
                }
            }
            return entityData;
        }
    }
}