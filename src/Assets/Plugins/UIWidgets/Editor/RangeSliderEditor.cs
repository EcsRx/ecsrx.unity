using UnityEngine;
using UnityEditor;
using System;

namespace UIWidgets {
	[CustomEditor(typeof(RangeSlider), true)]
	[CanEditMultipleObjects]
	public class RangeSliderEditor : Editor {
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			Array.ForEach(targets, x => ((RangeSlider)x).EditorUpdate());
		}
	}
}