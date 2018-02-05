using UnityEngine;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples.ToDoList {
	public class ToDoListView : ListViewCustomHeight<ToDoListViewComponent,ToDoListItem> {
		bool isStarted = false;

		protected override void Awake()
		{
			Start();
		}

		public override void Start()
		{
			if (isStarted)
			{
				return ;
			}
			isStarted = true;

			base.Start();
		}
			
		protected override void SetData(ToDoListViewComponent component, ToDoListItem item)
		{
			component.SetData(item);
		}
			
		protected override void HighlightColoring(ToDoListViewComponent component)
		{
			component.Coloring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}
			
		protected override void SelectColoring(ToDoListViewComponent component)
		{
			component.Coloring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}
			
		protected override void DefaultColoring(ToDoListViewComponent component)
		{
			component.Coloring(DefaultColor, DefaultBackgroundColor, FadeDuration);
		}
	}
}