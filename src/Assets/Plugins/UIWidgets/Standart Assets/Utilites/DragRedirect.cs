using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace UIWidgets {
	/// <summary>
	/// Redirect drag events from current gameobject to specified.
	/// </summary>
	public class DragRedirect : UIBehaviour, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
	{
		/// <summary>
		/// Drag events will be redirected to this gameobject.
		/// </summary>
		[SerializeField]
		public GameObject RedirectTo;

		/// <summary>
		/// Gets the handlers.
		/// </summary>
		/// <returns>The handlers.</returns>
		/// <typeparam name="T">The handler type.</typeparam>
		protected T[] GetHandlers<T>() where T : class
		{
			#if UNITY_4_6 || UNITY_4_7
			return Array.ConvertAll(RedirectTo.GetComponents(typeof(T)), x => (x as T));
			#else
			return RedirectTo.GetComponents<T>();
			#endif
		}

		/// <summary>
		/// Called by a BaseInputModule before a drag is started.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnBeginDrag(PointerEventData eventData)
		{
			foreach (var handler in GetHandlers<IBeginDragHandler>())
			{
				handler.OnBeginDrag(eventData);
			}
		}

		/// <summary>
		/// Called by a BaseInputModule when a drag has been found but before it is valid to begin the drag.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnInitializePotentialDrag(PointerEventData eventData)
		{
			foreach (var handler in GetHandlers<IInitializePotentialDragHandler>())
			{
				handler.OnInitializePotentialDrag(eventData);
			}
		}

		/// <summary>
		/// When draging is occuring this will be called every time the cursor is moved.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnDrag(PointerEventData eventData)
		{
			foreach (var handler in GetHandlers<IDragHandler>())
			{
				handler.OnDrag(eventData);
			}
		}

		/// <summary>
		/// Called by a BaseInputModule when a drag is ended.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnEndDrag(PointerEventData eventData)
		{
			foreach (var handler in GetHandlers<IEndDragHandler>())
			{
				handler.OnEndDrag(eventData);
			}
		}

		/// <summary>
		/// Called by a BaseInputModule when an OnScroll event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnScroll(PointerEventData eventData)
		{
			foreach (var handler in GetHandlers<IScrollHandler>())
			{
				handler.OnScroll(eventData);
			}
		}
	}
}