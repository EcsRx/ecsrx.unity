using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcsRx.UnityEditor.Editor.Extensions
{
    public static class EditorExtensions
    {
        public static readonly RectOffset DefaultPadding = new RectOffset(5, 5, 5, 5);
        public static readonly GUIStyle DefaultBoxStyle = new GUIStyle(GUI.skin.box) { padding = DefaultPadding };

        public static void UseVerticalBoxLayout(this global::UnityEditor.Editor editor, Action action)
        {
            EditorGUILayout.BeginVertical(DefaultBoxStyle);
            action();
            EditorGUILayout.EndVertical();
        }

        public static void WithVerticalLayout(this global::UnityEditor.Editor editor, Action action)
        {
            EditorGUILayout.BeginVertical();
            action();
            EditorGUILayout.EndVertical();
        }

        public static void UseHorizontalBoxLayout(this global::UnityEditor.Editor editor, Action action)
        {
            EditorGUILayout.BeginHorizontal(DefaultBoxStyle);
            action();
            EditorGUILayout.EndHorizontal();
        }

        public static void WithHorizontalLayout(this global::UnityEditor.Editor editor, Action action)
        {
            EditorGUILayout.BeginHorizontal();
            action();
            EditorGUILayout.EndHorizontal();
        }

        public static void WithLabel(this global::UnityEditor.Editor editor, string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }

        public static bool WithIconButton(this global::UnityEditor.Editor editor, string icon)
        {
            return GUILayout.Button(icon, GUILayout.Width(20), GUILayout.Height(15));
        }

        public static void WithLabelField(this global::UnityEditor.Editor editor, string label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            editor.WithLabel(label);
            EditorGUILayout.LabelField(value);
            EditorGUILayout.EndHorizontal();
        }

        public static string WithTextField(this global::UnityEditor.Editor editor, string label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            editor.WithLabel(label);
            var result = EditorGUILayout.TextField(value);
            EditorGUILayout.EndHorizontal();
            return result;
        }
        
        public static int WithNumberField(this global::UnityEditor.Editor editor, string label, int value)
        {
            EditorGUILayout.BeginHorizontal();
            editor.WithLabel(label);
            var result = EditorGUILayout.IntField(value);
            EditorGUILayout.EndHorizontal();
            return result;
        }
        
        public static float WithNumberField(this global::UnityEditor.Editor editor, string label, float value)
        {
            EditorGUILayout.BeginHorizontal();
            editor.WithLabel(label);
            var result = EditorGUILayout.FloatField(value);
            EditorGUILayout.EndHorizontal();
            return result;
        }

        // Only works with unity 5.3+
        public static void SaveActiveSceneChanges(this global::UnityEditor.Editor editor)
        {
            var activeScene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(activeScene);
        }
    }
}