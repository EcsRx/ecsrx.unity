using UnityEditor;
using System.Collections.Generic;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(BaseDragSupport), true)]
	public class DragSupportEditor : CursorsEditor {
		public DragSupportEditor()
		{
			Cursors = new List<string>(){
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