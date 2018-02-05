using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Reflection;
using System.Linq;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(ListViewItem), true)]
	public class ListViewItemEditor : Editor
	{
		protected Dictionary<string,SerializedProperty> SerializedProperties = new Dictionary<string,SerializedProperty>();
		protected Dictionary<string,SerializedProperty> SerializedEvents = new Dictionary<string,SerializedProperty>();

		protected List<string> Properties = new List<string>();
		protected List<string> Events = new List<string>();

		protected virtual void OnEnable()
		{
			Properties.Clear();
			Events.Clear();
			SerializedProperties.Clear();
			SerializedEvents.Clear();

			var property = serializedObject.GetIterator();
			property.NextVisible(true);
			while (property.NextVisible(false))
			{
				AddProperty(property);
			}

			Events.Sort();

			Properties.ForEach(x => SerializedProperties.Add(x, serializedObject.FindProperty(x)));
			Events.ForEach(x => SerializedEvents.Add(x, serializedObject.FindProperty(x)));
		}

		protected static bool DetectGenericType(object instance, string name)
		{
			Type type = instance.GetType();
			while (type != null)
			{
				if (type.FullName.StartsWith(name, StringComparison.InvariantCulture))
				{
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}

		protected void AddProperty(SerializedProperty property)
		{
			if (IsEvent(property))
			{
				Events.Add(property.name);
			}
			else
			{
				Properties.Add(property.name);
			}
		}

		protected bool IsEvent(SerializedProperty property)
		{
			var object_type = property.serializedObject.targetObject.GetType();
			var property_type = object_type.GetField(property.propertyPath, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

			return typeof(UnityEventBase).IsAssignableFrom(property_type.FieldType);
		}

		protected List<string> exclude = new List<string>(){
			"Icon",
			"Text",
			"Toggle",
			"Filler",
			"OnNodeExpand",
			"AnimateArrow",
			"NodeOpened",
			"NodeClosed",
			"PaddingPerLevel",
			"SetNativeSize",
		};

		protected bool ShowEvents;

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var isTreeViewNode = DetectGenericType(serializedObject.targetObject, "UIWidgets.TreeViewComponentBase`1");
			if (isTreeViewNode)
			{
				EditorGUILayout.PropertyField(SerializedProperties["Icon"], true);
				EditorGUILayout.PropertyField(SerializedProperties["Text"], true);
				EditorGUILayout.PropertyField(SerializedProperties["Toggle"], true);
				EditorGUILayout.PropertyField(SerializedProperties["Filler"], true);

				EditorGUILayout.PropertyField(SerializedProperties["OnNodeExpand"], true);
				EditorGUI.indentLevel++;
				if (SerializedProperties["OnNodeExpand"].enumValueIndex==0)// rotate
				{
					EditorGUILayout.PropertyField(SerializedProperties["AnimateArrow"], true);
				}
				else
				{
					EditorGUILayout.PropertyField(SerializedProperties["NodeOpened"], true);
					EditorGUILayout.PropertyField(SerializedProperties["NodeClosed"], true);
				}
				EditorGUI.indentLevel--;

				EditorGUILayout.PropertyField(SerializedProperties["PaddingPerLevel"], true);
				EditorGUILayout.PropertyField(SerializedProperties["SetNativeSize"], true);

				SerializedProperties.Where(x => !exclude.Contains(x.Key)).ForEach(x => EditorGUILayout.PropertyField(x.Value, true));
			}
			else
			{
				SerializedProperties.ForEach(x => EditorGUILayout.PropertyField(x.Value, true));
			}

			EditorGUILayout.BeginVertical();
			ShowEvents = GUILayout.Toggle(ShowEvents, "Events", "Foldout", GUILayout.ExpandWidth(true));
			if (ShowEvents)
			{
				SerializedEvents.ForEach(x => EditorGUILayout.PropertyField(x.Value, true));
			}
			EditorGUILayout.EndVertical();

			serializedObject.ApplyModifiedProperties();
		}
	}
}