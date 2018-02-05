using UnityEngine;
using UnityEngine.EventSystems;
using UIWidgets;

namespace UIWidgetsSamples {
	/// <summary>
	/// TileViewSample drop support.
	/// </summary>
	[RequireComponent(typeof(TileViewSample))]
	public class TileViewSampleDropSupport : MonoBehaviour, IDropSupport<TileViewItemSample> {
		TileViewSample tistView;
		public TileViewSample TistView {
			get {
				if (tistView==null)
				{
						tistView = GetComponent<TileViewSample>();
				}
				return tistView;
			}
		}

		[SerializeField]
		protected ListViewDropIndicator DropIndicator;

		#region IDropSupport<TileViewItemSample>
		/// <summary>
		/// Determines whether this instance can receive drop with the specified data and eventData.
		/// </summary>
		/// <returns><c>true</c> if this instance can receive drop with the specified data and eventData; otherwise, <c>false</c>.</returns>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public bool CanReceiveDrop(TileViewItemSample data, PointerEventData eventData)
		{
			var index = TistView.GetNearestIndex(eventData);

			ShowDropIndicator(index);

			return true;
		}

		/// <summary>
		/// Process dropped data.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void Drop(TileViewItemSample data, PointerEventData eventData)
		{
			var index = TistView.GetNearestIndex(eventData);

			DropItem(data, index);

			HideDropIndicator();
		}

		/// <summary>
		/// Process canceled drop.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		public void DropCanceled(TileViewItemSample data, PointerEventData eventData)
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
				DropIndicator.Show(index, TistView);
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
		/// Add item to TileView.DataSource.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="index">Index.</param>
		void DropItem(TileViewItemSample item, int index)
		{
			if (index==-1)
			{
				TistView.DataSource.Add(item);
			}
			else
			{
				TistView.DataSource.Insert(index, item);
			}
		}
	}
}