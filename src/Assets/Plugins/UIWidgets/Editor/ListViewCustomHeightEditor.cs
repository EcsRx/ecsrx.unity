using UnityEditor;
using UIWidgets;

namespace UIWidgetsSamples
{
	public class ListViewCustomHeightEditor : ListViewCustomEditor
	{
		public ListViewCustomHeightEditor()
		{
			Properties.Add("ForceAutoHeightCalculation");
		}
	}
}