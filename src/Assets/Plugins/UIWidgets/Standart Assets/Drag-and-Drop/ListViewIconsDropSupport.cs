using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWidgets {
	/// <summary>
	/// ListViewIcons drop support.
	/// Receive drops from TreeView and ListViewIcons.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ListViewIconsDropSupport")]
	[RequireComponent(typeof(ListViewIcons))]
	public class ListViewIconsDropSupport : MonoBehaviour, IDropSupport<TreeNode<TreeViewItem>>, IDropSupport<ListViewIconsItemDescription> {
		ListViewIcons listView;

		/// <summary>
		/// Current ListViewIcons.
		/// </summary>
		/// <value>ListViewIcons.</value>
		public ListViewIcons ListView {
			get {
				if (listView==null)
				{
					listView = GetComponent<ListViewIcons>();
				}
				return listView;
			}
		}

		/// <summary>
		/// The drop indicator.
		/// </summary>
		[SerializeField]
		protected ListViewDropIndicator DropIndicator;

		#region IDropSupport<TreeNode<TreeViewItem>>
		/// <summary>
		/// Determines whether this instance can receive drop with the specified data and eventData.
		/// </summary>
		/// <returns><c>true</c> if this instance can receive drop with the specified data and eventData; otherwise, <c>false</c>.</returns>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public bool CanReceiveDrop(TreeNode<TreeViewItem> data, PointerEventData eventData)
		{
			var result = data.Nodes==null || data.Nodes.Count==0;

			if (result)
			{
				var index = ListView.GetNearestIndex(eventData);
				ShowDropIndicator(index);
			}
			return result;
		}

		/// <summary>
		/// Process dropped data.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void Drop(TreeNode<TreeViewItem> data, PointerEventData eventData)
		{
			var index = ListView.GetNearestIndex(eventData);

			var item = new ListViewIconsItemDescription() {
				Name = data.Item.Name,
				LocalizedName = data.Item.LocalizedName,
				Icon = data.Item.Icon,
				Value = data.Item.Value
			};
			AddItem(item, index);

			// remove node from tree
			data.Parent = null;

			HideDropIndicator();
		}

		/// <summary>
		/// Process canceled drop.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void DropCanceled(TreeNode<TreeViewItem> data, PointerEventData eventData)
		{
			HideDropIndicator();
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
			var index = ListView.GetNearestIndex(eventData);

			ShowDropIndicator(index);

			return true;
		}

		/// <summary>
		/// Process dropped data.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void Drop(ListViewIconsItemDescription data, PointerEventData eventData)
		{
			var index = ListView.GetNearestIndex(eventData);

			AddItem(data, index);

			HideDropIndicator();
		}

		/// <summary>
		/// Process canceled drop.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void DropCanceled(ListViewIconsItemDescription data, PointerEventData eventData)
		{
			HideDropIndicator();
		}
		#endregion

		/// <summary>
		/// Shows the drop indicator.
		/// </summary>
		/// <param name="index">Index.</param>
		void ShowDropIndicator(int index)
		{
			if (DropIndicator!=null)
			{
				DropIndicator.Show(index, ListView);
			}
		}

		/// <summary>
		/// Hides the drop indicator.
		/// </summary>
		void HideDropIndicator()
		{
			if (DropIndicator!=null)
			{
				DropIndicator.Hide();
			}
		}

		/// <summary>
		/// Add item to ListViewIcons.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="index">Index.</param>
		void AddItem(ListViewIconsItemDescription item, int index)
		{
			if (index==-1)
			{
				ListView.DataSource.Add(item);
			}
			else
			{
				ListView.DataSource.Insert(index, item);
			}
		}
	}
}