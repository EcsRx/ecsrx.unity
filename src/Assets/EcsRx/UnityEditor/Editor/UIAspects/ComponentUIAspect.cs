using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.UnityEditor.Editor.EditorInputs;
using EcsRx.UnityEditor.Editor.EditorInputs.Basic;
using EcsRx.UnityEditor.Editor.Helpers;
using EcsRx.UnityEditor.Extensions;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.UIAspects
{
    public static class ComponentUIAspect
    {
        private static IDictionary<Type, IEditorInput[]> _cachedEditorInputs = new Dictionary<Type, IEditorInput[]>();

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
                    ReadOnlyStringEditorInput.TypeUI(property.Name, $"Unsupported [{propertyType.Name}]");
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
