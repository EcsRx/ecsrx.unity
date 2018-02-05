using UnityEditor;
using System.Collections.Generic;
using System;

namespace UIWidgets {
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Progressbar), true)]
	public class ProgressbarEditor : Editor
	{
		Dictionary<string,SerializedProperty> serializedProperties = new Dictionary<string,SerializedProperty>();

		string[] properties = new string[]{
			"Max",
			"_value",
			"type",
			"Direction",

			"IndeterminateBar",
			"DeterminateBar",

			"EmptyBar",
			"EmptyBarText",
			"fullBar",
			"FullBarText",
			"BarMask",
			"textType",

			"Speed",
			"UnscaledTime",
		};

		protected void OnEnable()
		{
			Array.ForEach(properties, x => {
				var p = serializedObject.FindProperty(x);
				serializedProperties.Add(x, p);
			});
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			//base.OnInspectorGUI();

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(serializedProperties["Max"]);
			EditorGUILayout.PropertyField(serializedProperties["_value"]);
			EditorGUILayout.PropertyField(serializedProperties["type"]);

			EditorGUI.indentLevel++;
			if (serializedProperties["type"].enumValueIndex==0)
			{
				EditorGUILayout.PropertyField(serializedProperties["DeterminateBar"]);
				EditorGUILayout.PropertyField(serializedProperties["BarMask"]);
				EditorGUILayout.PropertyField(serializedProperties["EmptyBar"]);
				EditorGUILayout.PropertyField(serializedProperties["EmptyBarText"]);
				EditorGUILayout.PropertyField(serializedProperties["fullBar"]);
				EditorGUILayout.PropertyField(serializedProperties["FullBarText"]);
				EditorGUILayout.PropertyField(serializedProperties["textType"]);
			}
			else
			{
				EditorGUILayout.PropertyField(serializedProperties["IndeterminateBar"]);
				EditorGUILayout.PropertyField(serializedProperties["UnscaledTime"]);
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField(serializedProperties["Direction"]);
			EditorGUILayout.PropertyField(serializedProperties["Speed"]);

			serializedObject.ApplyModifiedProperties();

			Array.ForEach(targets, x => ((Progressbar)x).Refresh());
		}
	}
}