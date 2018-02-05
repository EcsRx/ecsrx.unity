using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples.Tasks {

	public class TaskPicker : Picker<Task,TaskPicker> {
		
		[SerializeField]
		public TaskView TaskView;

		public override void BeforeOpen(Task defaultValue)
		{
			// set default value
			TaskView.SelectedIndex = TaskView.DataSource.IndexOf(defaultValue);

			// add callback
			TaskView.OnSelectObject.AddListener(ListViewCallback);
		}

		// callback when value selected
		void ListViewCallback(int index)
		{
			// apply selected value and close picker
			Selected(TaskView.DataSource[index]);
		}

		public override void BeforeClose()
		{
			// remove callback
			TaskView.OnSelectObject.RemoveListener(ListViewCallback);
		}
	}
}