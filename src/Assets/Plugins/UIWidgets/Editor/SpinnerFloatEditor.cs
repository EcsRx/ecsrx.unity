using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using System.Collections.Generic;
using System;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(SpinnerFloat), true)]
	public class SpinnerFloatEditor : SelectableEditor
	{
		Dictionary<string,SerializedProperty> serializedProperties = new Dictionary<string,SerializedProperty>();

		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		const string ValueChanged = "m_OnValueChange";
		const string EndEdit = "m_EndEdit";
		#else
		const string ValueChanged = "m_OnValueChanged";
		const string EndEdit = "m_OnEndEdit";
		#endif

		protected string[] properties = new string[]{
			//InputField
			"m_TextComponent",
			"m_CaretBlinkRate",
			"m_SelectionColor",
			"m_HideMobileInput",
			"m_Placeholder",
			ValueChanged,
			EndEdit,
			
			//Spinner
			"_min",
			"_max",
			"_step",
			"_value",

			"Validation",

			"format",
			"DecimalSeparators",
			"_plusButton",
			"_minusButton",
			"HoldStartDelay",
			"HoldChangeDelay",

			"onValueChangeFloat",
			"onPlusClick",
			"onMinusClick",
		};
		
		protected override void OnEnable()
		{
			base.OnEnable();
			
			Array.ForEach(properties, x => {
				serializedProperties.Add(x, serializedObject.FindProperty(x));
			});
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			base.OnInspectorGUI();
			
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(serializedProperties["_min"], true);
			EditorGUILayout.PropertyField(serializedProperties["_max"], true);
			EditorGUILayout.PropertyField(serializedProperties["_step"], true);
			EditorGUILayout.PropertyField(serializedProperties["_value"], true);
			EditorGUILayout.PropertyField(serializedProperties["Validation"], true);
			EditorGUILayout.PropertyField(serializedProperties["format"], true);
			EditorGUILayout.PropertyField(serializedProperties["DecimalSeparators"], true);
			EditorGUILayout.PropertyField(serializedProperties["HoldStartDelay"], true);
			EditorGUILayout.PropertyField(serializedProperties["HoldChangeDelay"], true);
			EditorGUILayout.PropertyField(serializedProperties["_plusButton"], true);
			EditorGUILayout.PropertyField(serializedProperties["_minusButton"], true);
			
			EditorGUILayout.PropertyField(serializedProperties["m_TextComponent"], true);
			
			if (serializedProperties["m_TextComponent"] != null && serializedProperties["m_TextComponent"].objectReferenceValue != null)
			{
				var text = serializedProperties["m_TextComponent"].objectReferenceValue as UnityEngine.UI.Text;
				if (text.supportRichText)
				{
					EditorGUILayout.HelpBox("Using Rich Text with input is unsupported.", MessageType.Warning);
				}
				
				if (text.alignment != TextAnchor.UpperLeft &&
				    text.alignment != TextAnchor.UpperCenter &&
				    text.alignment != TextAnchor.UpperRight)
				{
					EditorGUILayout.HelpBox("Using a non upper alignment with input is unsupported.", MessageType.Warning);
				}
			}
			
			EditorGUI.BeginDisabledGroup(serializedProperties["m_TextComponent"] == null || serializedProperties["m_TextComponent"].objectReferenceValue == null);
			
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(serializedProperties["m_Placeholder"], true);
			EditorGUILayout.PropertyField(serializedProperties["m_CaretBlinkRate"], true);
			EditorGUILayout.PropertyField(serializedProperties["m_SelectionColor"], true);
			EditorGUILayout.PropertyField(serializedProperties["m_HideMobileInput"], true);
			
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(serializedProperties[ValueChanged]);
			EditorGUILayout.PropertyField(serializedProperties[EndEdit]);

			EditorGUILayout.PropertyField(serializedProperties["onValueChangeFloat"]);
			EditorGUILayout.PropertyField(serializedProperties["onPlusClick"]);
			EditorGUILayout.PropertyField(serializedProperties["onMinusClick"]);
			
			EditorGUI.EndDisabledGroup();
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}