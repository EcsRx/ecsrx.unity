using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Persistity.Extensions;
using Persistity.Mappings;
using Persistity.Registries;
using UnityEngine;

namespace Persistity.Serialization.Json
{
   public class JsonSerializer : GenericSerializer<JToken, JToken>, IJsonSerializer
    {
        public const string TypeField = "Type";
        public const string DataField = "Data";
        public const string KeyField = "Key";
        public const string ValueField = "Value";

        public JsonSerializer(IMappingRegistry mappingRegistry, JsonConfiguration configuration = null) : base(mappingRegistry)
        {
            Configuration = configuration ?? JsonConfiguration.Default;
        }

        private readonly Type[] CatchmentTypes =
        {
            typeof(string), typeof(bool), typeof(byte), typeof(short), typeof(int),
            typeof(long), typeof(Guid), typeof(float), typeof(double), typeof(decimal)
        };

        protected override void HandleNullData(JToken state)
        { state.Replace(JValue.CreateNull()); }

        protected override void HandleNullObject(JToken state)
        { HandleNullData(state); }

        protected override void AddCountToState(JToken state, int count)
        { }

        protected override JToken GetDynamicTypeState(JToken state, Type type)
        {
            state[TypeField] = type.GetPersistableName();
            return state[DataField] = new JObject();
        }

        protected override void SerializeDefaultPrimitive(object value, Type type, JToken element)
        {
            if (type == typeof(Vector2))
            {
                var typedObject = (Vector2)value;
                element["x"] = typedObject.x;
                element["y"] = typedObject.y;
                return;
            }
            if (type == typeof(Vector3))
            {
                var typedObject = (Vector3)value;
                element["x"] = typedObject.x;
                element["y"] = typedObject.y;
                element["z"] = typedObject.z;
                return;
            }
            if (type == typeof(Vector4))
            {
                var typedObject = (Vector4)value;
                element["x"] = typedObject.x;
                element["y"] = typedObject.y;
                element["z"] = typedObject.z;
                element["w"] = typedObject.w;
                return;
            }
            if (type == typeof(Quaternion))
            {
                var typedObject = (Quaternion)value;
                element["x"] = typedObject.x;
                element["y"] = typedObject.y;
                element["z"] = typedObject.z;
                element["w"] = typedObject.w;
                return;
            }
            if (type == typeof(DateTime))
            {
                var typedValue = (DateTime)value;
                var stringValue = typedValue.ToBinary().ToString();
                element.Replace(new JValue(stringValue));
                return;
            }

            if (type.IsTypeOf(CatchmentTypes) || type.IsEnum)
            {
                element.Replace(new JValue(value));
                return;
            }
        }

        public override DataObject Serialize(object data)
        {
            var node = new JObject();
            var dataType = data.GetType();
            var typeMapping = MappingRegistry.GetMappingFor(dataType);
            Serialize(typeMapping.InternalMappings, data, node);

            var typeElement = new JProperty("Type", dataType.GetPersistableName());
            node.Add(typeElement);
            
            var xmlString = node.ToString();
            return new DataObject(xmlString);
        }

        protected override void Serialize<T>(IEnumerable<Mapping> mappings, T data, JToken state)
        {
            foreach (var mapping in mappings)
            {
                var newElement = new JObject();
                state[mapping.LocalName] = newElement;

                DelegateMappingType(mapping, data, newElement);
            }
        }

        protected override void SerializeCollection<T>(CollectionMapping collectionMapping, T data, JToken state)
        {
            var objectValue = AttemptGetValue(collectionMapping, data, state);
            if (objectValue == null) { return; }
            var collectionValue = (objectValue as IList);

            var jsonArray = new JArray();
            state.Replace(jsonArray);
            for (var i = 0; i < collectionValue.Count; i++)
            {
                var element = collectionValue[i];
                var jsonObject = new JObject();
                jsonArray.Add(jsonObject);
                SerializeCollectionElement(collectionMapping, element, jsonObject);
            }
        }
        
        protected override void SerializeDictionary<T>(DictionaryMapping dictionaryMapping, T data, JToken state)
        {
            var objectValue = AttemptGetValue(dictionaryMapping, data, state);
            if (objectValue == null) { return; }
            var dictionaryValue = (objectValue as IDictionary);

            var jsonArray = new JArray();
            state.Replace(jsonArray);
            foreach (var key in dictionaryValue.Keys)
            {
                var jsonObject = new JObject();
                jsonArray.Add(jsonObject);
                SerializeDictionaryKeyValuePair(dictionaryMapping, dictionaryValue, key, jsonObject);
            }
        }
        
        protected override void SerializeDictionaryKeyValuePair(DictionaryMapping dictionaryMapping, IDictionary dictionary, object key, JToken state)
        {
            var keyElement = new JObject();
            var valueElement = new JObject();
            state[KeyField] = keyElement;
            state[ValueField] = valueElement;

            SerializeDictionaryKey(dictionaryMapping, key, keyElement);
            SerializeDictionaryValue(dictionaryMapping, dictionary[key], valueElement);
        }
    }
}