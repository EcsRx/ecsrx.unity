﻿using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Plugins.Views.Components;
using EcsRx.UnityEditor.Data;
using EcsRx.UnityEditor.Editor.Extensions;
using EcsRx.UnityEditor.Editor.Helpers;
using EcsRx.UnityEditor.Editor.Models;
using EcsRx.UnityEditor.Events;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcsRx.UnityEditor.Editor.UIAspects
{
    public class EntityDataUIAspect
    {
        private const string RemoveComponentMessage = "Are you sure you wish to remove this component and all its values from the entity?";

        private readonly IDictionary<string, ComponentEditorState> _componentShowList = new Dictionary<string, ComponentEditorState>();
        private readonly IList<IComponent> _componentsRemovalList = new List<IComponent>();

        public event ComponentEventHandler ComponentAdded, ComponentRemoved;

        public EntityData EntityData { get; set; }
        public global::UnityEditor.Editor LinkedEditor { get; set; }

        public EntityDataUIAspect(EntityData entityData, global::UnityEditor.Editor linkedEditor)
        {
            EntityData = entityData;
            LinkedEditor = linkedEditor;
        }

        public void DisplayUI()
        {
            ComponentSelectionSection();

            EditorGUILayout.Space();
            foreach (var component in EntityData.Components)
            {
                ComponentSection(component);
                GUILayout.Space(5.0f);
            }

            foreach (var componentToRemove in _componentsRemovalList)
            {
                EntityData.Components.Remove(componentToRemove);
                ComponentRemoved?.Invoke(this, new ComponentEvent(componentToRemove));
            }
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

            var types = availableTypes.Select(x => $"{x.Name} [{x.Namespace}]").ToArray();

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

            ComponentAdded?.Invoke(this, new ComponentEvent(component));
        }

        private void ComponentSection(IComponent component)
        {
            var backgroundColor = GUI.backgroundColor;
            var textColor = GUI.contentColor;
            var componentType = component.GetType();
            var componentName = componentType.Name;
            var componentHeadingColor = componentName.GetHashCode().ToMutedColor();
            var componentBackgroundColor = componentName.GetHashCode().ToMutedColor(0.15f);

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
                    iconStyle.normal.textColor = Color.white;
                    GUILayout.Label(isShowing ? "▲" : "▼", iconStyle, GUILayout.Width(20), GUILayout.Height(15));

                    GUI.contentColor = textColor;
                    var headingStyle = new GUIStyle { alignment = TextAnchor.MiddleLeft, fontSize = 12, fontStyle = FontStyle.Bold};
                    headingStyle.normal.textColor = Color.white;
                    GUILayout.Label(componentName, headingStyle);

                    var buttonStyle = GUI.skin.button;
                    buttonStyle.alignment = TextAnchor.MiddleRight;
                    buttonStyle.fixedWidth = 20.0f;
                    GUI.backgroundColor = backgroundColor;
                    if (GUILayout.Button("X", buttonStyle))
                    {
                        if (EditorUtility.DisplayDialog("Remove " + componentName, RemoveComponentMessage, "Yes", "No"))
                        { _componentsRemovalList.Add(component); }
                    }
                });

                if (Event.current.type == EventType.Repaint)
                { _componentShowList[componentName].InteractionArea = headingRect; }

                GUI.backgroundColor = backgroundColor;
                GUI.contentColor = textColor;

                if (!isShowing) { return; }
                
                var hasChanged = ComponentUIAspect.ShowComponentProperties(component);
                
                if(EditorApplication.isPlaying) { return; }
                
                if(hasChanged) { EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene()); }
            });
        }
    }
}