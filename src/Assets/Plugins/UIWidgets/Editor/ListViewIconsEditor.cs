using UnityEditor;

namespace UIWidgets
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(ListViewIcons), true)]
	public class ListViewIconsEditor : ListViewCustomBaseEditor
	{
		public ListViewIconsEditor()
		{
			Properties.Insert(1, "sort");
		}
	}
}