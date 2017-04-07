using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Persistity.Extensions;
using Persistity.Mappings;
using Persistity.Registries;
using UnityEngine;

namespace Persistity.Serialization.Xml
{
    public class XmlSerializer : GenericSerializer<XElement, XElement>, IXmlSerializer
    {
        public const string TypeAttributeName = "Type";
        public const string NullElementName = "IsNull";
        public const string CountElementName = "Count";

        protected XmlPrimitiveSerializer XmlPrimitiveSerializer { get; private set; }

        public XmlSerializer(IMappingRegistry mappingRegistry, XmlConfiguration configuration = null) : base(mappingRegistry)
        {
            Configuration = configuration ?? XmlConfiguration.Default;
            XmlPrimitiveSerializer = new XmlPrimitiveSerializer();
        }

        protected override void HandleNullData(XElement state)
        { state.Add(new XAttribute(NullElementName, true)); }

        protected override void HandleNullObject(XElement state)
        { HandleNullData(state); }

        protected override void AddCountToState(XElement state, int count)
        { state.Add(new XAttribute(CountElementName, count)); }
        
        protected override XElement GetDynamicTypeState(XElement state, Type type)
        {
            var typeAttribute = new XAttribute(TypeAttributeName, type.GetPersistableName());
            state.Add(typeAttribute);
            return state;
        }

        protected override void SerializeDefaultPrimitive(object value, Type type, XElement element)
        { XmlPrimitiveSerializer.SerializeDefaultPrimitive(value, type, element); }

        public override DataObject Serialize(object data)
        {
            var element = new XElement("Container");
            var dataType = data.GetType();
            var typeMapping = MappingRegistry.GetMappingFor(dataType);
            Serialize(typeMapping.InternalMappings, data, element);

            var typeElement = new XElement("Type", dataType.GetPersistableName());
            element.Add(typeElement);
            
            var xmlString = element.ToString();
            return new DataObject(xmlString);
        }

        protected override void Serialize<T>(IEnumerable<Mapping> mappings, T data, XElement state)
        {
            foreach (var mapping in mappings)
            {
                var newElement = new XElement(mapping.LocalName);
                state.Add(newElement);

                DelegateMappingType(mapping, data, newElement);
            }
        }

        protected override void SerializeCollectionElement<T>(CollectionMapping collectionMapping, T element, XElement state)
        {
            var newElement = new XElement("CollectionElement");
            state.Add(newElement);

            if (element == null)
            {
                HandleNullObject(newElement);
                return;
            }

            if (collectionMapping.IsElementDynamicType)
            {
                SerializeDynamicTypeData(element, newElement);
                return;
            }

            if (collectionMapping.InternalMappings.Count > 0)
            { Serialize(collectionMapping.InternalMappings, element, newElement); }
            else
            { SerializePrimitive(element, collectionMapping.CollectionType, newElement); }
        }

        protected override void SerializeDictionaryKeyValuePair(DictionaryMapping dictionaryMapping, IDictionary dictionary, object key, XElement state)
        {
            var keyElement = new XElement("Key");
            SerializeDictionaryKey(dictionaryMapping, key, keyElement);

            var valueElement = new XElement("Value");
            SerializeDictionaryValue(dictionaryMapping, dictionary[key], valueElement);

            var keyValuePairElement = new XElement("KeyValuePair");
            keyValuePairElement.Add(keyElement, valueElement);
            state.Add(keyValuePairElement);
        }
    }
}