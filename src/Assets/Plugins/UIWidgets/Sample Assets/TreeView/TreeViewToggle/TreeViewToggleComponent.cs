using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TreeViewToggleComponent : TreeViewComponent {

		public void ProcessClick()
		{
			// toogle Item.Value - 1 = selected, 0 = unselected
			Item.Value = (Item.Value==0) ? 1 : 0;

			if (Item.Value==1)// if node selected
			{
				Debug.Log("selected: " + Item.Name);

				// activate corresponding GameObjects
			}
			else// if node deselected
			{
				Debug.Log("deselected: " + Item.Name);

				// deactivate corresponding GameObjects
			}
		}
	}
}