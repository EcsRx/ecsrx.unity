using UnityEngine.EventSystems;

namespace UIWidgets {
	/// <summary>
	/// IDropSupport.
	/// Receive drops from DragSupport<T>.
	/// </summary>
	public interface IDropSupport<T>  {

		/// <summary>
		/// Determines whether this instance can receive drop with the specified data and eventData.
		/// </summary>
		/// <returns><c>true</c> if this instance can receive drop with the specified data and eventData; otherwise, <c>false</c>.</returns>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		bool CanReceiveDrop(T data, PointerEventData eventData);

		/// <summary>
		/// Process dropped data.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		void Drop(T data, PointerEventData eventData);

		/// <summary>
		/// Process canceled drop.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="eventData">Event data.</param>
		void DropCanceled(T data, PointerEventData eventData);
	}
}