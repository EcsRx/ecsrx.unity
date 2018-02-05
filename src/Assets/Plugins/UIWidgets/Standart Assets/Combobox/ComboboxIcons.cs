using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets {
	/// <summary>
	/// Combobox with icons.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ComboboxIcons")]
	public class ComboboxIcons : ComboboxCustom<ListViewIcons,ListViewIconsItemComponent,ListViewIconsItemDescription>
	{
		/// <summary>
		/// Updates the current component with selected item.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="item">Item.</param>
		protected override void SetData(ListViewIconsItemComponent component, ListViewIconsItemDescription item)
		{
			component.SetData(item);
		}
	}
}