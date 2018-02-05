using UnityEngine;
using UnityEditor;
using System;

namespace UIWidgets {
	[CustomEditor(typeof(RangeSliderFloat), true)]
	[CanEditMultipleObjects]
	public class RangeSliderFloatEditor : Editor {
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			Array.ForEach(targets, x => ((RangeSliderFloat)x).EditorUpdate());
		}
	}
}