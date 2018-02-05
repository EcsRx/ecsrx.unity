using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// ListView with int values.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ListViewInt")]
	public class ListViewInt : ListViewCustom<ListViewIntComponentBase,int> {
		/// <summary>
		/// Sets component data with specified item.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="item">Item.</param>
		protected override void SetData(ListViewIntComponentBase component, int item)
		{
			component.SetData(item);
		}

		/// <summary>
		/// Set highlights colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void HighlightColoring(ListViewIntComponentBase component)
		{
			component.Coloring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}

		/// <summary>
		/// Set select colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void SelectColoring(ListViewIntComponentBase component)
		{
			component.Coloring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}

		/// <summary>
		/// Set default colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void DefaultColoring(ListViewIntComponentBase component)
		{
			component.Coloring(DefaultColor, DefaultBackgroundColor, FadeDuration);
		}
	}
}