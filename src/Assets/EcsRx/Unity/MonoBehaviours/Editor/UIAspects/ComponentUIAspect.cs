using EcsRx.Components;
using EcsRx.Unity.Helpers.EditorInputs;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers.UIAspects
{
    public class ComponentUIAspect
    {
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