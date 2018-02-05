using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(ListViewGameObjects), true)]
	public class ListViewGameObjectsEditor : Editor
	{
		Dictionary<string,SerializedProperty> serializedProperties = new Dictionary<string,SerializedProperty>();
		
		string[] properties = new string[]{
			"objects",

			"DestroyGameObjects",
			"Multiple",
			"selectedIndex",

			"Container",
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
			
			Array.ForEach(properties, x => {
				EditorGUILayout.PropertyField(serializedProperties[x], true);
			});

			serializedObject.ApplyModifiedProperties();
			
			//Array.ForEach(targets, x => ((ListView)x).UpdateItems());
		}
	}
}