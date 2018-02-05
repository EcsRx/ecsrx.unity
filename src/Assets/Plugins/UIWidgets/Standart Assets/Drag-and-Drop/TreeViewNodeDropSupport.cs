using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// TreeViewNode drop support.
	/// Receive drops from TreeView and ListViewIcons.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/TreeViewNodeDropSupport")]
	[RequireComponent(typeof(TreeViewComponent))]
	public class TreeViewNodeDropSupport : MonoBehaviour, IDropSupport<TreeNode<TreeViewItem>>, IDropSupport<ListViewIconsItemDescription> {
		TreeViewComponent source;

		/// <summary>
		/// Gets the current TreeViewComponent.
		/// </summary>
		/// <value>Current TreeViewComponent.</value>
		public TreeViewComponent Source {
			get {
				if (source==null)
				{
					source = GetComponent<TreeViewComponent>();
				}
				return source;
			}
		}

		#region IDropSupport<TreeNode<TreeViewItem>>
		/// <summary>
		/// Determines whether this instance can receive drop with the specified data and eventData.
		/// </summary>
		/// <returns><c>true</c> if this instance can receive drop with the specified data and eventData; otherwise, <c>false</c>.</returns>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public bool CanReceiveDrop(TreeNode<TreeViewItem> data, PointerEventData eventData)
		{
			return data.CanBeParent(Source.Node);
		}

		/// <summary>
		/// Process dropped data.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void Drop(TreeNode<TreeViewItem> data, PointerEventData eventData)
		{
			data.Parent = Source.Node;
		}

		/// <summary>
		/// Process canceled drop.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void DropCanceled(TreeNode<TreeViewItem> data, PointerEventData eventData)
		{
		}
		#endregion

		#region IDropSupport<ListViewIconsItemDescription>
		/// <summary>
		/// Determines whether this instance can receive drop with the specified data and eventData.
		/// </summary>
		/// <returns><c>true</c> if this instance can receive drop with the specified data and eventData; otherwise, <c>false</c>.</returns>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public bool CanReceiveDrop(ListViewIconsItemDescription data, PointerEventData eventData)
		{
			return true;
		}

		/// <summary>
		/// Process dropped data.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void Drop(ListViewIconsItemDescription data, PointerEventData eventData)
		{
			var node = Source.Node;
			if (node.Nodes==null)
			{
				node.Nodes = new ObservableList<TreeNode<TreeViewItem>>();
			}

			var newItem = new TreeViewItem(data.Name) {
				LocalizedName = data.LocalizedName,
				Icon = data.Icon,
				Value = data.Value
			};
			var newNode = new TreeNode<TreeViewItem>(newItem);
			node.Nodes.Add(newNode);
		}

		/// <summary>
		/// Process canceled drop.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void DropCanceled(ListViewIconsItemDescription data, PointerEventData eventData)
		{
		}
		#endregion
	}
}