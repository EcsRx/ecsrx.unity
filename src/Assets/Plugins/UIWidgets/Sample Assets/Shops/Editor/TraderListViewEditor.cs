using UnityEditor;
using UIWidgets;

namespace UIWidgetsSamples.Shops
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(TraderListView), true)]
	public class TraderListViewEditor : ListViewCustomEditor
	{
		public TraderListViewEditor()
		{
			Properties.Remove("customItems");
		}
	}
}