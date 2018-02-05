using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace UIWidgets {
	[CustomEditor(typeof(ListViewPaginator), true)]
	[CanEditMultipleObjects]
	public class ListViewPaginatorEditor : Editor {

		Dictionary<string,SerializedProperty> Properties = new Dictionary<string,SerializedProperty>();
		string[] properties = new string[]{
			"ListView",
			"perPage",

			"DefaultPage",
			"ActivePage",
			"PrevPage",
			"NextPage",

			"FastDragDistance",
			"FastDragTime",

			"currentPage",
			"ForceScrollOnPage",
			
			"Animation",
			"Movement",
			"UnscaledTime",

			"OnPageSelect",
		};

		protected virtual void OnEnable()
		{
			Properties.Clear();

			Array.ForEach(properties, x => Properties.Add(x, serializedObject.FindProperty(x)));
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(Properties["ListView"], true);
			EditorGUILayout.PropertyField(Properties["perPage"], true);

			EditorGUILayout.PropertyField(Properties["DefaultPage"], true);
			EditorGUILayout.PropertyField(Properties["ActivePage"], true);
			EditorGUILayout.PropertyField(Properties["PrevPage"], true);
			EditorGUILayout.PropertyField(Properties["NextPage"], true);

			EditorGUILayout.PropertyField(Properties["FastDragDistance"], true);
			EditorGUILayout.PropertyField(Properties["FastDragTime"], true);

			EditorGUILayout.PropertyField(Properties["currentPage"], true);
			EditorGUILayout.PropertyField(Properties["ForceScrollOnPage"], true);

			EditorGUILayout.PropertyField(Properties["Animation"], true);
			EditorGUI.indentLevel++;
			if (Properties["Animation"].boolValue)
			{
				EditorGUILayout.PropertyField(Properties["Movement"], true);
				EditorGUILayout.PropertyField(Properties["UnscaledTime"], true);
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField(Properties["OnPageSelect"], true);

			serializedObject.ApplyModifiedProperties();
		}
	}
}