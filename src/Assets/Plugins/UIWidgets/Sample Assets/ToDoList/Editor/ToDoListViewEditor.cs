using UnityEditor;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples.ToDoList
{
	// No more used.
	[CanEditMultipleObjects]
	//[CustomEditor(typeof(ToDoListView), true)]
	public class ToDoListViewEditor : ListViewCustomEditor
	{
		public ToDoListViewEditor()
		{
			Properties.Add("itemHeight");
		}
	}
}