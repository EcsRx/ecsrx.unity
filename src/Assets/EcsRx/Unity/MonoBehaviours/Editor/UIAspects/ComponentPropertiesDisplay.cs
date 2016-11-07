using System;
using Assets.EcsRx.Unity.Extensions;
using EcsRx.Json;
using EcsRx.Unity.Helpers.EditorInputs;
using EcsRx.Unity.MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers.UIAspects
{
    public class ComponentPropertiesDisplay
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

        public static void ShowComponentProperties(RegisterAsEntity registerAsEntity, int index)
        {
            var value = registerAsEntity.Components[index];
            var type = AttemptGetType(value);

            var component = Activator.CreateInstance(type);
            var node = JSON.Parse(registerAsEntity.Properties[index]);
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

                registerAsEntity.Components[index] = component.GetType().ToString();
                var json = component.SerializeComponent();
                registerAsEntity.Properties[index] = json.ToString();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}