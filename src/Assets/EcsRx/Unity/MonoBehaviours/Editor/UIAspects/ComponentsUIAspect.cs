using System.Collections.Generic;
using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Unity.MonoBehaviours.Editor.EditorHelper;
using EcsRx.Unity.MonoBehaviours.Editor.Extensions;
using EcsRx.Unity.MonoBehaviours.Editor.Models;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers.UIAspects
{
    public class EntityUiAspect
    {
        private readonly IDictionary<string, ComponentEditorState> _componentShowList = new Dictionary<string, ComponentEditorState>();

        public IEntity Entity { get; set; }

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

            GUI.backgroundColor = componentBackgroundColor;
            EditorGUILayout.BeginVertical(EditorGUIHelper.DefaultBoxStyle);
            {
                GUI.backgroundColor = componentHeadingColor;
                var headingRect = EditorGUILayout.BeginHorizontal(EditorGUIHelper.DefaultBoxStyle);
                {
                    var headingStyle = new GUIStyle { alignment = TextAnchor.MiddleCenter };
                    EditorGUIHelper.DrawOutlinedLabel(componentName.ToUpper(), 1, headingStyle);
                }
                EditorGUILayout.EndHorizontal();

                if (Event.current.type == EventType.Repaint)
                { _componentShowList[componentName].InteractionArea = headingRect; }

                GUI.backgroundColor = backgroundColor;
                GUI.contentColor = textColor;

                if (_componentShowList[componentName].ShowProperties)
                { ComponentUIAspect.ShowComponentProperties(component); }
            }
            EditorGUILayout.EndVertical();
        }
    }
}