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
using EcsRx.Unity.MonoBehaviours.Editor.EditorHelper;
using EcsRx.Unity.MonoBehaviours.Editor.Extensions;
using EcsRx.Unity.MonoBehaviours.Editor.Models;
using Persistity.Serialization.Binary;

namespace EcsRx.Unity.Helpers
{
    [Serializable]
    [CustomEditor(typeof(RegisterAsEntity))]
    public partial class RegisterAsEntityInspector : Editor
    {
        private RegisterAsEntity _registerAsEntity;

        private bool showComponents;
        private Color backgroundColor;
        private Color textColor;

        private readonly IDictionary<string, ComponentEditorState> _componentShowList = new Dictionary<string, ComponentEditorState>();

        private void PoolSection()
        {
            EditorGUIHelper.WithVerticalBoxLayout(() =>
            {
                _registerAsEntity.PoolName = EditorGUIHelper.WithTextField("Pool: ", _registerAsEntity.PoolName);
            });
        }

        private void ComponentListings()
        {
            var componentCount = _registerAsEntity.EntityData.Components.Count;

            EditorGUIHelper.WithVerticalBoxLayout(() =>
            {
                EditorGUIHelper.WithHorizontalLayout(() =>
                {

                });
            });
            EditorGUILayout.BeginVertical(EditorGUIHelper.DefaultBoxStyle);
            EditorGUIHelper.WithHorizontalLayout(() =>
            {
                EditorGUIHelper.WithLabel("Components (" + componentCount + ")");
                if (EditorGUIHelper.WithIconButton("▸")) { showComponents = false; }
                if (EditorGUIHelper.WithIconButton("▾")) { showComponents = true; }
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

                    EditorGUIHelper.WithVerticalBoxLayout(() =>
                    {
                        var namePortions = currentComponent.GetType().FullName.Split(',')[0].Split('.');
                        var typeName = namePortions.Last();
                        var typeNamespace = string.Join(".", namePortions.Take(namePortions.Length - 1).ToArray());

                        EditorGUIHelper.WithVerticalLayout(() =>
                        {
                            EditorGUIHelper.WithHorizontalLayout(() =>
                            {
                                if (EditorGUIHelper.WithIconButton("-"))
                                { componentsToRemove.Add(currentIndex); }

                                EditorGUIHelper.WithLabel(typeName);
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
            var availableTypes = ComponentLookup.AllComponents
                .Where(x => !typeof(ViewComponent).IsAssignableFrom(x))
                .Where(x => !_registerAsEntity.EntityData.Components.Select(y => y.GetType().FullName).Contains(x.FullName))
                .ToArray();

            var types = availableTypes.Select(x => string.Format("{0} [{1}]", x.Name, x.Namespace)).ToArray();

            var index = -1;
            EditorGUIHelper.WithHorizontalLayout(() =>
            {
                EditorGUILayout.LabelField("Add Component", GUILayout.MaxWidth(100.0f));
                index = EditorGUILayout.Popup(index, types);
            });

            if (index < 0) { return; }
            var componentType = availableTypes.ElementAt(index);
            var component = (IComponent)Activator.CreateInstance(componentType);
            _registerAsEntity.EntityData.Components.Add(component);
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

            EditorGUILayout.Space();
            EntityIdSection();

            EditorGUILayout.Space();
            ComponentSelectionSection();

            EditorGUILayout.Space();
            foreach (var component in _registerAsEntity.EntityData.Components)
            { ComponentSection(component); }

            if (Event.current.type == EventType.MouseDown)
            {
                foreach (var componentState in _componentShowList.Values)
                {
                    if (componentState.HasBeenClicked())
                    {
                        componentState.ShowProperties = !componentState.ShowProperties;
                        Repaint();
                    }
                }
            }

            /*
                PoolSection();
                EditorGUILayout.Space();
                ComponentSelectionSection();
                ComponentListings();
            */

            PersistChanges();
        }

        

        private void EntityIdSection()
        {
            EditorGUIHelper.WithVerticalLayout(() =>
            {
                EditorGUIHelper.WithHorizontalLayout(() =>
                {
                    EditorGUILayout.LabelField("Entity Id", GUILayout.MaxWidth(100.0f));
                    var entityId = EditorGUILayout.TextField(_registerAsEntity.EntityId.ToString());
                    try
                    {
                        var entityGuid = new Guid(entityId);
                        _registerAsEntity.EntityData.EntityId = entityGuid;
                        _registerAsEntity.EntityId = entityGuid;
                    }
                    catch (Exception)
                    { }

                    if (GUILayout.Button("↻", GUILayout.MaxWidth(25.0f)))
                    {
                        var newGuid = Guid.NewGuid();
                        _registerAsEntity.EntityData.EntityId = newGuid;
                        _registerAsEntity.EntityId = newGuid;
                    }
                });
            });
        }

        private void ComponentSection(IComponent component)
        {
            backgroundColor = GUI.backgroundColor;
            textColor = GUI.contentColor;
            var componentType = component.GetType();
            var componentName = componentType.Name;
            var componentBackgroundColor = componentName.GetHashCode().ToColor(0.3f);
            var componentHeadingColor = componentName.GetHashCode().ToColor(0.6f);

            if (!_componentShowList.ContainsKey(componentName))
            {
                var componentState = new ComponentEditorState
                {
                    ComponentName = componentName,
                    ShowProperties = false
                };
                _componentShowList.Add(componentName, componentState);
            }

            GUI.backgroundColor = componentBackgroundColor;
            EditorGUIHelper.WithVerticalBoxLayout(() =>
            {
                GUI.backgroundColor = componentHeadingColor;
                var headingRect = EditorGUIHelper.WithHorizontalBoxLayout(() =>
                {
                    var headingStyle = new GUIStyle { alignment = TextAnchor.MiddleCenter, fontSize = 12 };
                    EditorGUIHelper.DrawOutlinedLabel(componentName, 1, headingStyle);
                });

                if (Event.current.type == EventType.Repaint)
                { _componentShowList[componentName].InteractionArea = headingRect; }

                GUI.backgroundColor = backgroundColor;
                GUI.contentColor = textColor;

                if (_componentShowList[componentName].ShowProperties)
                { ComponentUIAspect.ShowComponentProperties(component); }
            });
        }
    }
}
