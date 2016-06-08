using System.Reflection;
using EcsRx.Json;
using UnityEngine;

namespace Assets.EcsRx.Unity.Extensions
{ 
    public static class ComponentExtensions
    {
        public static JSONClass SerializeComponent(this object component)
        {
            var node = new JSONClass();
            foreach (var property in component.GetType().GetProperties())
            {
                if (property.CanRead && property.CanWrite)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        node.Add(property.Name, new JSONData((int)property.GetValue(component, null)));
                        continue;
                    }
                    if (property.PropertyType == typeof(bool))
                    {
                        node.Add(property.Name, new JSONData((bool)property.GetValue(component, null)));
                        continue;
                    }
                    if (property.PropertyType == typeof(float))
                    {
                        node.Add(property.Name, new JSONData((float)property.GetValue(component, null)));
                        continue;
                    }
                    if (property.PropertyType == typeof(Vector3))
                    {
                        node.Add(property.Name, new JSONData((Vector3)property.GetValue(component, null)));
                        continue;
                    }
                    if (property.PropertyType == typeof(Vector2))
                    {
                        node.Add(property.Name, new JSONData((Vector2)property.GetValue(component, null)));
                        continue;
                    }
                    if (property.PropertyType == typeof(string))
                    {
                        node.Add(property.Name, new JSONData((string)property.GetValue(component, null)));
                        continue;
                    }
                }
            }
            return node;
        }

        public static void DeserializeComponent(this object component, JSONNode node)
        {
            foreach (var property in component.GetType().GetProperties())
            {
                ApplyValue(component, node, property);
            }
        }
    
        private static void ApplyValue(object component, JSONNode node, PropertyInfo property)
        {
            if (property.CanRead && property.CanWrite)
            {
                var propertyData = node[property.Name];
                if (property.PropertyType == typeof(int))
                {
                    property.SetValue(component, propertyData.AsInt, null);
                    return;
                }
                if (propertyData == null) return;
                if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(component, propertyData.AsBool, null);
                    return;
                }
                if (property.PropertyType == typeof(float))
                {
                    property.SetValue(component, propertyData.AsFloat, null);
                    return;
                }
                if (property.PropertyType == typeof(Vector3))
                {
                    property.SetValue(component, propertyData.AsVector3, null);
                    return;
                }
                if (property.PropertyType == typeof(Vector2))
                {
                    property.SetValue(component, propertyData.AsVector2, null);
                    return;
                }
                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(component, propertyData.Value, null);
                    return;
                }
            }
        }
    }
}