using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace UIWidgets {
	[CustomEditor(typeof(ScrollRectPaginator), true)]
	[CanEditMultipleObjects]
	public class ScrollRectPaginatorEditor : Editor {

		Dictionary<string,SerializedProperty> Properties = new Dictionary<string,SerializedProperty>();
		string[] properties = new string[]{
			"ScrollRect",

			"DefaultPage",
			"ActivePage",
			"PrevPage",
			"NextPage",
			
			"Direction",
			"pageSizeType",
			"pageSize",
			"pageSpacing",
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

			EditorGUILayout.PropertyField(Properties["ScrollRect"], true);

			EditorGUILayout.PropertyField(Properties["DefaultPage"], true);
			EditorGUILayout.PropertyField(Properties["ActivePage"], true);
			EditorGUILayout.PropertyField(Properties["PrevPage"], true);
			EditorGUILayout.PropertyField(Properties["NextPage"], true);

			EditorGUILayout.PropertyField(Properties["Direction"], true);
			EditorGUILayout.PropertyField(Properties["pageSizeType"], true);
			EditorGUI.indentLevel++;
			if (Properties["pageSizeType"].enumValueIndex==1)// fixed
			{
				EditorGUILayout.PropertyField(Properties["pageSize"], true);
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.PropertyField(Properties["pageSpacing"], true);
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