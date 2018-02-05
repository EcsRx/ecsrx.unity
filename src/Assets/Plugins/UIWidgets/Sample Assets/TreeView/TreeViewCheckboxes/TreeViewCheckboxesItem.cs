using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	[System.Serializable]
	public class TreeViewCheckboxesItem : TreeViewItem {
		[SerializeField]
		public bool Selected;

		public TreeViewCheckboxesItem(string itemName, Sprite itemIcon = null, bool itemSelected = false)
			: base(itemName, itemIcon)
		{
			Selected = itemSelected;
		}
	}
}