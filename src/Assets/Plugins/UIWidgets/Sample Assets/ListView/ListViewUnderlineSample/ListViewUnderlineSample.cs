using UnityEngine;
using System;
using UIWidgets;

namespace UIWidgetsSamples {
	
	public class ListViewUnderlineSample : ListViewCustom<ListViewUnderlineSampleComponent,ListViewUnderlineSampleItemDescription> {
		bool isStartedListViewCustomSample = false;

		Comparison<ListViewUnderlineSampleItemDescription> itemsComparison = (x, y) => x.Name.CompareTo(y.Name);

		protected override void Awake()
		{
			Start();
		}
		
		public override void Start()
		{
			if (isStartedListViewCustomSample)
			{
				return ;
			}
			isStartedListViewCustomSample = true;
			
			base.Start();
			DataSource.Comparison = itemsComparison;
		}
		
		protected override void SetData(ListViewUnderlineSampleComponent component, ListViewUnderlineSampleItemDescription item)
		{
			component.SetData(item);
		}
		
		protected override void HighlightColoring(ListViewUnderlineSampleComponent component)
		{
			component.Coloring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}
		
		protected override void SelectColoring(ListViewUnderlineSampleComponent component)
		{
			component.Coloring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}
		
		protected override void DefaultColoring(ListViewUnderlineSampleComponent component)
		{
			component.Coloring(DefaultColor, DefaultBackgroundColor, FadeDuration);
		}
	}
}