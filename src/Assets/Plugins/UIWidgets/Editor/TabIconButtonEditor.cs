using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using System;
using System.Collections.Generic;

namespace UIWidgets {
	[CustomEditor(typeof(TabIconButton), true)]
	[CanEditMultipleObjects]
	public class TabIconButtonEditor : ButtonEditor {
		Dictionary<string,SerializedProperty> serializedProperties = new Dictionary<string,SerializedProperty>();
		
		string[] properties = new string[]{
			"Name",
			"Icon",
		};

		protected override void OnEnable()
		{
			Array.ForEach(properties, x => serializedProperties.Add(x, serializedObject.FindProperty(x)));

			base.OnEnable();
		}

		public override void OnInspectorGUI()
		{
			serializedProperties.ForEach(x => EditorGUILayout.PropertyField(x.Value));
			serializedObject.ApplyModifiedProperties();

			base.OnInspectorGUI();
		}
	}
}