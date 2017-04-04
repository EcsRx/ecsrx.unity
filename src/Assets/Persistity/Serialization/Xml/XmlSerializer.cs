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

        public XmlSerializer(IMappingRegistry mappingRegistry, XmlConfiguration configuration = null) : base(mappingRegistry)
        {
            Configuration = configuration ?? XmlConfiguration.Default;
        }

        private readonly Type[] CatchmentTypes =
        {
            typeof(string), typeof(bool), typeof(byte), typeof(short), typeof(int),
            typeof(long), typeof(Guid), typeof(float), typeof(double), typeof(decimal)
        };

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
        {
            if (type == typeof(Vector2))
            {
                var typedObject = (Vector2)value;
                element.Add(new XElement("x", typedObject.x));
                element.Add(new XElement("y", typedObject.y));
                return;
            }
            if (type == typeof(Vector3))
            {
                var typedObject = (Vector3)value;
                element.Add(new XElement("x", typedObject.x));
                element.Add(new XElement("y", typedObject.y));
                element.Add(new XElement("z", typedObject.z));
                return;
            }
            if (type == typeof(Vector4))
            {
                var typedObject = (Vector4)value;
                element.Add(new XElement("x", typedObject.x));
                element.Add(new XElement("y", typedObject.y));
                element.Add(new XElement("z", typedObject.z));
                element.Add(new XElement("w", typedObject.w));
                return;
            }
            if (type == typeof(Quaternion))
            {
                var typedObject = (Quaternion)value;
                element.Add(new XElement("x", typedObject.x));
                element.Add(new XElement("y", typedObject.y));
                element.Add(new XElement("z", typedObject.z));
                element.Add(new XElement("w", typedObject.w));
                return;
            }
            if (type == typeof(DateTime))
            {
                var typedValue = (DateTime)value;
                var stringValue = typedValue.ToBinary().ToString();
                element.Value = stringValue;
                return;
            }

            if (type.IsTypeOf(CatchmentTypes) || type.IsEnum)
            {
                element.Value = value.ToString();
                return;
            }
        }

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