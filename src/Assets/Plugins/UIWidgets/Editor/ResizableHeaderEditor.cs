using UnityEditor;
using System.Collections.Generic;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(ResizableHeader), true)]
	public class ResizableHeaderEditor : CursorsEditor {
		public ResizableHeaderEditor()
		{
			Cursors = new List<string>(){
				"CurrentCamera",

				"CursorTexture",
				"CursorHotSpot",

				"AllowDropCursor",
				"AllowDropCursorHotSpot",

				"DeniedDropCursor",
				"DeniedDropCursorHotSpot",

				"DefaultCursorTexture",
				"DefaultCursorHotSpot",
			};
		}
	}
}