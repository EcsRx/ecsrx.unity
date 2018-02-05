using UnityEditor;
using UIWidgets;

namespace UIWidgetsSamples
{
	// No more required
	[CanEditMultipleObjects]
	//[CustomEditor(typeof(ListViewVariableHeight), true)]
	public class ListViewVariableHeightEditor : ListViewCustomHeightEditor
	{
		public ListViewVariableHeightEditor()
		{
			Properties.Add("itemHeight");
		}
	}
}