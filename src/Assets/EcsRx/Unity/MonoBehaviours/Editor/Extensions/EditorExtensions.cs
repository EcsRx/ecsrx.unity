using System;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers.Extensions
{
    public static class EditorExtensions
    {
        public static void UseBoxLayout(this Editor editor, Action action)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
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

        public static void WithField(this Editor editor, string label, string value)
        {
            editor.WithLabel(label);
            GUILayout.Label(value);
        }
    }
}