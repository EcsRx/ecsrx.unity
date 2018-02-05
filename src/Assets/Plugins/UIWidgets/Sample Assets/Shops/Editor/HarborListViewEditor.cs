using UnityEditor;
using UIWidgets;

namespace UIWidgetsSamples.Shops
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HarborListView), true)]
	public class HarborListViewEditor : ListViewCustomEditor
	{
		public HarborListViewEditor()
		{
			Properties.Remove("customItems");
		}
	}
}