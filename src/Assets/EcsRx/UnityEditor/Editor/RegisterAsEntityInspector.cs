using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Unity.Extensions;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Unity.UIAspects;
using EcsRx.Plugins.Views.Components;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity
{
    [Serializable]
    [CustomEditor(typeof(RegisterAsEntity))]
    public partial class RegisterAsEntityInspector : UnityEditor.Editor
    {
        private RegisterAsEntity _registerAsEntity;

        private readonly IEnumerable<Type> allComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => typeof(IComponent).IsAssignableFrom(p) && p.IsClass && !typeof(ViewComponent).IsAssignableFrom(p));

        private bool showComponents;

        private void PoolSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                _registerAsEntity.CollectionId = this.WithNumberField("EntityCollection: ", _registerAsEntity.CollectionId);
            });
        }

        private void ComponentListings()
        {
            EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);
            this.WithHorizontalLayout(() =>
            {
                this.WithLabel("Components (" + _registerAsEntity.Components.Count() + ")");
                if (this.WithIconButton("▸")) { showComponents = false; }
                if (this.WithIconButton("▾")) { showComponents = true; }
            });

            var componentsToRemove = new List<int>();
            var componentCount = _registerAsEntity.Components.Count();
            if (showComponents)
            {
                for (var i = 0; i < componentCount; i++)
                {
                    var currentIndex = i;
                    this.UseVerticalBoxLayout(() =>
                    {
                        var componentType = _registerAsEntity.Components[currentIndex];
                        var namePortions = componentType.Split(',')[0].Split('.');
                        var typeName = namePortions.Last();
                        var typeNamespace = string.Join(".", namePortions.Take(namePortions.Length - 1).ToArray());

                        this.WithVerticalLayout(() =>
                        {
                            this.WithHorizontalLayout(() =>
                            {
                                if (this.WithIconButton("-"))
                                { componentsToRemove.Add(currentIndex); }

                                this.WithLabel(typeName);
                            });

                            EditorGUILayout.LabelField(typeNamespace);
                            EditorGUILayout.Space();
                        });

                        var componentTypeName = _registerAsEntity.Components[currentIndex];

                        // This error only really occurs if the scene has corrupted or an update has changed where editor state is stored on the underlying MB
                        if(_registerAsEntity.ComponentEditorState.Count <= currentIndex)
                        { Debug.LogError("It seems there is missing editor state for [" + componentTypeName + "] this can often be fixed by removing and re-adding the RegisterAsEntity MonoBehavior"); }

                        var editorStateValue = _registerAsEntity.ComponentEditorState[currentIndex];
                        var component = ComponentUIAspect.RehydrateEditorComponent(componentTypeName, editorStateValue);

                        ComponentUIAspect.ShowComponentProperties(component);

                        var serializedData = component.SerializeComponent();
                        _registerAsEntity.ComponentEditorState[currentIndex] = serializedData.ToString();
                    });
                }
            }

            EditorGUILayout.EndVertical();

            for (var i = 0; i < componentsToRemove.Count(); i++)
            {
                _registerAsEntity.Components.RemoveAt(componentsToRemove[i]);
                _registerAsEntity.ComponentEditorState.RemoveAt(componentsToRemove[i]);
            }
        }
        

        private void ComponentSelectionSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                var availableTypes = allComponentTypes
                    .Where(x => !_registerAsEntity.Components.Contains(x.ToString()))
                    .ToArray();

                var types = availableTypes.Select(x => string.Format("{0} [{1}]", x.Name, x.Namespace)).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);
                if (index >= 0)
                {
                    var component = availableTypes.ElementAt(index);
                    var componentName = component.ToString();
                    _registerAsEntity.Components.Add(componentName);
                    var json = component.SerializeComponent();
                    _registerAsEntity.ComponentEditorState.Add(json.ToString());
                }
            });
        }

        private void PersistChanges()
        {
            if (GUI.changed)
            { this.SaveActiveSceneChanges(); }
        }

        public override void OnInspectorGUI()
        {
            _registerAsEntity = (RegisterAsEntity)target;
            
            PoolSection();
            EditorGUILayout.Space();
            ComponentSelectionSection();
            ComponentListings();
            PersistChanges();
            
        }
    }
}
