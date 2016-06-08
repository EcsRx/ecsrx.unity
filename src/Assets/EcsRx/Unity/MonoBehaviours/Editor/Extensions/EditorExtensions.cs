using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcsRx.Unity.Helpers.Extensions
{
    public static class EditorExtensions
    {
        public static readonly RectOffset DefaultPadding = new RectOffset(5, 5, 5, 5);
        public static readonly GUIStyle DefaultBoxStyle = new GUIStyle(GUI.skin.box) { padding = DefaultPadding };

        public static void UseVerticalBoxLayout(this Editor editor, Action action)
        {
            EditorGUILayout.BeginVertical(DefaultBoxStyle);
            action();
            EditorGUILayout.EndVertical();
        }

        public static void WithVerticalLayout(this Editor editor, Action action)
        {
            EditorGUILayout.BeginVertical();
            action();
            EditorGUILayout.EndVertical();
        }

        public static void UseHorizontalBoxLayout(this Editor editor, Action action)
        {
            EditorGUILayout.BeginHorizontal(DefaultBoxStyle);
            action();
            EditorGUILayout.EndHorizontal();
        }

        public static void WithHorizontalLayout(this Editor editor, Action action)
        {
            EditorGUILayout.BeginHorizontal();
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

        // Only works with unity 5.3+
        public static void SaveActiveSceneChanges(this Editor editor)
        {
            var activeScene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(activeScene);
        }
    }
}