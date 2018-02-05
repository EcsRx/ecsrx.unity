using UnityEngine;
using UnityEditor;

namespace UIWidgets {
	public class TreeViewDataSourceWindow : EditorWindow
	{
		Vector2 scrollPos;
		static GameObject currentGameObject;

		public static void Init()
		{
			var window = EditorWindow.GetWindow<TreeViewDataSourceWindow>();
			window.Show();
		}

		void OnEnable()
		{
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
			title = "TreeViewEditor";
			#else
			titleContent = new GUIContent("TreeViewEditor");
			#endif
		}

		void OnGUI()
		{
			if (Selection.activeGameObject!=null)
			{
				var component = Selection.activeGameObject.GetComponent<TreeViewDataSource>();
				if (component!=null)
				{
					currentGameObject = Selection.activeGameObject;
				}
			}

			var data = currentGameObject.GetComponent<TreeViewDataSource>();
			if (data==null)
			{
				GUILayout.Label("Please select TreeView in Hierarchy window.", EditorStyles.boldLabel);
				return ;
			}
			var serializedData = new SerializedObject(data);
			var propertyData = serializedData.FindProperty("Data");

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			int prev_depth = -1;

			Rect item_position = new Rect(position);
			item_position.x = 4;
			item_position.y = 4;
			item_position.height = 18;
			item_position.width -= 25;

			var n = propertyData.arraySize;
			for (int i = 0; i < n; i++)
			{
				if (i==n)
				{
					continue ;
				}
				var item = propertyData.GetArrayElementAtIndex(i);

				EditorGUILayout.BeginHorizontal();

				prev_depth = DisplayItem(item, prev_depth, i, item_position, propertyData, ref n);

				EditorGUILayout.EndHorizontal();

				item_position.y += 21;
			}
			EditorGUILayout.EndScrollView();

			FixTree(propertyData, n);

			if (GUILayout.Button("Add Node", GUILayout.Width(100)))
			{
				propertyData.InsertArrayElementAtIndex(propertyData.arraySize);
				//serializedData.ApplyModifiedProperties();
			}

			serializedData.ApplyModifiedProperties();
		}

		void FixTree(SerializedProperty list, int n)
		{
			int depth = 0;
			for (int i = 0; i < n; i++)
			{
				var sDepth = list.GetArrayElementAtIndex(i).FindPropertyRelative("Depth");
				if (sDepth.intValue < 0)
				{
					sDepth.intValue = 0;
				}
				if ((sDepth.intValue - depth) > 1)
				{
					sDepth.intValue = depth + 1;
				}
				depth = sDepth.intValue;
			}
		}

		int DisplayItem(SerializedProperty item, int prevDepth, int index, Rect itemPosition, SerializedProperty list, ref int n)
		{
			EditorGUI.BeginProperty(itemPosition, new GUIContent(), item);

			var sIsExpanded = item.FindPropertyRelative("IsExpanded");
			//var sIsVisible = item.FindPropertyRelative("IsVisible");
			var sIcon = item.FindPropertyRelative("Icon");
			var sName = item.FindPropertyRelative("Name");
			var sDepth = item.FindPropertyRelative("Depth");

			var buttons = 6;
			var buttonWidth = 25;
			var nameWidth = itemPosition.width - (20 * sDepth.intValue) - 4 - (buttonWidth + 4) - 140 - ((buttonWidth + 4) * buttons);

			var start = itemPosition.x + (20 * sDepth.intValue);

			var isExpandedRect = new Rect(start, itemPosition.y, buttonWidth, itemPosition.height);
			start += buttonWidth + 4;

			var iconRect = new Rect(start, itemPosition.y, 140, itemPosition.height);
			start += 140;

			var nameRect = new Rect(start, itemPosition.y, nameWidth, itemPosition.height);
			start += nameWidth;

			var moveLeftRect = new Rect(start, itemPosition.y, buttonWidth, itemPosition.height);
			start += buttonWidth + 4;

			var moveRightRect = new Rect(start, itemPosition.y, buttonWidth, itemPosition.height);
			start += buttonWidth + 4;

			var moveUpRect = new Rect(start, itemPosition.y, buttonWidth, itemPosition.height);
			start += buttonWidth + 4;

			var moveDownRect = new Rect(start, itemPosition.y, buttonWidth, itemPosition.height);
			start += buttonWidth + 4;

			var addRect = new Rect(start, itemPosition.y, buttonWidth, itemPosition.height);
			start += buttonWidth + 4;

			var deleteRect = new Rect(start, itemPosition.y, buttonWidth, itemPosition.height);
			start += buttonWidth + 4;


			GUILayout.Space(20 * sDepth.intValue + 4);

			if (GUILayout.Button(sIsExpanded.boolValue ? "-" : "+", GUILayout.Width(25)))
			{
				sIsExpanded.boolValue = !sIsExpanded.boolValue;
			}

			EditorGUI.LabelField(isExpandedRect, new GUIContent("", "Is expanded?"));

			EditorGUI.PropertyField(iconRect, sIcon, GUIContent.none);
			EditorGUI.LabelField(iconRect, new GUIContent("", "Icon"));

			EditorGUI.PropertyField(nameRect, sName, GUIContent.none);
			EditorGUI.LabelField(nameRect, new GUIContent("", "Name"));

			GUILayout.Space(nameWidth + 148);

			GUI.enabled = sDepth.intValue > 0;
			if (GUILayout.Button("←", GUILayout.Width(buttonWidth)))
			{
				NodeMoveLeft(index, n, list);
			}
			GUI.enabled = true;
			EditorGUI.LabelField(moveLeftRect, new GUIContent("", "Move node with subnodes left"));

			GUI.enabled = sDepth.intValue <= prevDepth;
			if (GUILayout.Button("→", GUILayout.Width(buttonWidth)))
			{
				NodeMoveRight(index, n, list);
			}
			GUI.enabled = true;
						EditorGUI.LabelField(moveRightRect, new GUIContent("", "Move node with subnodes right"));

			EditorGUI.EndProperty();

			GUI.enabled = index > 0;
			if (GUILayout.Button("↑", GUILayout.Width(buttonWidth)))
			{
				NodeMoveUp(index, n, list);
			}
			GUI.enabled = true;
			EditorGUI.LabelField(moveUpRect, new GUIContent("", "Move node with subnodes up"));

			GUI.enabled = (index + 1) < list.arraySize;
			if (GUILayout.Button("↓", GUILayout.Width(buttonWidth)))
			{
				NodeMoveDown(index, n, list);
			}
			GUI.enabled = true;
			EditorGUI.LabelField(moveDownRect, new GUIContent("", "Move node with subnodes down"));

			if (GUILayout.Button("+", GUILayout.Width(buttonWidth)))
			{
				list.InsertArrayElementAtIndex(index + 1);
			}
			EditorGUI.LabelField(addRect, new GUIContent("", "Add node after current"));

			if (GUILayout.Button("-", GUILayout.Width(buttonWidth)))
			{
				NodeDelete(index, n, list);

				n -= 1;
				return prevDepth;
			}
			EditorGUI.LabelField(deleteRect, new GUIContent("", "Delete current node"));

			return sDepth.intValue;
		}

		void NodeDelete(int index, int list_length, SerializedProperty list)
		{
			int depth = list.GetArrayElementAtIndex(index).FindPropertyRelative("Depth").intValue;
			for (int j = index + 1; j < list_length; j++)
			{
				var child = list.GetArrayElementAtIndex(j);
				if (child.FindPropertyRelative("Depth").intValue<=depth)
				{
					break;
				}
				child.FindPropertyRelative("Depth").intValue -= 1;
			}
			list.DeleteArrayElementAtIndex(index);
		}

		void NodeMoveLeft(int index, int list_length, SerializedProperty list)
		{
			ChangeDeep(index, list_length, list, -1);
		}

		void NodeMoveRight(int index, int list_length, SerializedProperty list)
		{
			ChangeDeep(index, list_length, list, 1);
		}

		void ChangeDeep(int index, int list_length, SerializedProperty list, int delta_depth)
		{
			if (delta_depth==0)
			{
				return ;
			}
			var sDepth = list.GetArrayElementAtIndex(index).FindPropertyRelative("Depth");
			var depth = sDepth.intValue;
			for (int j = index + 1; j < list_length; j++)
			{
				var child = list.GetArrayElementAtIndex(j);
				if (child.FindPropertyRelative("Depth").intValue<=depth)
				{
					break;
				}
				child.FindPropertyRelative("Depth").intValue += delta_depth;
			}
			sDepth.intValue += delta_depth;
		}

		void NodeMoveUp(int index, int list_length, SerializedProperty list)
		{
			var sDepth = list.GetArrayElementAtIndex(index).FindPropertyRelative("Depth");
			var depth = sDepth.intValue;
			var new_depth = (index==1) ? 0 :  list.GetArrayElementAtIndex(index - 2).FindPropertyRelative("Depth").intValue;

			list.MoveArrayElement(index, index - 1);

			for (int j = index + 1; j < list_length; j++)
			{
				var child = list.GetArrayElementAtIndex(j);
				if (child.FindPropertyRelative("Depth").intValue<=depth)
				{
					break;
				}
				list.MoveArrayElement(j, j - 1);
			}
			ChangeDeep(index - 1, list_length, list, new_depth - depth);
		}
		void NodeMoveDown(int index, int list_length, SerializedProperty list)
		{
			//list.MoveArrayElement(index, index - 1);

			var sDepth = list.GetArrayElementAtIndex(index).FindPropertyRelative("Depth");
			var depth = sDepth.intValue;

			int n = index;
			for (int j = index + 1; j < list_length; j++)
			{
				var child = list.GetArrayElementAtIndex(j);
				if (child.FindPropertyRelative("Depth").intValue<=depth)
				{
					break;
				}
				n += 1;
			}
			for (int j = n; j >= index; j--)
			{
				list.MoveArrayElement(j, j + 1);
			}
			var new_depth = ((n+1)==list_length) ? 0 :  list.GetArrayElementAtIndex(index).FindPropertyRelative("Depth").intValue;
			ChangeDeep(n, list_length, list, new_depth - depth);
		}

	}
}