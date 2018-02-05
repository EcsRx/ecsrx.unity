using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// TreeView drop support.
	/// Receive drops from TreeView and ListViewIcons.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/TreeViewDropSupport")]
	[RequireComponent(typeof(TreeView))]
	public class TreeViewDropSupport : MonoBehaviour, IDropSupport<TreeNode<TreeViewItem>>, IDropSupport<ListViewIconsItemDescription> {
		TreeView source;

		/// <summary>
		/// Gets the current TreeView.
		/// </summary>
		/// <value>Current TreeView.</value>
		public TreeView Source {
			get {
				if (source==null)
				{
					source = GetComponent<TreeView>();
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
			return Source.Nodes==null || !Source.Nodes.Contains(data);
		}

		/// <summary>
		/// Process dropped data.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void Drop(TreeNode<TreeViewItem> data, PointerEventData eventData)
		{
			if (Source.Nodes==null)
			{
				Source.Nodes = new ObservableList<TreeNode<TreeViewItem>>();
			}

			Source.Nodes.Add(data);
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
			if (Source.Nodes==null)
			{
				Source.Nodes = new ObservableList<TreeNode<TreeViewItem>>();
			}
			
			var newItem = new TreeViewItem(data.Name) {
				LocalizedName = data.LocalizedName,
				Icon = data.Icon,
				Value = data.Value
			};
			var newNode = new TreeNode<TreeViewItem>(newItem);
			Source.Nodes.Add(newNode);
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