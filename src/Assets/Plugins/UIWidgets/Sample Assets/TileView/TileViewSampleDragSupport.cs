using UnityEngine;
using UnityEngine.EventSystems;
using UIWidgets;

namespace UIWidgetsSamples {
	/// <summary>
	/// TileViewSample drag support.
	/// </summary>
	[RequireComponent(typeof(TileViewComponentSample))]
	public class TileViewSampleDragSupport : DragSupport<TileViewItemSample> {
		[SerializeField]
		public TileViewSample TileView;

		[SerializeField]
		public TileViewComponentSample DragInfo;

		int index = 0;

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
			var component = GetComponent<TileViewComponentSample>();
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
			if (success && (TileView!=null))
			{
				var first_index = TileView.DataSource.IndexOf(Data);
				var last_index = TileView.DataSource.LastIndexOf(Data);
				if (index==first_index)
				{
					TileView.DataSource.RemoveAt(index);
				}
				else if ((index+1)==last_index)
				{
					TileView.DataSource.RemoveAt(index+1);
				}
				else
				{
					TileView.DataSource.Remove(Data);
				}
			}

			base.Dropped(success);
		}
	}
}