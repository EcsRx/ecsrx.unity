using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.Events;

namespace UIWidgets
{
	//[CanEditMultipleObjects]
	[CustomEditor(typeof(ListViewBase), true)]
	public class ListViewCustomBaseEditor : Editor
	{
		protected bool IsListViewCustom = false;
		protected bool IsListViewCustomHeight = false;
		protected bool IsTileView = false;
		protected bool IsTreeViewCustom = false;

		protected Dictionary<string,SerializedProperty> SerializedProperties = new Dictionary<string,SerializedProperty>();
		protected Dictionary<string,SerializedProperty> SerializedEvents = new Dictionary<string,SerializedProperty>();

		protected List<string> Properties = new List<string>{
			"customItems",
			"Multiple",
			"selectedIndex",
			
			"direction",

			"DefaultItem",
			"Container",
			"scrollRect",

			"LimitScrollValue",

			"defaultColor",
			"defaultBackgroundColor",

			"HighlightedColor",
			"HighlightedBackgroundColor",

			"selectedColor",
			"selectedBackgroundColor",

			"FadeDuration",

			//"OnSelectObject",

			"EndScrollDelay",
		};

		protected List<string> Events = new List<string>{
			"OnSelect",
			"OnDeselect",
			//"OnSubmit",
			//"OnCancel",
			//"OnItemSelect",
			//"OnItemCancel",
			//"OnFocusIn",
			//"OnFocusOut",
			"OnSelectObject",
			"OnDeselectObject",
			//"OnPointerEnterObject",
			//"OnPointerExitObject",
			"OnStartScrolling",
			"OnEndScrolling",
		};

		protected List<string> Exclude = new List<string>{
			"selectedIndicies",
			"sort",
			"itemHeight",
			"itemWidth",
			"KeepSelection",

			"OnSelectObject",
			
			"OnSubmit",
			"OnCancel",
			"OnItemSelect",
			"OnItemCancel",
			"OnFocusIn",
			"OnFocusOut",
			"OnPointerEnterObject",
			"OnPointerExitObject",
		};

		static bool DetectGenericType(object instance, string name)
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

		protected virtual void FillProperties()
		{
			var property = serializedObject.GetIterator();
			property.NextVisible(true);
			while (property.NextVisible(false))
			{
				AddProperty(property);
			}
		}

		protected void AddProperty(SerializedProperty property)
		{
			if (Exclude.Contains(property.name))
			{
				return ;
			}
			if (Events.Contains(property.name) || Properties.Contains(property.name))
			{
				return ;
			}

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
			if (property_type==null)
			{
				return false;
			}

			return typeof(UnityEventBase).IsAssignableFrom(property_type.FieldType);
		}

		protected virtual void OnEnable()
		{
			FillProperties();
			
			if (!IsListViewCustom)
			{
				IsListViewCustom = DetectGenericType(serializedObject.targetObject, "UIWidgets.ListViewCustom`2");
			}
			if (!IsListViewCustomHeight)
			{
				IsListViewCustomHeight = DetectGenericType(serializedObject.targetObject, "UIWidgets.ListViewCustomHeight`2");
			}
			if (!IsTileView)
			{
				IsTileView = DetectGenericType(serializedObject.targetObject, "UIWidgets.TileView`2");
			}
			if (!IsTreeViewCustom)
			{
				IsTreeViewCustom = DetectGenericType(serializedObject.targetObject, "UIWidgets.TreeViewCustom`2");
			}

			if (IsListViewCustomHeight)
			{
				var isCanCalculateHeight = false;
				var ourType = serializedObject.targetObject.GetType(); 
				var mi = ourType.GetMethod("IsItemCanCalculateHeight", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (mi!=null)
				{
					var isCanCalculateHeightFunc = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), serializedObject.targetObject, mi);
					isCanCalculateHeight = isCanCalculateHeightFunc.Invoke();
				}

				if (isCanCalculateHeight)
				{
					if (!Properties.Contains("ForceAutoHeightCalculation"))
					{
						Properties.Add("ForceAutoHeightCalculation");
					}
				}
				else
				{
					if (Properties.Contains("ForceAutoHeightCalculation"))
					{
						Properties.Remove("ForceAutoHeightCalculation");
					}
				}
				if (!Properties.Contains("itemHeight"))
				{
					Properties.Add("itemHeight");
				}
			}

			if (IsTreeViewCustom)
			{
				Properties.Remove("customItems");
				Properties.Remove("selectedIndex");
				Properties.Remove("direction");
			}

			if (IsListViewCustom)
			{
				Properties.ForEach(x => {
					var property = serializedObject.FindProperty(x);
					if (property!=null)
					{
						SerializedProperties.Add(x, property);
					}
				});
				Events.ForEach(x => {
					var property = serializedObject.FindProperty(x);
					if (property!=null)
					{
						SerializedEvents.Add(x, property);
					}
				});
			}
		}

		public bool ShowEvents;

		public override void OnInspectorGUI()
		{
			if (IsListViewCustom)
			{
				serializedObject.Update();

				SerializedProperties.ForEach(x => {
					if (x.Key=="customItems")
					{
						EditorGUILayout.PropertyField(x.Value, new GUIContent("Data Source"), true);
					}
					else
					{
						EditorGUILayout.PropertyField(x.Value, true);
					}
				});

				EditorGUILayout.BeginVertical();
				ShowEvents = GUILayout.Toggle(ShowEvents, "Events", "Foldout", GUILayout.ExpandWidth(true));
				if (ShowEvents)
				{
					SerializedEvents.ForEach(x => EditorGUILayout.PropertyField(x.Value, true));
				}
				EditorGUILayout.EndVertical();

				serializedObject.ApplyModifiedProperties();

				var showWarning = false;
				Array.ForEach(targets, x => {
					var ourType = x.GetType(); 
					
					var mi = ourType.GetMethod("CanOptimize", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
					
					if (mi!= null){
						var canOptimize = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), x, mi);
						showWarning |= !canOptimize.Invoke();
					}
				});
				if (showWarning)
				{
					if (IsTileView || IsTreeViewCustom)
					{
						EditorGUILayout.HelpBox("Virtualization requires specified ScrollRect and Container should have EasyLayout component.", MessageType.Warning);
					}
					else
					{
						EditorGUILayout.HelpBox("Virtualization requires specified ScrollRect and Container should have EasyLayout or Horizontal or Vertical Layout Group component.", MessageType.Warning);
					}
				}
			}
			else
			{
				DrawDefaultInspector();
			}
		}
	}
}