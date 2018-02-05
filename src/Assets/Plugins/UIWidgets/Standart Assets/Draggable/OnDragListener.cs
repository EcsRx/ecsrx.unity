﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// OnDragListener.
	/// </summary>
	public class OnDragListener : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler {

		/// <summary>
		/// OnDrag event.
		/// </summary>
		[SerializeField]
		public PointerUnityEvent OnDragEvent = new PointerUnityEvent();

		/// <summary>
		/// OnDragStart event.
		/// </summary>
		[SerializeField]
		public PointerUnityEvent OnDragStartEvent = new PointerUnityEvent();

		/// <summary>
		/// OnDragEnd event.
		/// </summary>
		[SerializeField]
		public PointerUnityEvent OnDragEndEvent = new PointerUnityEvent();

		/// <summary>
		/// OnInitializePotentialDrag event.
		/// </summary>
		[SerializeField]
		public PointerUnityEvent OnInitializePotentialDragEvent = new PointerUnityEvent();


		/// <summary>
		/// When draging is occuring this will be called every time the cursor is moved.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnDrag(PointerEventData eventData)
		{
			OnDragEvent.Invoke(eventData);
		}

		/// <summary>
		/// Called by a BaseInputModule before a drag is started.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnBeginDrag (PointerEventData eventData)
		{
			OnDragStartEvent.Invoke(eventData);
		}

		/// <summary>
		/// Called by a BaseInputModule when a drag is ended.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnEndDrag (PointerEventData eventData)
		{
			OnDragEndEvent.Invoke(eventData);
		}

		/// <summary>
		/// Called by a BaseInputModule when a drag has been found but before it is valid to begin the drag.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnInitializePotentialDrag (PointerEventData eventData)
		{
			OnInitializePotentialDragEvent.Invoke(eventData);
		}
	}
}