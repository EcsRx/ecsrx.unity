using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWidgets {
	/// <summary>
	/// ResizableHeaderDragCell.
	/// </summary>
	public class ResizableHeaderDragCell : DragSupport<ResizableHeaderDragCell> {

		/// <summary>
		/// The position.
		/// </summary>
		public int Position = -1;

		/// <summary>
		/// ResizableHeader.
		/// </summary>
		public ResizableHeader ResizableHeader;

		/// <summary>
		/// Determines whether this instance can be dragged.
		/// </summary>
		/// <returns><c>true</c> if this instance can be dragged; otherwise, <c>false</c>.</returns>
		/// <param name="eventData">Current event data.</param>
		public override bool CanDrag(PointerEventData eventData)
		{
			return !ResizableHeader.InActiveRegion && ResizableHeader.AllowReorder;
		}

		/// <summary>
		/// Set Data, which will be passed to Drop component.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		protected override void InitDrag(PointerEventData eventData)
		{
			ResizableHeader.ProcessCellReorder = true;
			Data = this;
		}

		/// <summary>
		/// Called when drop completed.
		/// </summary>
		/// <param name="success"><c>true</c> if Drop component received data; otherwise, <c>false</c>.</param>
		public override void Dropped(bool success)
		{
			ResizableHeader.ProcessCellReorder = false;

			base.Dropped(success);
		}
	}
}