using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UIWidgets {

	[CustomEditor(typeof(TreeViewDataSource))]
	public class TreeViewDataSourceEditor : Editor {

		public override void OnInspectorGUI()
		{
			//base.DrawDefaultInspector();
			if (GUILayout.Button("Edit"))
			{
				TreeViewDataSourceWindow.Init();
			}
		}
	}
}