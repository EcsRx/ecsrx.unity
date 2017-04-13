using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Persistity.Mappings;
using Persistity.Mappings.Types;
using Persistity.Registries;

namespace Persistity.Serialization.Json
{
    public class JsonDeserializer : GenericDeserializer<JToken, JToken>, IJsonDeserializer
    {
        protected JsonPrimitiveDeserializer JsonPrimitiveDeserializer { get; private set; }

        public JsonDeserializer(IMappingRegistry mappingRegistry, ITypeCreator typeCreator, JsonConfiguration configuration = null)
            : base(mappingRegistry, typeCreator)
        {
            Configuration = configuration ?? JsonConfiguration.Default;
            JsonPrimitiveDeserializer = new JsonPrimitiveDeserializer();
        }

        protected override bool IsDataNull(JToken state)
        {
            if(state == null) { return true; }
            if (state.Type == JTokenType.Null || state.Type == JTokenType.None)
            { return true; }

            return false;
        }

        protected override bool IsObjectNull(JToken state)
        { return IsDataNull(state); }

        protected override string GetDynamicTypeNameFromState(JToken state)
        { return state[JsonSerializer.TypeField].ToString(); }

        protected override JToken GetDynamicTypeDataFromState(JToken state)
        { return state[JsonSerializer.DataField]; }

        protected override object DeserializeDefaultPrimitive(Type type, JToken state)
        { return JsonPrimitiveDeserializer.DeserializeDefaultPrimitive(type, state); }
        
        public override object Deserialize(DataObject data)
        {
            var jsonData = JObject.Parse(data.AsString);
            var typeName = jsonData[JsonSerializer.TypeField].ToString();
            var type = TypeCreator.LoadType(typeName);
            var typeMapping = MappingRegistry.GetMappingFor(type);
            var instance = Activator.CreateInstance(type);
            
            Deserialize(typeMapping.InternalMappings, instance, jsonData);
            return instance;
        }

        public override void DeserializeInto(DataObject data, object existingInstance)
        {
            var jsonData = JObject.Parse(data.AsString);
            var typeName = jsonData[JsonSerializer.TypeField].ToString();
            var type = TypeCreator.LoadType(typeName);
            var typeMapping = MappingRegistry.GetMappingFor(type);
            
            Deserialize(typeMapping.InternalMappings, existingInstance, jsonData);
        }

        protected override int GetCountFromState(JToken state)
        { return state.Children().Count(); }

        protected override void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, JToken state)
        {
            foreach (var mapping in mappings)
            {
                var childNode = state.HasValues ? state[mapping.LocalName] : null;
                DelegateMappingType(mapping, instance, childNode);
            }
        }

        protected override void DeserializeCollection<T>(CollectionMapping mapping, T instance, JToken state)
        {
            if (IsObjectNull(state))
            {
                mapping.SetValue(instance, null);
                return;
            }

            var count = GetCountFromState(state);
            var collectionInstance = CreateCollectionFromMapping(mapping, count);
            mapping.SetValue(instance, collectionInstance);

            for (var i = 0; i < count; i++)
            {
                var collectionElement = state[i];
                var elementInstance = DeserializeCollectionElement(mapping, collectionElement);

                if (collectionInstance.IsFixedSize)
                { collectionInstance[i] = elementInstance; }
                else
                { collectionInstance.Insert(i, elementInstance); }
            }
        }

        protected override void DeserializeDictionary<T>(DictionaryMapping mapping, T instance, JToken state)
        {
            if (IsObjectNull(state))
            {
                mapping.SetValue(instance, null);
                return;
            }

            var count = GetCountFromState(state);
            var dictionary = TypeCreator.CreateDictionary(mapping.KeyType, mapping.ValueType);
            mapping.SetValue(instance, dictionary);

            for (var i = 0; i < count; i++)
            {
                var keyValuePairElement = state[i];
                DeserializeDictionaryKeyValuePair(mapping, dictionary, keyValuePairElement);
            }
        }

        protected override void DeserializeDictionaryKeyValuePair(DictionaryMapping mapping, IDictionary dictionary, JToken state)
        {
            var keyElement = state["Key"];
            var keyInstance = DeserializeDictionaryKey(mapping, keyElement);
            var valueElement = state["Value"];
            var valueInstance = DeserializeDictionaryValue(mapping, valueElement);
            dictionary.Add(keyInstance, valueInstance);
        }
    }
}