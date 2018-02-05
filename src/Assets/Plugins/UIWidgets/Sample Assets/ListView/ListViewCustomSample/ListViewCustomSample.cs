using System;
using UIWidgets;

namespace UIWidgetsSamples {

	public class ListViewCustomSample : ListViewCustom<ListViewCustomSampleComponent,ListViewCustomSampleItemDescription> {
		bool isStartedListViewCustomSample = false;

		Comparison<ListViewCustomSampleItemDescription> itemsComparison = (x, y) => {
			return x.Name.CompareTo(y.Name);
		};

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

		protected override void SetData(ListViewCustomSampleComponent component, ListViewCustomSampleItemDescription item)
		{
			component.SetData(item);
		}

		protected override void HighlightColoring(ListViewCustomSampleComponent component)
		{
			component.Coloring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}

		protected override void SelectColoring(ListViewCustomSampleComponent component)
		{
			component.Coloring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}

		protected override void DefaultColoring(ListViewCustomSampleComponent component)
		{
			component.Coloring(DefaultColor, DefaultBackgroundColor, FadeDuration);
		}
	}
}