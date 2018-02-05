﻿using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace UIWidgets
{
	public class TileViewEditor : Editor
	{
		protected Dictionary<string,SerializedProperty> SerializedProperties = new Dictionary<string,SerializedProperty>();
		
		protected List<string> Properties = new List<string>{
			"customItems",
			"Multiple",
			"selectedIndex",
			
			"direction",
			
			"DefaultItem",
			"Container",
			"scrollRect",
			
			"defaultColor",
			"defaultBackgroundColor",
			
			"HighlightedColor",
			"HighlightedBackgroundColor",
			
			"selectedColor",
			"selectedBackgroundColor",
		};
		
		protected virtual void OnEnable()
		{
			Properties.ForEach(x => {
				SerializedProperties.Add(x, serializedObject.FindProperty(x));
			});
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			SerializedProperties.ForEach(x => EditorGUILayout.PropertyField(x.Value, true));
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}