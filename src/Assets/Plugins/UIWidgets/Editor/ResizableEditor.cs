using UnityEditor;
using System.Collections.Generic;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Resizable), true)]
	public class ResizableEditor : CursorsEditor {
		public ResizableEditor()
		{
			Cursors = new List<string>(){
				"CurrentCamera",
				"CursorEWTexture",
				"CursorEWHotSpot",
				"CursorNSTexture",
				"CursorNSHotSpot",
				"CursorNESWTexture",
				"CursorNESWHotSpot",
				"CursorNWSETexture",
				"CursorNWSEHotSpot",
				"DefaultCursorTexture",
				"DefaultCursorHotSpot",
			};
		}
	}
}