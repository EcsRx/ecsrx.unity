using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {

	public class ListViewSlider : ListViewCustom<ListViewSliderComponent,ListViewSliderItem> {
		protected override void SetData(ListViewSliderComponent component, ListViewSliderItem item)
		{
			component.SetData(item);
		}
		
		protected override void HighlightColoring(ListViewSliderComponent component)
		{
		}
		
		protected override void SelectColoring(ListViewSliderComponent component)
		{
		}
		
		protected override void DefaultColoring(ListViewSliderComponent component)
		{
		}
	}
}