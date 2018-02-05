using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(ListView), true)]
	public class ListViewEditor : Editor
	{
		Dictionary<string,SerializedProperty> serializedProperties = new Dictionary<string,SerializedProperty>();
		
		string[] properties = new string[]{
			"Source",
			"strings",
			//"dataSource",
			"file",

			"CommentsStartWith",
			"sort",
			"Unique",
			"AllowEmptyItems",

			"Multiple",
			"selectedIndex",

			"direction",

			"backgroundColor",
			"textColor",
			"HighlightedBackgroundColor",
			"HighlightedTextColor",
			"selectedBackgroundColor",
			"selectedTextColor",
			"FadeDuration",

			"Container",
			"DefaultItem",
			"scrollRect",

			"OnSelectString",
			"OnDeselectString",
		};
		
		protected virtual void OnEnable()
		{
			Array.ForEach(properties, x => {
				var p = serializedObject.FindProperty(x);
				serializedProperties.Add(x, p);
			});
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedProperties["Source"]);
			
			EditorGUI.indentLevel++;
			if (serializedProperties["Source"].enumValueIndex==0)
			{
				var options = new GUILayoutOption[] {};
				EditorGUILayout.PropertyField(serializedProperties["strings"], new GUIContent("Items"), true, options);
				//EditorGUILayout.PropertyField(serializedProperties["dataSource"], new GUIContent("Items"), true, options);
			}
			else
			{
				EditorGUILayout.PropertyField(serializedProperties["file"]);
				EditorGUILayout.PropertyField(serializedProperties["CommentsStartWith"], true);
				EditorGUILayout.PropertyField(serializedProperties["AllowEmptyItems"]);
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField(serializedProperties["Unique"], new GUIContent("Only unique items"));
			EditorGUILayout.PropertyField(serializedProperties["sort"]);
			EditorGUILayout.PropertyField(serializedProperties["Multiple"]);
			EditorGUILayout.PropertyField(serializedProperties["selectedIndex"]);

			EditorGUILayout.PropertyField(serializedProperties["backgroundColor"]);
			EditorGUILayout.PropertyField(serializedProperties["textColor"]);
			EditorGUILayout.PropertyField(serializedProperties["HighlightedBackgroundColor"]);
			EditorGUILayout.PropertyField(serializedProperties["HighlightedTextColor"]);
			EditorGUILayout.PropertyField(serializedProperties["selectedBackgroundColor"]);
			EditorGUILayout.PropertyField(serializedProperties["selectedTextColor"]);
			EditorGUILayout.PropertyField(serializedProperties["FadeDuration"]);

			EditorGUILayout.PropertyField(serializedProperties["DefaultItem"]);
			EditorGUILayout.PropertyField(serializedProperties["Container"]);
			EditorGUILayout.PropertyField(serializedProperties["scrollRect"]);

			EditorGUILayout.PropertyField(serializedProperties["OnSelectString"]);
			EditorGUILayout.PropertyField(serializedProperties["OnDeselectString"]);

			serializedObject.ApplyModifiedProperties();
		}
	}
}