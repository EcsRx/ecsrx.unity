using System;
using EcsRx.Components;
using EcsRx.Unity.EditorInputs;
using EcsRx.Unity.Extensions;
using EcsRx.Unity.Helpers;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.UIAspects
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

        public static IComponent RehydrateEditorComponent(string typeName, string editorStateData)
        {
            var component = InstantiateDefaultComponent<IComponent>(typeName);
            if (string.IsNullOrEmpty(editorStateData)) { return component; }

            var componentJson = JSON.Parse(editorStateData);
            component.DeserializeComponent(componentJson);
            return component;
        }

        public static T InstantiateDefaultComponent<T>(string componentTypeName)
            where T : IComponent
        {
            var type = AttemptGetType(componentTypeName);
            return (T)Activator.CreateInstance(type);
        }
        
        public static void ShowComponentProperties<T>(T component)
            where T : IComponent
        {
            var componentProperties = component.GetType().GetProperties();
            foreach (var property in componentProperties)
            {
                EditorGUILayout.BeginHorizontal();
                var propertyType = property.PropertyType;
                var propertyValue = property.GetValue(component, null);

                var handler = DefaultEditorInputRegistry.GetHandlerFor(propertyType);
                if (handler == null)
                {
                    Debug.LogWarning("This type is not supported: " + propertyType.Name + " - In component: " + component.GetType().Name);
                    EditorGUILayout.EndHorizontal();
                    continue;
                }

                var updatedValue = handler.CreateUI(property.Name, propertyValue);

                if (updatedValue != null)
                { property.SetValue(component, updatedValue, null); }

                EditorGUILayout.EndHorizontal();
            }
        } 
         

    }
}
