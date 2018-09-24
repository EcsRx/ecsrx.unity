using System.Linq;
using EcsRx.Components;
using EcsRx.Persistence.Editor.EditorInputs;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Persistence.Editor.UIAspects
{
    public class ComponentUIAspect
    {
         public static void ShowComponentProperties<T>(T component)
            where T : IComponent
        {
            var componentProperties = component.GetType().GetProperties().ToArray();

            var handledProperties = 0;
            
            GUILayout.Space(5.0f);
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
                GUILayout.Space(5.0f);
                handledProperties++;
            }

            if (handledProperties == 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("No supported properties for this component");
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5.0f);
            }
        }
    }
}