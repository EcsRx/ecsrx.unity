using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Persistence.Data;
using EcsRx.Unity.Components;
using EcsRx.Unity.Helpers;
using EcsRx.Unity.Helpers.UIAspects;
using EcsRx.Unity.MonoBehaviours.Editor.EditorHelper;
using EcsRx.Unity.MonoBehaviours.Editor.Extensions;
using EcsRx.Unity.MonoBehaviours.Editor.Models;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours.Editor.UIAspects
{
    public class EntityDataUIAspect
    {
        private const string RemoveComponentMessage = "Are you sure you wish to remove this component and all its values from the entity?";

        private readonly IDictionary<string, ComponentEditorState> _componentShowList = new Dictionary<string, ComponentEditorState>();
        private readonly IList<IComponent> _componentsRemovalList = new List<IComponent>();

        public EntityData EntityData { get; set; }
        public UnityEditor.Editor LinkedEditor { get; set; }

        public EntityDataUIAspect(EntityData entityData, UnityEditor.Editor linkedEditor)
        {
            EntityData = entityData;
            LinkedEditor = linkedEditor;
        }

        public void DisplayUI()
        {
            EntityIdSection();
            ComponentSelectionSection();

            EditorGUILayout.Space();
            foreach (var component in EntityData.Components)
            { ComponentSection(component); }

            foreach (var componentToRemove in _componentsRemovalList)
            { EntityData.Components.Remove(componentToRemove); }
            _componentsRemovalList.Clear();

            if (Event.current.type == EventType.MouseDown)
            {
                foreach (var componentState in _componentShowList.Values)
                {
                    if (componentState.HasBeenClicked())
                    {
                        componentState.ShowProperties = !componentState.ShowProperties;
                        LinkedEditor.Repaint();
                    }
                }
            }
        }

        private void ComponentSelectionSection()
        {
            var availableTypes = ComponentLookup.AllComponents
                .Where(x => !typeof(ViewComponent).IsAssignableFrom(x))
                .Where(x => !EntityData.Components.Select(y => y.GetType().FullName).Contains(x.FullName))
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
            EntityData.Components.Add(component);
        }

        private void ComponentSection(IComponent component)
        {
            var backgroundColor = GUI.backgroundColor;
            var textColor = GUI.contentColor;
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

            var isShowing = _componentShowList[componentName].ShowProperties;

            GUI.backgroundColor = componentBackgroundColor;
            EditorGUIHelper.WithVerticalBoxLayout(() =>
            {
                GUI.backgroundColor = componentHeadingColor;
                var headingRect = EditorGUIHelper.WithHorizontalBoxLayout(() =>
                {
                    var iconStyle = new GUIStyle { fontSize = 12 };
                    iconStyle.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    GUILayout.Label(isShowing ? "▲" : "▼", iconStyle, GUILayout.Width(20), GUILayout.Height(15));

                    GUI.contentColor = textColor;
                    var headingStyle = new GUIStyle { alignment = TextAnchor.MiddleLeft, fontSize = 12 };
                    headingStyle.normal.textColor = Color.white;
                    EditorGUIHelper.DrawOutlinedLabel(componentName, 1, headingStyle);
                });

                if (Event.current.type == EventType.Repaint)
                { _componentShowList[componentName].InteractionArea = headingRect; }

                GUI.backgroundColor = backgroundColor;
                GUI.contentColor = textColor;

                if (isShowing)
                {
                    ComponentUIAspect.ShowComponentProperties(component);

                    EditorGUILayout.Space();
                    if (GUILayout.Button("Remove Component"))
                    {
                        if (EditorUtility.DisplayDialog("Remove " + componentName, RemoveComponentMessage, "Yes", "No"))
                        { _componentsRemovalList.Add(component); }
                    }
                }
            });
        }

        private void EntityIdSection()
        {
            EditorGUIHelper.WithVerticalLayout(() =>
            {
                EditorGUIHelper.WithHorizontalLayout(() =>
                {
                    EditorGUILayout.LabelField("Entity Id", GUILayout.MaxWidth(100.0f));
                    var entityId = EditorGUILayout.TextField(EntityData.EntityId.ToString());
                    try
                    {
                        var entityGuid = new Guid(entityId);
                        EntityData.EntityId = entityGuid;
                    }
                    catch (Exception)
                    { }

                    if (EditorGUIHelper.WithIconButton("↻", "Generate new GUID"))
                    {
                        var newGuid = Guid.NewGuid();
                        EntityData.EntityId = newGuid;
                    }
                });
            });
        }
    }
}