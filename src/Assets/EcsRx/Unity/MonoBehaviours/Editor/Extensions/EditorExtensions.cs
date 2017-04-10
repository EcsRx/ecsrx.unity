using System;
using EcsRx.Unity.MonoBehaviours.Editor.Models;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcsRx.Unity.Helpers.Extensions
{
    public static class EditorExtensions
    {
        public static readonly RectOffset DefaultPadding = new RectOffset(5, 5, 5, 5);
        public static readonly RectOffset DefaultMargin = new RectOffset(2, 2, 2, 2);
        public static readonly GUIStyle DefaultBoxStyle = new GUIStyle(GUI.skin.box) { padding = DefaultPadding, margin = DefaultMargin };

        public static void UseVerticalBoxLayout(this Editor editor, Action action, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(DefaultBoxStyle, options);
            action();
            EditorGUILayout.EndVertical();
        }

        public static void WithVerticalLayout(this Editor editor, Action action, GUIStyle style = null, params GUILayoutOption[] options)
        {
            if (style == null)
            { EditorGUILayout.BeginVertical(options); }
            else
            { EditorGUILayout.BeginVertical(style, options); }
            action();
            EditorGUILayout.EndVertical();
        }

        public static void UseHorizontalBoxLayout(this Editor editor, Action action, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(DefaultBoxStyle, options);
            action();
            EditorGUILayout.EndHorizontal();
        }

        public static void WithHorizontalLayout(this Editor editor, Action action, GUIStyle style = null, params GUILayoutOption[] options)
        {
            if(style == null)
            { EditorGUILayout.BeginHorizontal(options); }
            else
            { EditorGUILayout.BeginHorizontal(style, options); }
            action();
            EditorGUILayout.EndHorizontal();
        }

        public static void WithLabel(this Editor editor, string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }

        public static bool WithIconButton(this Editor editor, string icon)
        {
            return GUILayout.Button(icon, GUILayout.Width(20), GUILayout.Height(15));
        }

        public static void WithLabelField(this Editor editor, string label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            editor.WithLabel(label);
            EditorGUILayout.LabelField(value);
            EditorGUILayout.EndHorizontal();
        }

        public static string WithTextField(this Editor editor, string label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            editor.WithLabel(label);
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

        public static void DrawOutlinedLabel(this Editor editor, string labelText, int strength, GUIStyle style)
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
        public static void SaveActiveSceneChanges(this Editor editor)
        {
            var activeScene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(activeScene);
        }
    }
}