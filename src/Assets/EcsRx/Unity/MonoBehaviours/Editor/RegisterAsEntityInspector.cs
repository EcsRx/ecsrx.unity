using Assets.EcsRx.Unity.Extensions;
using EcsRx.Unity.Helpers.Extensions;
using EcsRx.Unity.Helpers.UIAspects;
using EcsRx.Unity.MonoBehaviours;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using EcsRx.Components;
using EcsRx.Persistence.Data;
using EcsRx.Unity.Components;
using EcsRx.Unity.MonoBehaviours.Helpers;

namespace EcsRx.Unity.Helpers
{
    [Serializable]
    [CustomEditor(typeof(RegisterAsEntity))]
    public partial class RegisterAsEntityInspector : Editor
    {
        private RegisterAsEntity _registerAsEntity;

        private bool showComponents;

        private void PoolSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                _registerAsEntity.PoolName = this.WithTextField("Pool: ", _registerAsEntity.PoolName);
            });
        }

        private void ComponentListings()
        {
            EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);
            this.WithHorizontalLayout(() =>
            {
                this.WithLabel("Components (" + _registerAsEntity.ComponentData.Count + ")");
                if (this.WithIconButton("▸")) { showComponents = false; }
                if (this.WithIconButton("▾")) { showComponents = true; }
            });

            var componentsToRemove = new List<int>();
            var componentCount = _registerAsEntity.ComponentData.Count;
            if (showComponents)
            {
                for (var i = 0; i < componentCount; i++)
                {
                    var currentIndex = i;
                    var cachedComponent = _registerAsEntity.ComponentData[currentIndex];

                    // This error only really occurs if the scene has corrupted or an update has changed where editor state is stored on the underlying MB
                    if (_registerAsEntity.ComponentData.Count <= currentIndex)
                    {
                        Debug.LogError("It seems there is missing editor state for [" + cachedComponent + "] this can often be fixed by removing and re-adding the RegisterAsEntity MonoBehavior");
                        break;
                    }

                    this.UseVerticalBoxLayout(() =>
                    {
                        var namePortions = cachedComponent.ComponentTypeReference.Split(',')[0].Split('.');
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

                        var component = EntityTransformer.DeserializeComponent(cachedComponent);
                        ComponentUIAspect.ShowComponentProperties(component);

                        var componentState = EntityTransformer.SerializeComponent(component);
                        cachedComponent.ComponentState = componentState;
                    });
                }
            }

            EditorGUILayout.EndVertical();

            for (var i = 0; i < componentsToRemove.Count; i++)
            { _registerAsEntity.ComponentData.RemoveAt(componentsToRemove[i]); }
        }
        

        private void ComponentSelectionSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                var availableTypes = ComponentLookup.AllComponents
                    .Where(x => !typeof(ViewComponent).IsAssignableFrom(x))
                    .Where(x => !_registerAsEntity.ComponentData.Select(y => y.ComponentTypeReference).Contains(x.ToString()))
                    .ToArray();

                var types = availableTypes.Select(x => string.Format("{0} [{1}]", x.Name, x.Namespace)).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);

                if (index < 0) { return; }

                var componentType = availableTypes.ElementAt(index);
                var component = (IComponent)Activator.CreateInstance(componentType);
                var componentState = EntityTransformer.SerializeComponent(component);
                var componentName = component.ToString();
                var componentCache = new ComponentData
                {
                    ComponentTypeReference = componentName,
                    ComponentState = componentState
                };
                _registerAsEntity.ComponentData.Add(componentCache);
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
