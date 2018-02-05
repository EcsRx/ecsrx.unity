using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace UIWidgets {

	/// <summary>
	/// SlideBlock axis.
	/// </summary>
	public enum SlideBlockAxis
	{
		LeftToRight = 0,
		TopToBottom = 1,
		RightToLeft = 2,
		BottomToTop = 3,
	}

	/// <summary>
	/// SlideBlock.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/SlideBlock")]
	public class SlideBlock : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
		/// <summary>
		/// AnimationCurve.
		/// </summary>
		[SerializeField]
		[Tooltip("Requirements: start value should be less than end value; Recommended start value = 0; end value = 1;")]
		public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		/// <summary>
		/// Direction.
		/// </summary>
		public SlideBlockAxis Direction = SlideBlockAxis.LeftToRight;

		[SerializeField]
		bool isOpen;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is open.
		/// </summary>
		/// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
		public bool IsOpen {
			get {
				return isOpen;
			}
			set {
				isOpen = value;
				ResetPosition();
			}
		}

		[SerializeField]
		bool scrollRectSupport;

		/// <summary>
		/// Process children ScrollRect's drag events.
		/// </summary>
		/// <value><c>true</c> if scroll rect support; otherwise, <c>false</c>.</value>
		public bool ScrollRectSupport {
			get {
				return scrollRectSupport;
			}
			set {
				if (scrollRectSupport)
				{
					GetComponentsInChildren<ScrollRect>().ForEach(RemoveHandleEvents);
				}
				scrollRectSupport = value;
				if (scrollRectSupport)
				{
					GetComponentsInChildren<ScrollRect>().ForEach(AddHandleEvents);
				}
			}
		}

		[SerializeField]
		GameObject optionalHandle;

		/// <summary>
		/// Gets or sets the optional handle.
		/// </summary>
		/// <value>The optional handle.</value>
		public GameObject OptionalHandle {
			get {
				return optionalHandle;
			}
			set {
				if (optionalHandle!=null)
				{
					RemoveHandleEvents(optionalHandle);
				}
				optionalHandle = value;
				if (optionalHandle!=null)
				{
					AddHandleEvents(optionalHandle);
				}
			}
		}

		/// <summary>
		/// Use unscaled time.
		/// </summary>
		[SerializeField]
		public bool UnscaledTime = false;

		/// <summary>
		/// OnOpen event.
		/// </summary>
		public UnityEvent OnOpen = new UnityEvent();

		/// <summary>
		/// OnClose event.
		/// </summary>
		public UnityEvent OnClose = new UnityEvent();

		RectTransform rectTransform;

		/// <summary>
		/// Gets the RectTransform.
		/// </summary>
		/// <value>RectTransform.</value>
		protected RectTransform RectTransform {
			get {
				if (rectTransform==null)
				{
					rectTransform = transform as RectTransform;
				}
				return rectTransform;
			}
		}

		/// <summary>
		/// Position on Start().
		/// </summary>
		Vector2 closePosition;

		/// <summary>
		/// The current animation.
		/// </summary>
		IEnumerator currentAnimation;
		
		float size;

		string GetWarning()
		{
			var keys = Curve.keys;
			if (keys[0].value >= keys[keys.Length - 1].value)
			{
				return string.Format("Curve requirements: start value (current: {0}) should be less than end value (current: {1}).", keys[0].value, keys[keys.Length - 1].value);
			}
			return string.Empty;
		}

		void Start()
		{
			var warning = GetWarning();
			if (warning!=string.Empty)
			{
				Debug.LogWarning(warning, this);
			}

			closePosition = RectTransform.anchoredPosition;
			size = (IsHorizontal()) ? RectTransform.rect.width : RectTransform.rect.height;
			if (IsOpen)
			{
				if (SlideBlockAxis.LeftToRight==Direction)
				{
					closePosition = new Vector2(closePosition.x - size, closePosition.y);
				}
				if (SlideBlockAxis.RightToLeft==Direction)
				{
					closePosition = new Vector2(closePosition.x + size, closePosition.y);
				}
				else if (SlideBlockAxis.TopToBottom==Direction)
				{
					closePosition = new Vector2(closePosition.x, closePosition.y + size);
				}
				else if (SlideBlockAxis.BottomToTop==Direction)
				{
					closePosition = new Vector2(closePosition.x, closePosition.y - size);
				}
			}
						
			OptionalHandle = optionalHandle;

			ScrollRectSupport = scrollRectSupport;
		}

		void AddScrollRectHandle()
		{
			GetComponentsInChildren<ScrollRect>().ForEach(AddHandleEvents);
		}

		void RemoveScrollRectHandle()
		{
			GetComponentsInChildren<ScrollRect>().ForEach(RemoveHandleEvents);
		}

		void AddHandleEvents(Component handleComponent)
		{
			AddHandleEvents(handleComponent.gameObject);
		}

		void AddHandleEvents(GameObject handleObject)
		{
			var handle = handleObject.GetComponent<SlideBlockHandle>();
			if (handle==null)
			{
				handle = handleObject.AddComponent<SlideBlockHandle>();
			}
			handle.BeginDragEvent.AddListener(OnBeginDrag);
			handle.DragEvent.AddListener(OnDrag);
			handle.EndDragEvent.AddListener(OnEndDrag);
		}

		void RemoveHandleEvents(Component handleComponent)
		{
			RemoveHandleEvents(handleComponent.gameObject);
		}

		void RemoveHandleEvents(GameObject handleObject)
		{
			var handle = handleObject.GetComponent<SlideBlockHandle>();
			if (handle!=null)
			{
				handle.BeginDragEvent.RemoveListener(OnBeginDrag);
				handle.DragEvent.RemoveListener(OnDrag);
				handle.EndDragEvent.RemoveListener(OnEndDrag);
			}
		}

		void ResetPosition()
		{
			if (isOpen)
			{
				size = (IsHorizontal()) ? RectTransform.rect.width : RectTransform.rect.height;

				if (SlideBlockAxis.LeftToRight==Direction)
				{
					RectTransform.anchoredPosition = new Vector2(closePosition.x + size, RectTransform.anchoredPosition.y);
				}
				else if (SlideBlockAxis.RightToLeft==Direction)
				{
					RectTransform.anchoredPosition = new Vector2(closePosition.x - size, RectTransform.anchoredPosition.y);
				}
				else if (SlideBlockAxis.TopToBottom==Direction)
				{
					RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, closePosition.y - size);
				}
				else if (SlideBlockAxis.BottomToTop==Direction)
				{
					RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, closePosition.y + size);
				}
			}
			else
			{
				RectTransform.anchoredPosition = closePosition;
			}
		}

		/// <summary>
		/// Toggle this instance.
		/// </summary>
		public void Toggle()
		{
			if (IsOpen)
			{
				Close();
			}
			else
			{
				Open();
			}
		}

		/// <summary>
		/// Called by a BaseInputModule before a drag is started.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnBeginDrag(PointerEventData eventData)
		{
			size = (IsHorizontal()) ? RectTransform.rect.width : RectTransform.rect.height;
		}
		
		/// <summary>
		/// Called by a BaseInputModule when a drag is ended.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnEndDrag(PointerEventData eventData)
		{
			var pos = (IsHorizontal())
				? RectTransform.anchoredPosition.x - closePosition.x
				: RectTransform.anchoredPosition.y - closePosition.y;

			if (pos==0f)
			{
				isOpen = false;
				OnClose.Invoke();
				return ;
			}

			var sign = IsReverse() ? -1 : +1;

			if (pos==(size * sign))
			{
				isOpen = true;
				OnOpen.Invoke();
				return ;
			}

			var k = pos / (size * sign);
			if (k >= 0.5f)
			{
				isOpen = false;
				Open();
			}
			else
			{
				isOpen = true;
				Close();
			}
		}
		
		/// <summary>
		/// When draging is occuring this will be called every time the cursor is moved.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnDrag(PointerEventData eventData)
		{
			if (currentAnimation!=null)
			{
				StopCoroutine(currentAnimation);
			}

			Vector2 p1;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, eventData.pressEventCamera, out p1);
			Vector2 p2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position - eventData.delta, eventData.pressEventCamera, out p2);
			var delta = p1 - p2;

			if (SlideBlockAxis.LeftToRight==Direction)
			{
				var x = Mathf.Clamp(RectTransform.anchoredPosition.x + delta.x, closePosition.x, closePosition.x + size);
				RectTransform.anchoredPosition = new Vector2(x, RectTransform.anchoredPosition.y);
			}
			else if (SlideBlockAxis.RightToLeft==Direction)
			{
				var x = Mathf.Clamp(RectTransform.anchoredPosition.x + delta.x, closePosition.x - size, closePosition.x);
				RectTransform.anchoredPosition = new Vector2(x, RectTransform.anchoredPosition.y);
			}
			else if (SlideBlockAxis.TopToBottom==Direction)
			{
				var y = Mathf.Clamp(RectTransform.anchoredPosition.y + delta.y, closePosition.y - size, closePosition.y);
				RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, y);
			}
			else if (SlideBlockAxis.BottomToTop==Direction)
			{
				var y = Mathf.Clamp(RectTransform.anchoredPosition.y + delta.y, closePosition.y, closePosition.y + size);
				RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, y);
			}
		}

		/// <summary>
		/// Open this instance.
		/// </summary>
		public void Open()
		{
			if (IsOpen)
			{
				return ;
			}
			Animate();
		}

		/// <summary>
		/// Close this instance.
		/// </summary>
		public void Close()
		{
			if (!IsOpen)
			{
				return ;
			}
			Animate();
		}

		bool IsHorizontal()
		{
			return SlideBlockAxis.LeftToRight==Direction || SlideBlockAxis.RightToLeft==Direction;
		}

		bool IsReverse()
		{
			return SlideBlockAxis.RightToLeft==Direction || SlideBlockAxis.TopToBottom==Direction;
		}

		void Animate()
		{
			if (currentAnimation!=null)
			{
				StopCoroutine(currentAnimation);
			}

			var animationLength = Curve.keys[Curve.keys.Length - 1].time;

			size = IsHorizontal() ? RectTransform.rect.width : RectTransform.rect.height;

			float movement;

			var startPosition = IsHorizontal() ? RectTransform.anchoredPosition.x : RectTransform.anchoredPosition.y;
			var endPosition = IsHorizontal() ? closePosition.x : closePosition.y;
			var delta = startPosition - endPosition;
			if (isOpen)
			{
				movement = delta;
			}
			else
			{
				var sign = IsReverse() ? -1 : +1;
				movement = (size * sign) - delta;
			}

			var acceleration = (movement==0f) ? 1f : size / movement;
			if (IsReverse())
			{
				acceleration *= -1;
			}

			currentAnimation = RunAnimation(animationLength, startPosition, movement, acceleration);
			StartCoroutine(currentAnimation);
		}

		IEnumerator RunAnimation(float animationLength, float startPosition, float movement, float acceleration)
		{
			float delta;
			var direction = isOpen ? -1 : +1;

			var startTime = (UnscaledTime ? Time.unscaledTime : Time.time);
			do
			{
				delta = ((UnscaledTime ? Time.unscaledTime : Time.time) - startTime) * acceleration;
				var value = Curve.Evaluate(delta);
				if (IsHorizontal())
				{
					RectTransform.anchoredPosition = new Vector2(startPosition + (value * movement * direction), RectTransform.anchoredPosition.y);
				}
				else
				{
					RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, startPosition + (value * movement * direction));
				}
				yield return null;
			}
			while (delta < animationLength);

			isOpen = !isOpen;
			ResetPosition();

			if (IsOpen)
			{
				OnOpen.Invoke();
			}
			else
			{
				OnClose.Invoke();
			}
		}
	}
}