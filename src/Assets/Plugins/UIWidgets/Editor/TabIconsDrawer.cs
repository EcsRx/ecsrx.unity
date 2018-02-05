using UnityEditor;
using UnityEngine;

namespace UIWidgets
{
	[CustomPropertyDrawer (typeof(TabIcons))]
	public class TabIconsDrawer : PropertyDrawer {
		
		// Draw the property inside the given rect
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Using BeginProperty / EndProperty on the parent property means that
			// prefab override logic works on the entire property.
			EditorGUI.BeginProperty(position, label, property);
			
			// Draw label
			//position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			// Calculate rects
			var width = position.width / 4;
			var tabRect = new Rect(position.x + (width * 0), position.y, width, position.height);
			var nameRect = new Rect(position.x + (width * 1), position.y, width, position.height);
			var iconDefaultRect = new Rect(position.x + (width * 2), position.y, width, position.height);
			var iconActiveGORect = new Rect(position.x + (width * 3), position.y, width, position.height);

			// Draw fields - passs GUIContent.none to each so they are drawn without labels
			EditorGUI.PropertyField(tabRect, property.FindPropertyRelative("TabObject"), GUIContent.none);
			EditorGUI.LabelField(tabRect, new GUIContent("", "Tab Object"));

			EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Name"), GUIContent.none);
			EditorGUI.LabelField(nameRect, new GUIContent("", "Name"));

			EditorGUI.PropertyField(iconDefaultRect, property.FindPropertyRelative("IconDefault"), GUIContent.none);
			EditorGUI.LabelField(iconDefaultRect, new GUIContent("", "Default Icon"));

			EditorGUI.PropertyField(iconActiveGORect, property.FindPropertyRelative("IconActive"), GUIContent.none);
			EditorGUI.LabelField(iconActiveGORect, new GUIContent("", "Active Icon"));
			
			EditorGUI.EndProperty();
		}
	}
}
