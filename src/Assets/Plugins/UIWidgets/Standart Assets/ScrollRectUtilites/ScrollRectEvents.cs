using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace UIWidgets
{
	/// <summary>
	/// ScrollRect events.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ScrollRectEvents")]
	[RequireComponent(typeof(ScrollRect))]
	public class ScrollRectEvents : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
		/// <summary>
		/// The required movement before raise events.
		/// </summary>
		[SerializeField]
		public float RequiredMovement = 50f;

		/// <summary>
		/// OnPullUp event.
		/// </summary>
		[SerializeField]
		public UnityEvent OnPullUp = new UnityEvent();

		/// <summary>
		/// OnPullDown event.
		/// </summary>
		[SerializeField]
		public UnityEvent OnPullDown = new UnityEvent();

		/// <summary>
		/// OnPullLeft event.
		/// </summary>
		[SerializeField]
		public UnityEvent OnPullLeft = new UnityEvent();
		
		/// <summary>
		/// OnPullRight event.
		/// </summary>
		[SerializeField]
		public UnityEvent OnPullRight = new UnityEvent();

		ScrollRect scrollRect;

		/// <summary>
		/// Gets the ScrollRect.
		/// </summary>
		/// <value>ScrollRect.</value>
		public ScrollRect ScrollRect {
			get {
				if (scrollRect==null)
				{
					scrollRect = GetComponent<ScrollRect>();
				}
				return scrollRect;
			}
		}

		bool initedPullUp;
		bool initedPullDown;
		bool initedPullLeft;
		bool initedPullRight;

		float MovementUp;
		float MovementDown;
		float MovementLeft;
		float MovementRight;

		/// <summary>
		/// Called by a BaseInputModule before a drag is started.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			ResetDrag();
		}

		/// <summary>
		/// Called by a BaseInputModule when a drag is ended.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnEndDrag(PointerEventData eventData)
		{
			ResetDrag();
		}

		/// <summary>
		/// Resets the drag values.
		/// </summary>
		protected virtual void ResetDrag()
		{
			initedPullUp = false;
			initedPullDown = false;
			initedPullLeft = false;
			initedPullRight = false;
			
			MovementUp = 0f;
			MovementDown = 0f;
			MovementLeft = 0f;
			MovementRight = 0f;
		}

        /// <summary>
        /// When draging is occuring this will be called every time the cursor is moved.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public virtual void OnDrag(PointerEventData eventData)
		{
			var scrollRectTransform = (ScrollRect.transform as RectTransform);
			var scroll_height = scrollRectTransform.rect.height;
			var scroll_width = scrollRectTransform.rect.width;

			var max_y = Mathf.Max(0f, ScrollRect.content.rect.height - scroll_height);
			var max_x = Mathf.Max(0f, ScrollRect.content.rect.width - scroll_width);

			if ((ScrollRect.content.anchoredPosition.y <= 0f) && (!initedPullUp))
			{
				MovementUp += -eventData.delta.y;
				if (MovementUp >= RequiredMovement)
				{
					initedPullUp = true;
					OnPullUp.Invoke();
				}
			}

			if ((ScrollRect.content.anchoredPosition.y >= max_y) && (!initedPullDown))
			{
				MovementDown += eventData.delta.y;
				if (MovementDown >= RequiredMovement)
				{
					initedPullDown = true;
					OnPullDown.Invoke();
				}
			}

			if ((ScrollRect.content.anchoredPosition.x <= 0f) && (!initedPullLeft))
			{
				MovementLeft += -eventData.delta.x;
				if (MovementLeft >= RequiredMovement)
				{
					initedPullLeft = true;
					OnPullLeft.Invoke();
				}
			}
			
			if ((ScrollRect.content.anchoredPosition.x >= max_x) && (!initedPullRight))
			{
				MovementRight += eventData.delta.x;
				if (MovementRight >= RequiredMovement)
				{
					initedPullRight = true;
					OnPullRight.Invoke();
				}
			}

		}
	}
}