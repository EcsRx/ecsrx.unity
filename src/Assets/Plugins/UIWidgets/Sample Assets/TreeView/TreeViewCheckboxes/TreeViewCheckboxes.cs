using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TreeViewCheckboxes : TreeViewCustom<TreeViewCheckboxesComponent,TreeViewCheckboxesItem> {
		public NodeEvent OnNodeCheckboxChanged = new NodeEvent();

		void NodeCheckboxChanged(int index)
		{
			OnNodeCheckboxChanged.Invoke(DataSource[index].Node);
		}

		protected override void AddCallback(ListViewItem item, int index)
		{
			base.AddCallback(item, index);

			(item as TreeViewCheckboxesComponent).NodeCheckboxChanged.AddListener(NodeCheckboxChanged);
		}

		protected override void RemoveCallback(ListViewItem item, int index)
		{
			if (item!=null)
			{
				(item as TreeViewCheckboxesComponent).NodeCheckboxChanged.RemoveListener(NodeCheckboxChanged);
				base.RemoveCallback(item, index);
			}
		}

		protected override void SetData(TreeViewCheckboxesComponent component, ListNode<TreeViewCheckboxesItem> item)
		{
			component.SetData(item.Node, item.Depth);
		}
		
		protected override void HighlightColoring(TreeViewCheckboxesComponent component)
		{
			component.Coloring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}
		
		protected override void SelectColoring(TreeViewCheckboxesComponent component)
		{
			component.Coloring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}
		
		protected override void DefaultColoring(TreeViewCheckboxesComponent component)
		{
			component.Coloring(DefaultColor, DefaultBackgroundColor, FadeDuration);
		}
	}
}