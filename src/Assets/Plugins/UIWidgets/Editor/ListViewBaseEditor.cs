using UnityEditor;
using System.Collections.Generic;
using System;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	//[CustomEditor(typeof(ListViewBase), false)]
	public class ListViewBaseEditor : Editor
	{
		Dictionary<string,SerializedProperty> serializedProperties = new Dictionary<string,SerializedProperty>();
		
		string[] properties = new string[]{
			"items",
			"DestroyGameObjects",
			"Multiple",
			"selectedIndex",

			"Container",
		};
		
		protected virtual void OnEnable()
		{
			Array.ForEach(properties, x => {
				serializedProperties.Add(x, serializedObject.FindProperty(x));
			});
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedProperties["items"], true);
			EditorGUILayout.PropertyField(serializedProperties["DestroyGameObjects"]);
			EditorGUILayout.PropertyField(serializedProperties["Multiple"]);
			EditorGUILayout.PropertyField(serializedProperties["selectedIndex"]);
			EditorGUILayout.PropertyField(serializedProperties["Container"]);

			serializedObject.ApplyModifiedProperties();

			//Array.ForEach(targets, x => ((ListViewBase)x).UpdateItems());
		}
	}
}