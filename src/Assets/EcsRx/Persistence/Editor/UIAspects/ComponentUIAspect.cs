using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Persistence.Editor.EditorInputs;
using EcsRx.Persistence.Editor.EditorInputs.Reactive;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Persistence.Editor.UIAspects
{
    public static class ComponentUIAspect
    {
        private static IDictionary<Type, IEditorInput[]> _cachedEditorInputs = new Dictionary<Type, IEditorInput[]>();

        public static void CacheEditorInputs(IComponent component)
        {
            var componentType = component.GetType();
            var componentProperties = componentType.GetProperties().ToArray();
            var handlers = new IEditorInput[componentProperties.Length];

            for (var i = 0; i < componentProperties.Length; i++)
            {
                var property = componentProperties[i];
                var propertyType = property.PropertyType;

                var handler = DefaultEditorInputRegistry.GetHandlerFor(propertyType);
                handlers[i] = handler;
            }
            
            _cachedEditorInputs.Add(componentType, handlers);
            Observable.Timer(TimeSpan.FromMinutes(1)).First().Subscribe(x => _cachedEditorInputs.Remove(componentType));
        }
        
        public static bool ShowComponentProperties<T>(T component) where T : IComponent
        {
            var componentType = component.GetType();
            if(!_cachedEditorInputs.ContainsKey(componentType))
            { CacheEditorInputs(component); }

            var componentProperties = componentType.GetProperties().ToArray();
            var handlers = _cachedEditorInputs[componentType];
            var handledProperties = 0;
           
            var hasChanged = false;
            
            GUILayout.Space(5.0f);
            for (var i = 0; i < componentProperties.Length; i++)
            {
                var property = componentProperties[i];
                var handler = handlers[i];
                
                EditorGUILayout.BeginHorizontal();
                var propertyType = property.PropertyType;
                var propertyValue = property.GetValue(component, null);

                if (handler == null)
                {
                    Debug.LogWarning($"This type is not supported [{propertyType.Name}] - In component {component.GetType().Name}");
                    EditorGUILayout.EndHorizontal();
                    continue;
                }

                var uiStateChange = handler.CreateUI(property.Name, propertyValue);

                if (uiStateChange.HasChanged)
                {
                    hasChanged = true;

                    if(uiStateChange.Value != null)
                    { property.SetValue(component, uiStateChange.Value, null); }
                }

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

            return hasChanged;
        }
    }
}