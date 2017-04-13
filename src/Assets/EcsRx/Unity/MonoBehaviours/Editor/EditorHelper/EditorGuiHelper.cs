using System;
using EcsRx.Unity.MonoBehaviours.Editor.Models;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcsRx.Unity.MonoBehaviours.Editor.EditorHelper
{
    public static class EditorGUIHelper
    {
        public static readonly RectOffset DefaultPadding = new RectOffset(5, 5, 5, 5);
        public static readonly RectOffset DefaultMargin = new RectOffset(2, 2, 2, 2);
        public static readonly GUIStyle DefaultBoxStyle = new GUIStyle(GUI.skin.box) { padding = DefaultPadding, margin = DefaultMargin };

        public static Rect WithVerticalBoxLayout(Action action, params GUILayoutOption[] options)
        {
            var rect = EditorGUILayout.BeginVertical(DefaultBoxStyle, options);
            action();
            EditorGUILayout.EndVertical();
            return rect;
        }

        public static Rect WithVerticalLayout(Action action, GUIStyle style = null, params GUILayoutOption[] options)
        {
            Rect rect;
            if (style == null)
            { rect = EditorGUILayout.BeginVertical(options); }
            else
            { rect = EditorGUILayout.BeginVertical(style, options); }
            action();
            EditorGUILayout.EndVertical();
            return rect;
        }

        public static Rect WithHorizontalBoxLayout(Action action, params GUILayoutOption[] options)
        {
            var rect = EditorGUILayout.BeginHorizontal(DefaultBoxStyle, options);
            action();
            EditorGUILayout.EndHorizontal();
            return rect;
        }

        public static Rect WithHorizontalLayout(Action action, GUIStyle style = null, params GUILayoutOption[] options)
        {
            Rect rect;
            if (style == null)
            { rect = EditorGUILayout.BeginHorizontal(options); }
            else
            { rect = EditorGUILayout.BeginHorizontal(style, options); }
            action();
            EditorGUILayout.EndHorizontal();
            return rect;
        }

        public static void WithLabel(string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }

        public static bool WithIconButton(string icon, string tooltip = null)
        {
            var content = new GUIContent(icon, tooltip);
            return GUILayout.Button(content, GUILayout.Width(20), GUILayout.Height(15));
        }

        public static void WithIconLabel(string icon)
        {
            GUILayout.Label(icon, EditorStyles.boldLabel, GUILayout.Width(20), GUILayout.Height(15));
        }

        public static void WithLabelField(string label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            WithLabel(label);
            EditorGUILayout.LabelField(value);
            EditorGUILayout.EndHorizontal();
        }

        public static string WithTextField(string label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(100));
            var result = EditorGUILayout.TextField(value);
            EditorGUILayout.EndHorizontal();
            return result;
        }

        public static bool HasBeenClicked(this ComponentEditorState editorState)
        {
            var isMouseDown = Event.current.type == EventType.MouseDown;
            if (isMouseDown)
            {
                var mousePos = Event.current.mousePosition;
                var isOverCurrentControl = editorState.InteractionArea.Contains(mousePos);

                if (isOverCurrentControl)
                { return true; }
            }
            return false;
        }

        public static void DrawOutlinedLabel(string labelText, int strength, GUIStyle style)
        {
            var originalColor = style.normal.textColor;
            var outlineColor = new Color(0, 0, 0, 0.5f);
            style.normal.textColor = outlineColor;
            
            var rect = EditorGUILayout.GetControlRect();
            int i;
            for (i = -strength; i <= strength; i++)
            {
                GUI.Label(new Rect(rect.x - strength, rect.y + i, rect.width, rect.height), labelText, style);
                GUI.Label(new Rect(rect.x + strength, rect.y + i, rect.width, rect.height), labelText, style);
            }
            for (i = -strength + 1; i <= strength - 1; i++)
            {
                GUI.Label(new Rect(rect.x + i, rect.y - strength, rect.width, rect.height), labelText, style);
                GUI.Label(new Rect(rect.x + i, rect.y + strength, rect.width, rect.height), labelText, style);
            }
            style.normal.textColor = Color.white;
            GUI.Label(rect, labelText, style);
            style.normal.textColor = originalColor;
        }

        // Only works with unity 5.3+
        public static void SaveActiveSceneChanges(this UnityEditor.Editor editor)
        {
            var activeScene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(activeScene);
        }
    }
}