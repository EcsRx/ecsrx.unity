using EcsRx.Unity.Helpers.Extensions;
using EcsRx.Unity.Helpers.UIAspects;
using EcsRx.Unity.MonoBehaviours;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using EcsRx.Components;
using EcsRx.Persistence.Editor;
using EcsRx.Unity.Components;
using Persistity.Serialization.Binary;

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
            var componentCount = _registerAsEntity.EntityData.Components.Count;

            EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);
            this.WithHorizontalLayout(() =>
            {
                this.WithLabel("Components (" + componentCount + ")");
                if (this.WithIconButton("▸")) { showComponents = false; }
                if (this.WithIconButton("▾")) { showComponents = true; }
            });

            var componentsToRemove = new List<int>();
            if (showComponents)
            {
                for (var i = 0; i < componentCount; i++)
                {
                    var currentIndex = i;
                    var currentComponent = _registerAsEntity.EntityData.Components[currentIndex];

                    // This error only really occurs if the scene has corrupted or an update has changed where editor state is stored on the underlying MB
                    if (componentCount <= currentIndex)
                    {
                        Debug.LogError("It seems there is missing editor state for [" + currentComponent + "] this can often be fixed by removing and re-adding the RegisterAsEntity MonoBehavior");
                        break;
                    }

                    this.UseVerticalBoxLayout(() =>
                    {
                        var namePortions = currentComponent.GetType().FullName.Split(',')[0].Split('.');
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

                        ComponentUIAspect.ShowComponentProperties(currentComponent);
                    });
                }
            }

            EditorGUILayout.EndVertical();

            for (var i = 0; i < componentsToRemove.Count; i++)
            { _registerAsEntity.EntityData.Components.RemoveAt(componentsToRemove[i]); }
        }
        

        private void ComponentSelectionSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                var availableTypes = ComponentLookup.AllComponents
                    .Where(x => !typeof(ViewComponent).IsAssignableFrom(x))
                    .Where(x => !_registerAsEntity.EntityData.Components.Select(y => y.GetType().FullName).Contains(x.FullName))
                    .ToArray();

                var types = availableTypes.Select(x => string.Format("{0} [{1}]", x.Name, x.Namespace)).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);

                if (index < 0) { return; }

                var componentType = availableTypes.ElementAt(index);
                var component = (IComponent)Activator.CreateInstance(componentType);
                _registerAsEntity.EntityData.Components.Add(component);
            });
        }

        private void PersistChanges()
        {
            if (GUI.changed)
            {
                this._registerAsEntity.SerializeState();
                this.SaveActiveSceneChanges();
            }
        }

        private void OnEnable()
        {
            _registerAsEntity = (RegisterAsEntity)target;
            InjectIntoTarget();
            _registerAsEntity.DeserializeState();
        }

        private void InjectIntoTarget()
        {
            var serializer = EditorContext.Container.Resolve<IBinarySerializer>();
            var deserializer = EditorContext.Container.Resolve<IBinaryDeserializer>();
            _registerAsEntity.Serializer = serializer;
            _registerAsEntity.Deserializer = deserializer;
        }

        public override void OnInspectorGUI()
        {
            if (!_registerAsEntity.HasDeserialized)
            {
                _registerAsEntity.DeserializeState();
                return;
            }

            PoolSection();
            EditorGUILayout.Space();
            ComponentSelectionSection();
            ComponentListings();
            PersistChanges();
            
        }
    }
}
