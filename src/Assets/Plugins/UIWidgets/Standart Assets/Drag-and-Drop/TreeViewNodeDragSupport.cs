using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// TreeViewNode drag support.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/TreeViewNodeDragSupport")]
	[RequireComponent(typeof(TreeViewComponent))]
	public class TreeViewNodeDragSupport : DragSupport<TreeNode<TreeViewItem>> {
		/// <summary>
		/// The drag info.
		/// </summary>
		[SerializeField]
		public ListViewIconsItemComponent DragInfo;

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected virtual void Start()
		{
			if (DragInfo!=null)
			{
				DragInfo.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Set Data, which will be passed to Drop component.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		protected override void InitDrag(PointerEventData eventData)
		{
			Data = GetComponent<TreeViewComponent>().Node;

			ShowDragInfo();
		}

		/// <summary>
		/// Shows the drag info.
		/// </summary>
		protected virtual void ShowDragInfo()
		{
			if (DragInfo==null)
			{
				return ;
			}
			DragInfo.transform.SetParent(DragPoint, false);
			DragInfo.transform.localPosition = new Vector3(-5, 5, 0);

			DragInfo.SetData(new ListViewIconsItemDescription() {
				Name = Data.Item.Name,
				LocalizedName = Data.Item.LocalizedName,
				Icon = Data.Item.Icon,
				Value = Data.Item.Value
			});

			DragInfo.gameObject.SetActive(true);
		}

		/// <summary>
		/// Hides the drag info.
		/// </summary>
		protected virtual void HideDragInfo()
		{
			if (DragInfo==null)
			{
				return ;
			}
			DragInfo.gameObject.SetActive(false);
		}

		/// <summary>
		/// Called when drop completed.
		/// </summary>
		/// <param name="success"><c>true</c> if Drop component received data; otherwise, <c>false</c>.</param>
		public override void Dropped(bool success)
		{
			HideDragInfo();
			
			base.Dropped(success);
		}
	}
}