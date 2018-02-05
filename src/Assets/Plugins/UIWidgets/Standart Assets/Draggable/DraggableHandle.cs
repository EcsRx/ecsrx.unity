using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace UIWidgets {
	/// <summary>
	/// Draggable handle.
	/// </summary>
	public class DraggableHandle : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler {
		RectTransform drag;
		Canvas canvas;
		RectTransform canvasRect;

		/// <summary>
		/// Set the specified draggable object.
		/// </summary>
		/// <param name="newDrag">New drag.</param>
		public void Drag(RectTransform newDrag)
		{
			drag = newDrag;
		}
		
		/// <summary>
		/// Raises the initialize potential drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			canvasRect = Utilites.FindTopmostCanvas(transform) as RectTransform;
			canvas = canvasRect.GetComponent<Canvas>();
		}
		
		/// <summary>
		/// Raises the drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (canvas==null)
			{
				throw new MissingComponentException(gameObject.name + " not in Canvas hierarchy.");
			}
			Vector2 cur_pos;
			Vector2 prev_pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(drag, eventData.position, eventData.pressEventCamera, out cur_pos);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(drag, eventData.position - eventData.delta, eventData.pressEventCamera, out prev_pos);

			var new_pos = new Vector3(
				drag.localPosition.x + (cur_pos.x - prev_pos.x),
				drag.localPosition.y + (cur_pos.y - prev_pos.y),
				drag.localPosition.z);

			drag.localPosition = new_pos;
		}
	}
}