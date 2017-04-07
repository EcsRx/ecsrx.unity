using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Persistity.Mappings;
using Persistity.Mappings.Types;
using Persistity.Registries;

namespace Persistity.Serialization.Xml
{
    public class XmlDeserializer : GenericDeserializer<XElement, XElement>, IXmlDeserializer
    {
        protected XmlPrimitiveDeserializer XmlPrimitiveDeserializer { get; private set; }

        public XmlDeserializer(IMappingRegistry mappingRegistry, ITypeCreator typeCreator, XmlConfiguration configuration = null) : base(mappingRegistry, typeCreator)
        {
            Configuration = configuration ?? XmlConfiguration.Default;
            XmlPrimitiveDeserializer = new XmlPrimitiveDeserializer();
        }

        protected override bool IsDataNull(XElement state)
        { return state.Attribute("IsNull") != null; }

        protected override bool IsObjectNull(XElement state)
        { return IsDataNull(state); }

        protected override int GetCountFromState(XElement state)
        { return int.Parse(state.Attribute("Count").Value); }

        protected override object DeserializeDefaultPrimitive(Type type, XElement element)
        { return XmlPrimitiveDeserializer.DeserializeDefaultPrimitive(type, element); }

        public override T Deserialize<T>(DataObject data)
        { return (T) Deserialize(data); }

        public override object Deserialize(DataObject data)
        {
            var xDoc = XDocument.Parse(data.AsString);
            var containerElement = xDoc.Element("Container");
            var typeName = containerElement.Element("Type").Value;
            var type = TypeCreator.LoadType(typeName);
            var typeMapping = MappingRegistry.GetMappingFor(type);

            var instance = Activator.CreateInstance(typeMapping.Type);
            Deserialize(typeMapping.InternalMappings, instance, containerElement);
            return instance;
        }

        protected override void DeserializeCollection<T>(CollectionMapping mapping, T instance, XElement state)
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
                var collectionElement = state.Elements("CollectionElement").ElementAt(i);
                var elementInstance = DeserializeCollectionElement(mapping, collectionElement);

                if (collectionInstance.IsFixedSize)
                { collectionInstance[i] = elementInstance; }
                else
                { collectionInstance.Insert(i, elementInstance); }
            }
        }

        protected override void DeserializeDictionary<T>(DictionaryMapping mapping, T instance, XElement state)
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
                var keyValuePairElement = state.Elements("KeyValuePair").ElementAt(i);
                DeserializeDictionaryKeyValuePair(mapping, dictionary, keyValuePairElement);
            }
        }

        protected override void DeserializeDictionaryKeyValuePair(DictionaryMapping mapping, IDictionary dictionary, XElement state)
        {
            var keyElement = state.Element("Key");
            var keyInstance = DeserializeDictionaryKey(mapping, keyElement);
            var valueElement = state.Element("Value");
            var valueInstance = DeserializeDictionaryValue(mapping, valueElement);
            dictionary.Add(keyInstance, valueInstance);
        }

        protected override void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, XElement state)
        {
            foreach (var mapping in mappings)
            {
                var childElement = state.Element(mapping.LocalName);
                DelegateMappingType(mapping, instance, childElement);
            }
        }

        protected override string GetDynamicTypeNameFromState(XElement state)
        { return state.Attribute(XmlSerializer.TypeAttributeName).Value; }

        protected override XElement GetDynamicTypeDataFromState(XElement state)
        { return state; }
    }
}