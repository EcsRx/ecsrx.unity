using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// TreeView.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/TreeView")]
	public class TreeView : TreeViewCustom<TreeViewComponent,TreeViewItem> {

		/// <summary>
		/// Sets component data with specified item.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="item">Item.</param>
		protected override void SetData(TreeViewComponent component, ListNode<TreeViewItem> item)
		{
			component.SetData(item.Node, item.Depth);
		}

		/// <summary>
		/// Set highlights colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void HighlightColoring(TreeViewComponent component)
		{
			component.HighlightColoring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}
		
		/// <summary>
		/// Set select colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void SelectColoring(TreeViewComponent component)
		{
			component.SelectColoring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}
		
		/// <summary>
		/// Set default colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void DefaultColoring(TreeViewComponent component)
		{
			if (component!=null)
			{
				component.DefaultColoring(DefaultColor, DefaultBackgroundColor, FadeDuration);
			}
		}
	}
}