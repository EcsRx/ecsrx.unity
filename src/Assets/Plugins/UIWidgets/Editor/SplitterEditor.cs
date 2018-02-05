using UnityEditor;
using System.Collections.Generic;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Splitter), true)]
	public class SplitterEditor : CursorsEditor {
		public SplitterEditor()
		{
			Cursors = new List<string>(){
				"CurrentCamera",
				"CursorTexture",
				"CursorHotSpot",
				"DefaultCursorTexture",
				"DefaultCursorHotSpot",
			};
		}
	}
}