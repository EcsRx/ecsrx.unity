using UnityEngine;
using System.Linq;
using System;
using UIWidgets;

namespace UIWidgetsSamples {

	public class TileViewIcons : TileView<ListViewIconsItemComponent,ListViewIconsItemDescription> {
		/// <summary>
		/// Sets component data with specified item.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="item">Item.</param>
		protected override void SetData(ListViewIconsItemComponent component, ListViewIconsItemDescription item)
		{
			component.SetData(item);
		}

		/// <summary>
		/// Set highlights colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void HighlightColoring(ListViewIconsItemComponent component)
		{
			component.HighlightColoring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}

		/// <summary>
		/// Set select colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void SelectColoring(ListViewIconsItemComponent component)
		{
			component.SelectColoring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}

		/// <summary>
		/// Set default colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void DefaultColoring(ListViewIconsItemComponent component)
		{
			component.DefaultColoring(DefaultColor, DefaultBackgroundColor, FadeDuration);
		}
	}
}