using UnityEngine;
using System;
using UIWidgets;

namespace UIWidgetsSamples {
	public class Table : ListViewCustom<TableRowComponent,TableRow> {
		// this function is required
		protected override void SetData(TableRowComponent component, TableRow item)
		{
			component.SetData(item);
		}

		// those functions are optional
		protected override void HighlightColoring(TableRowComponent component)
		{
			component.Coloring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}
		
		protected override void SelectColoring(TableRowComponent component)
		{
			component.Coloring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}
		
		protected override void DefaultColoring(TableRowComponent component)
		{
			component.Coloring(DefaultColor, DefaultBackgroundColor, FadeDuration);
		}
	}
}