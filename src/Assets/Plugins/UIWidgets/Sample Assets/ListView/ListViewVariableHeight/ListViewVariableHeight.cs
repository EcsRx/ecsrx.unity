using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	
	public class ListViewVariableHeight : ListViewCustomHeight<ListViewVariableHeightComponent,ListViewVariableHeightItemDescription> {
		protected override void SetData(ListViewVariableHeightComponent component, ListViewVariableHeightItemDescription item)
		{
			component.SetData(item);
		}
		
		protected override void HighlightColoring(ListViewVariableHeightComponent component)
		{
			component.Coloring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}
		
		protected override void SelectColoring(ListViewVariableHeightComponent component)
		{
			component.Coloring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}
		
		protected override void DefaultColoring(ListViewVariableHeightComponent component)
		{
			component.Coloring(DefaultColor, DefaultBackgroundColor, FadeDuration);
		}
	}
}