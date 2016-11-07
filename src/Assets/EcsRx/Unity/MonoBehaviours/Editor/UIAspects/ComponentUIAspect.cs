using System;
using System.Collections.Generic;
using Assets.EcsRx.Unity.Extensions;
using EcsRx.Components;
using EcsRx.Json;
using EcsRx.Unity.Helpers.EditorInputs;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers.UIAspects
{
    public class ComponentUIAspect
    {
        public static Type AttemptGetType(string typeName)
        {
            var type = TypeHelper.GetTypeWithAssembly(typeName);
            if(type != null) { return type; }

            if (GUILayout.Button("TYPE NOT FOUND. TRY TO CONVERT TO BEST MATCH?"))
            {
                type = TypeHelper.TryGetConvertedType(typeName);
                if(type != null) { return type; }

                Debug.LogWarning("UNABLE TO CONVERT " + typeName);
                return null;
            }
            return null;
        }

        public static T InstantiateDefaultComponent<T>(string componentTypeName)
            where T : IComponent
        {
            var type = AttemptGetType(componentTypeName);
            return (T)Activator.CreateInstance(type);
        }

        public static void ShowComponentProperties<T>(T component, IList<string> properties, int index)
            where T: IComponent
        {
            var node = JSON.Parse(properties[index]);
            component.DeserializeComponent(node);

            foreach (var property in component.GetType().GetProperties())
            {
                EditorGUILayout.BeginHorizontal();
                var propertyType = property.PropertyType;
                var propertyValue = property.GetValue(component, null);

                var handler = DefaultEditorInputRegistry.GetHandlerFor(propertyValue);
                if (handler == null)
                {
                    Debug.LogWarning("This type is not supported: " + propertyType.Name + " - In component: " + component.GetType().Name);
                    continue;
                }

                var updatedValue = handler.CreateUI(property.Name, propertyValue);

                if (updatedValue != null)
                { property.SetValue(component, updatedValue, null); }

                var json = component.SerializeComponent();
                properties[index] = json.ToString();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}