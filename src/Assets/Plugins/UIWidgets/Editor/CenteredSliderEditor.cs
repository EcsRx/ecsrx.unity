using UnityEngine;
using UnityEditor;
using System;

namespace UIWidgets {
	[CustomEditor(typeof(CenteredSlider), true)]
	[CanEditMultipleObjects]
	public class CenteredSliderEditor : Editor {
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			Array.ForEach(targets, x => ((CenteredSlider)x).EditorUpdate());
		}
	}
}