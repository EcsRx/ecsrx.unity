using UIWidgets;

namespace UIWidgetsSamples.Tasks {
	public class TaskView : ListViewCustom<TaskComponent,Task> {
		public static readonly System.Comparison<Task> ItemsComparison = (x, y) => x.Name.CompareTo(y.Name);

		bool isStartedTaskView = false;

		public override void Start()
		{
			if (isStartedTaskView)
			{
				return ;
			}
			isStartedTaskView = true;

			base.Start();
			DataSource.Comparison = ItemsComparison;
		}

		protected override void SetData(TaskComponent component, Task item)
		{
			component.SetData(item);
		}

		protected override void HighlightColoring(TaskComponent component)
		{
			component.Coloring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}

		protected override void SelectColoring(TaskComponent component)
		{
			component.Coloring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}

		protected override void DefaultColoring(TaskComponent component)
		{
			component.Coloring(DefaultColor, DefaultBackgroundColor, FadeDuration);
		}
	}
}