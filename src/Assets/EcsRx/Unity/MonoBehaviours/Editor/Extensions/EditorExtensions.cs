using System;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers.Extensions
{
    public static class EditorExtensions
    {
        private static readonly RectOffset DefaultPadding = new RectOffset(5, 5, 5, 5);
        private static readonly GUIStyle DefaultBoxStyle = new GUIStyle(GUI.skin.box) { padding = DefaultPadding };

        public static void UseBoxLayout(this Editor editor, Action action)
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
    }
}