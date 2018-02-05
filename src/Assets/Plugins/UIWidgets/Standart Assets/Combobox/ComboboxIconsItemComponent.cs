using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// ComboboxIcons item component.
	/// Demonstrate how to remove selected item - add Remove() call on Button.OnClick().
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ComboboxIconsItemComponent")]
	public class ComboboxIconsItemComponent : ListViewIconsItemComponent {
		/// <summary>
		/// ComboboxIcons.
		/// </summary>
		public ComboboxIcons ComboboxIcons;

		/// <summary>
		/// Remove this instance.
		/// </summary>
		public void Remove()
		{
			ComboboxIcons.ListView.Deselect(Index);
		}
	}
}