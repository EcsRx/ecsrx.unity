using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using System.Collections.Generic;
using System;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Spinner), true)]
	public class SpinnerEditor : SelectableEditor
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

			"_plusButton",
			"_minusButton",
			"HoldStartDelay",
			"HoldChangeDelay",

			"onValueChangeInt",
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

			EditorGUILayout.PropertyField(serializedProperties["_min"]);
			EditorGUILayout.PropertyField(serializedProperties["_max"]);
			EditorGUILayout.PropertyField(serializedProperties["_step"]);
			EditorGUILayout.PropertyField(serializedProperties["_value"]);
			EditorGUILayout.PropertyField(serializedProperties["Validation"]);
			EditorGUILayout.PropertyField(serializedProperties["HoldStartDelay"]);
			EditorGUILayout.PropertyField(serializedProperties["HoldChangeDelay"]);
			EditorGUILayout.PropertyField(serializedProperties["_plusButton"]);
			EditorGUILayout.PropertyField(serializedProperties["_minusButton"]);

			EditorGUILayout.PropertyField(serializedProperties["m_TextComponent"]);
			
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
			
			EditorGUILayout.PropertyField(serializedProperties["m_Placeholder"]);
			EditorGUILayout.PropertyField(serializedProperties["m_CaretBlinkRate"]);
			EditorGUILayout.PropertyField(serializedProperties["m_SelectionColor"]);
			EditorGUILayout.PropertyField(serializedProperties["m_HideMobileInput"]);
			
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(serializedProperties[ValueChanged]);
			EditorGUILayout.PropertyField(serializedProperties[EndEdit]);

			EditorGUILayout.PropertyField(serializedProperties["onValueChangeInt"]);
			EditorGUILayout.PropertyField(serializedProperties["onPlusClick"]);
			EditorGUILayout.PropertyField(serializedProperties["onMinusClick"]);

			EditorGUI.EndDisabledGroup();
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}