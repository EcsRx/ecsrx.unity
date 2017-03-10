using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Json;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Types;

namespace EcsRx.Persistence.Transformers
{
    public class JsonTransformer : ITransformer<JSONNode>
    {
        public readonly string EntityIdKey = "entityId";
        public readonly string DataKey = "data";

        public IComponentDescriptorRegistry ComponentDescriptorRegistry { get; private set; }
        public IComponentTypeRegistry ComponentTypeRegistry { get; private set; }

        public JsonTransformer(IComponentTypeRegistry componentTypeRegistry, IComponentDescriptorRegistry componentDescriptorRegistry)
        {
            ComponentDescriptorRegistry = componentDescriptorRegistry;
            ComponentTypeRegistry = componentTypeRegistry;
        }

        public JSONNode Transform(ApplicationData applicationData)
        {
            var rootNode = new JSONClass();

            foreach (var entityData in applicationData.EntityData)
            {
                var entityNode = TransformEntity(entityData);
                rootNode.Add(entityNode);
            }

            return rootNode;
        }

        public JSONNode TransformEntity(EntityData entityData)
        {
            var entityNode = new JSONClass();

            entityNode.Add(EntityIdKey, new JSONData(entityData.EntityId.ToString()));
            var entityDataNode = new JSONClass();
            entityNode.Add(DataKey, entityDataNode);

            foreach (var dataValue in entityData.Data)
            {
                var processedValue = ProcessValue(dataValue.Value);
                entityDataNode.Add(dataValue.Key, processedValue);
            }

            return entityNode;
        }

        public JSONNode ProcessValue(object value)
        {
            var enumerableValue = value as IEnumerable;
            if (enumerableValue != null && !(value is string))
            {
                var subJson = new JSONArray();
                foreach (var subValue in enumerableValue)
                {
                    var subValueJson = ProcessValue(subValue);
                    subJson.Add(subValueJson);
                }
                return subJson;
            }

            return new JSONData(value.ToString());
        }

        public object ProcessValue(JSONNode jsonData)
        {/*
            var isArray = jsonData.AsArray.Count > 0;

            if (isArray)
            {
                var jsonArray = dataJson.Value.AsArray;
                var arrayData = jsonArray.Childs.Select(x => x.Value).ToArray();
                var newEntry = new KeyValuePair<string, object>(keySections[1], arrayData);
            }
            else
            {
                propertyType.DataType
                }*/
            return null;
        }

        public EntityData TransformEntity(JSONNode entityJson)
        {
            var entityData = new EntityData();
            entityData.EntityId = new Guid(entityJson[EntityIdKey].Value);

            foreach (KeyValuePair<string, JSONNode> dataJson in entityJson[DataKey].AsObject)
            {
                var keySections = dataJson.Key.Split('.');
                var componentType = FindComponentType(keySections[0]);
                var propertyType = FindPropertyDescriptor(componentType, keySections[1]);
                var isArray = dataJson.Value.AsArray.Count > 0;

                Console.WriteLine("Property: {0} - {1}:{2}", propertyType.DataType, dataJson.Key, dataJson.Value);
                Console.WriteLine("IsArray: {0}", isArray);

                if (isArray)
                {
                    var jsonArray = dataJson.Value.AsArray;
                    var arrayData = jsonArray.Childs.Select(x => x.Value).ToArray();
                    var newEntry = new KeyValuePair<string, object>(keySections[1], arrayData);
                }
                else
                {
                    //propertyType.DataType
                }


                //entityData.Data.Add(newEntry);
//                var propertySection = dataJson.Key.Replace(componentName + ".", "");
//                entityData.Data.Add(dataJson.Key, dataJson.Value.Value);
            }

            return entityData;
        }

        public Type FindComponentType(string componentName)
        {
            var componentType = ComponentTypeRegistry.GetComponentFromName(componentName);
            
            if(componentType == null)
            { throw new Exception("Cannot locate component named [" + componentName + "]"); }

            return componentType;
        }

        public PropertyDataDescriptor FindPropertyDescriptor(Type componentType, string propertyName)
        {
            var sanitisedPropertyName = propertyName;
            if (propertyName.Contains("."))
            {
                var splitProperties = propertyName.Split('.');
                sanitisedPropertyName = splitProperties[0];
            }

            var descriptor = ComponentDescriptorRegistry.GetDescriptorByType(componentType);
            if(descriptor == null) { throw new Exception("Could not find data descriptor for component [" + componentType.Name + "]"); }

            var hasProperty = descriptor.DataProperties.ContainsKey(sanitisedPropertyName);
            if(!hasProperty) {  throw new Exception("Could not find data property [" + sanitisedPropertyName + "] on component [" + componentType.Name + "]");}

            return descriptor.DataProperties[sanitisedPropertyName];
        }
    }
}