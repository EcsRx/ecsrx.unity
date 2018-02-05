using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWidgets {
	/// <summary>
	/// ListViewIcons drag support.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ListViewIconsDragSupport")]
	[RequireComponent(typeof(ListViewIconsItemComponent))]
	public class ListViewIconsDragSupport : DragSupport<ListViewIconsItemDescription> {
		/// <summary>
		/// ListViewIcons.
		/// </summary>
		[SerializeField]
		public ListViewIcons ListView;

		/// <summary>
		/// The drag info.
		/// </summary>
		[SerializeField]
		public ListViewIconsItemComponent DragInfo;

		int index = 0;

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
			var component = GetComponent<ListViewIconsItemComponent>();
			Data = component.Item;
			index = component.Index;

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

			DragInfo.SetData(Data);

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
			
			// remove used from current ListViewIcons.
			if (success && (ListView!=null))
			{
				var first_index = ListView.DataSource.IndexOf(Data);
				var last_index = ListView.DataSource.LastIndexOf(Data);
				if (index==first_index)
				{
					ListView.DataSource.RemoveAt(index);
				}
				else if ((index+1)==last_index)
				{
					ListView.DataSource.RemoveAt(index+1);
				}
				else
				{
					ListView.DataSource.Remove(Data);
				}
			}

			base.Dropped(success);
		}
	}
}