using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using System;
using System.Collections.Generic;

namespace UIWidgets {
	[CustomEditor(typeof(TabButton), true)]
	[CanEditMultipleObjects]
	public class TabButtonEditor : ButtonEditor {
		Dictionary<string,SerializedProperty> serializedProperties = new Dictionary<string,SerializedProperty>();
		
		string[] properties = new string[]{
			//"Name",
		};

		protected override void OnEnable()
		{
			Array.ForEach(properties, x => {
				var v = serializedObject.FindProperty(x);
				if (v==null)
				{
					//Debug.Log(x);
				}
				else
				{
					serializedProperties.Add(x, v);
				}
			});

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