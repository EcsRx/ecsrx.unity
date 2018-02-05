using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIWidgets {
	/// <summary>
	/// Pointer unity event.
	/// </summary>
	[System.Serializable]
	public class PointerUnityEvent : UnityEvent<PointerEventData> {

	}

	/// <summary>
	/// Button advanced. You can add callback on PointerEnter/Exit/Down/Up 
	/// </summary>
	public class ButtonAdvanced : UnityEngine.UI.Button,
		IPointerDownHandler,
		IPointerUpHandler,
		IPointerEnterHandler,
		IPointerExitHandler
	{
		/// <summary>
		/// What to do when the event system send a pointer down Event.
		/// </summary>
		public PointerUnityEvent onPointerDown = new PointerUnityEvent();

		/// <summary>
		/// What to do when the event system send a pointer up Event.
		/// </summary>
		public PointerUnityEvent onPointerUp = new PointerUnityEvent();

		/// <summary>
		/// What to do when the event system send a pointer enter Event.
		/// </summary>
		public PointerUnityEvent onPointerEnter = new PointerUnityEvent();

		/// <summary>
		/// What to do when the event system send a pointer exit Event.
		/// </summary>
		public PointerUnityEvent onPointerExit = new PointerUnityEvent();

		/// <summary>
		/// Raises the pointer down event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public override void OnPointerDown(PointerEventData eventData)
		{
			onPointerDown.Invoke(eventData);
			base.OnPointerDown(eventData);
		}

		/// <summary>
		/// Raises the pointer up event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public override void OnPointerUp(PointerEventData eventData)
		{
			onPointerUp.Invoke(eventData);
			base.OnPointerUp(eventData);
		}

		/// <summary>
		/// Raises the pointer enter event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public override void OnPointerEnter(PointerEventData eventData)
		{
			onPointerEnter.Invoke(eventData);
			base.OnPointerEnter(eventData);
		}

		/// <summary>
		/// Raises the pointer exit event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public override void OnPointerExit(PointerEventData eventData)
		{
			onPointerExit.Invoke(eventData);
			base.OnPointerExit(eventData);
		}
	}
}