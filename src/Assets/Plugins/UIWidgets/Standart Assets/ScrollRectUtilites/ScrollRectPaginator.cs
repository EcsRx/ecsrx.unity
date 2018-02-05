using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace UIWidgets {
	/// <summary>
	/// Paginator direction.
	/// Auto - detect direction from ScrollRect.Direction and size of ScrollRect.Content.
	/// Horizontal - horizontal.
	/// Vertical - vertical.
	/// </summary>
	public enum PaginatorDirection {
		Auto = 0,
		Horizontal = 1,
		Vertical = 2,
	}

	/// <summary>
	/// Page size type.
	/// Auto - use ScrollRect size.
	/// Fixed - fixed size.
	/// </summary>
	public enum PageSizeType {
		Auto = 0,
		Fixed = 1,
	}

	/// <summary>
	/// ScrollRectPageSelect event.
	/// </summary>
	[Serializable]
	public class ScrollRectPageSelect : UnityEvent<int> {

	}

	/// <summary>
	/// ScrollRect Paginator.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ScrollRectPaginator")]
	public class ScrollRectPaginator : MonoBehaviour {
		/// <summary>
		/// ScrollRect for pagination.
		/// </summary>
		[SerializeField]
		protected ScrollRect ScrollRect;

		/// <summary>
		/// DefaultPage template.
		/// </summary>
		[SerializeField]
		protected RectTransform DefaultPage;

		/// <summary>
		/// ScrollRectPage component of DefaultPage.
		/// </summary>
		protected ScrollRectPage SRDefaultPage;

		/// <summary>
		/// ActivePage.
		/// </summary>
		[SerializeField]
		protected RectTransform ActivePage;

		/// <summary>
		/// ScrollRectPage component of ActivePage.
		/// </summary>
		protected ScrollRectPage SRActivePage;

		/// <summary>
		/// The previous page.
		/// </summary>
		[SerializeField]
		protected RectTransform PrevPage;

		/// <summary>
		/// ScrollRectPage component of PrevPage.
		/// </summary>
		protected ScrollRectPage SRPrevPage;

		/// <summary>
		/// The next page.
		/// </summary>
		[SerializeField]
		protected RectTransform NextPage;

		/// <summary>
		/// ScrollRectPage component of NextPage.
		/// </summary>
		protected ScrollRectPage SRNextPage;

		/// <summary>
		/// The direction.
		/// </summary>
		[SerializeField]
		public PaginatorDirection Direction = PaginatorDirection.Auto;

		/// <summary>
		/// The type of the page size.
		/// </summary>
		[SerializeField]
		protected PageSizeType pageSizeType = PageSizeType.Auto;

		[SerializeField]
		protected float pageSpacing = 0f;

		public float PageSpacing {
			get {
				return pageSpacing;
			}
			set {
				pageSpacing = value;

				RecalculatePages();
			}
		}

		/// <summary>
		/// Minimal drag distance to fast scroll to next slide.
		/// </summary>
		[SerializeField]
		public float FastDragDistance = 30f;

		/// <summary>
		/// Max drag time to fast scroll to next slide.
		/// </summary>
		[SerializeField]
		public float FastDragTime = 0.5f;


		/// <summary>
		/// Gets or sets the type of the page size.
		/// </summary>
		/// <value>The type of the page size.</value>
		public virtual PageSizeType PageSizeType {
			get {
				return pageSizeType;
			}
			set {
				pageSizeType = value;
				RecalculatePages();
			}
		}

		/// <summary>
		/// The size of the page.
		/// </summary>
		[SerializeField]
		protected float pageSize;

		/// <summary>
		/// Gets or sets the size of the page.
		/// </summary>
		/// <value>The size of the page.</value>
		public virtual float PageSize {
			get {
				return pageSize;
			}
			set {
				pageSize = value;
				RecalculatePages();
			}
		}

		int pages;

		/// <summary>
		/// Gets or sets the pages count.
		/// </summary>
		/// <value>The pages.</value>
		public virtual int Pages {
			get {
				return pages;
			}
			protected set {
				pages = value;
				UpdatePageButtons();
			}
		}

		/// <summary>
		/// The current page number.
		/// </summary>
		[SerializeField]
		protected int currentPage;

		/// <summary>
		/// Gets or sets the current page number.
		/// </summary>
		/// <value>The current page.</value>
		public int CurrentPage {
			get {
				return currentPage;
			}
			set {
				GoToPage(value);
			}
		}

		/// <summary>
		/// The force scroll position to page.
		/// </summary>
		[SerializeField]
		public bool ForceScrollOnPage;

		/// <summary>
		/// Use animation.
		/// </summary>
		[SerializeField]
		public bool Animation = true;

		/// <summary>
		/// Movement curve.
		/// </summary>
		[SerializeField]
		[Tooltip("Requirements: start value should be less than end value; Recommended start value = 0; end value = 1;")]
		[FormerlySerializedAs("Curve")]
		public AnimationCurve Movement = AnimationCurve.EaseInOut(0, 0, 1, 1);

		/// <summary>
		/// Use unscaled time.
		/// </summary>
		[SerializeField]
		public bool UnscaledTime = true;

		/// <summary>
		/// OnPageSelect event.
		/// </summary>
		[SerializeField]
		public ScrollRectPageSelect OnPageSelect = new ScrollRectPageSelect();

		/// <summary>
		/// The default pages.
		/// </summary>
		protected List<ScrollRectPage> DefaultPages = new List<ScrollRectPage>();

		/// <summary>
		/// The default pages cache.
		/// </summary>
		protected List<ScrollRectPage> DefaultPagesCache = new List<ScrollRectPage>();

		/// <summary>
		/// The current animation.
		/// </summary>
		protected IEnumerator currentAnimation;

		/// <summary>
		/// Is animation running?
		/// </summary>
		protected bool isAnimationRunning;

		/// <summary>
		/// Is dragging ScrollRect?
		/// </summary>
		protected bool isDragging;

		/// <summary>
		/// The cursor position at drag start.
		/// </summary>
		protected Vector2 CursorStartPosition;

		bool isStarted;

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected virtual void Start()
		{
			if (isStarted)
			{
				return ;
			}
			isStarted = true;

			var resizeListener = ScrollRect.GetComponent<ResizeListener>();
			if (resizeListener==null)
			{
				resizeListener = ScrollRect.gameObject.AddComponent<ResizeListener>();
			}
			resizeListener.OnResize.AddListener(RecalculatePages);

			var contentResizeListener = ScrollRect.content.GetComponent<ResizeListener>();
			if (contentResizeListener==null)
			{
				contentResizeListener = ScrollRect.content.gameObject.AddComponent<ResizeListener>();
			}
			contentResizeListener.OnResize.AddListener(RecalculatePages);

			var dragListener = ScrollRect.GetComponent<OnDragListener>();
			if (dragListener==null)
			{
				dragListener = ScrollRect.gameObject.AddComponent<OnDragListener>();
			}
			dragListener.OnDragStartEvent.AddListener(OnScrollRectDragStart);
			dragListener.OnDragEvent.AddListener(OnScrollRectDrag);
			dragListener.OnDragEndEvent.AddListener(OnScrollRectDragEnd);

			ScrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);

			if (DefaultPage!=null)
			{
				SRDefaultPage = DefaultPage.GetComponent<ScrollRectPage>();
				if (SRDefaultPage==null)
				{
					SRDefaultPage = DefaultPage.gameObject.AddComponent<ScrollRectPage>();
				}
				SRDefaultPage.gameObject.SetActive(false);
			}

			if (ActivePage!=null)
			{
				SRActivePage = ActivePage.GetComponent<ScrollRectPage>();
				if (SRActivePage==null)
				{
					SRActivePage = ActivePage.gameObject.AddComponent<ScrollRectPage>();
				}
			}

			if (PrevPage!=null)
			{
				SRPrevPage = PrevPage.GetComponent<ScrollRectPage>();
				if (SRPrevPage==null)
				{
					SRPrevPage = PrevPage.gameObject.AddComponent<ScrollRectPage>();
				}
				SRPrevPage.SetPage(0);
				SRPrevPage.OnPageSelect.AddListener(Prev);
			}
			if (NextPage!=null)
			{
				SRNextPage = NextPage.GetComponent<ScrollRectPage>();
				if (SRNextPage==null)
				{
					SRNextPage = NextPage.gameObject.AddComponent<ScrollRectPage>();
				}
				SRNextPage.OnPageSelect.AddListener(Next);
			}

			RecalculatePages();
			var page = currentPage;
			currentPage = -1;
			GoToPage(page);
		}

		/// <summary>
		/// Determines whether tthe specified pageComponent is null.
		/// </summary>
		/// <returns><c>true</c> if the specified pageComponent is null; otherwise, <c>false</c>.</returns>
		/// <param name="pageComponent">Page component.</param>
		protected bool IsNullComponent(ScrollRectPage pageComponent)
		{
			return pageComponent==null;
		}

		/// <summary>
		/// Updates the page buttons.
		/// </summary>
		protected virtual void UpdatePageButtons()
		{
			if (SRDefaultPage==null)
			{
				return ;
			}

			DefaultPages.RemoveAll(IsNullComponent);

			if (DefaultPages.Count==Pages)
			{
				return ;
			}

			if (DefaultPages.Count < Pages)
			{
				DefaultPagesCache.RemoveAll(IsNullComponent);

				Enumerable.Range(DefaultPages.Count, Pages - DefaultPages.Count).ForEach(AddComponent);

				if (SRNextPage!=null)
				{
					SRNextPage.SetPage(Pages - 1);
					SRNextPage.transform.SetAsLastSibling();
				}
			}
			else
			{
				var to_cache = DefaultPages.GetRange(Pages, DefaultPages.Count - Pages);//.OrderByDescending<ScrollRectPage,int>(GetPageNumber);

				to_cache.ForEach(x => x.gameObject.SetActive(false));
				DefaultPagesCache.AddRange(to_cache);
				DefaultPages.RemoveRange(Pages, DefaultPages.Count - Pages);

				if (SRNextPage!=null)
				{
					SRNextPage.SetPage(Pages - 1);
				}
			}

			Utilites.UpdateLayout(DefaultPage.parent.GetComponent<LayoutGroup>());
		}

		/// <summary>
		/// Adds page the component.
		/// </summary>
		/// <param name="page">Page.</param>
		protected virtual void AddComponent(int page)
		{
			ScrollRectPage component;
			if (DefaultPagesCache.Count > 0)
			{
				component = DefaultPagesCache[DefaultPagesCache.Count - 1];
				DefaultPagesCache.RemoveAt(DefaultPagesCache.Count - 1);
			}
			else
			{
				component = Instantiate(SRDefaultPage) as ScrollRectPage;
				component.transform.SetParent(SRDefaultPage.transform.parent, false);

				component.OnPageSelect.AddListener(GoToPage);

				Utilites.FixInstantiated(SRDefaultPage, component);
			}
			component.transform.SetAsLastSibling();
			component.gameObject.SetActive(true);
			component.SetPage(page);

			DefaultPages.Add(component);
		}

		/// <summary>
		/// Gets the page number.
		/// </summary>
		/// <returns>The page number.</returns>
		/// <param name="pageComponent">Page component.</param>
		protected int GetPageNumber(ScrollRectPage pageComponent)
		{
			return pageComponent.Page;
		}

		/// <summary>
		/// Determines whether direction is horizontal.
		/// </summary>
		/// <returns><c>true</c> if this instance is horizontal; otherwise, <c>false</c>.</returns>
		protected bool IsHorizontal()
		{
			if (Direction==PaginatorDirection.Horizontal)
			{
				return true;
			}
			if (Direction==PaginatorDirection.Vertical)
			{
				return false;
			}

			var rect = ScrollRect.content.rect;
			return rect.width >= rect.height;
		}

		/// <summary>
		/// Gets the size of the page.
		/// </summary>
		/// <returns>The page size.</returns>
		protected virtual float GetPageSize()
		{
			if (PageSizeType==PageSizeType.Fixed)
			{
				return PageSize + PageSpacing;
			}
			if (IsHorizontal())
			{
				return (ScrollRect.transform as RectTransform).rect.width + PageSpacing;
			}
			else
			{
				return (ScrollRect.transform as RectTransform).rect.height + PageSpacing;
			}
		}

		/// <summary>
		/// Go to next page.
		/// </summary>
		/// <param name="x">Unused.</param>
		void Next(int x)
		{
			Next();
		}

		/// <summary>
		/// Go to previous page.
		/// </summary>
		/// <param name="x">Unused.</param>
		void Prev(int x)
		{
			Prev();
		}


		/// <summary>
		/// Go to next page.
		/// </summary>
		public virtual void Next()
		{
			if (CurrentPage==(Pages - 1))
			{
				return ;
			}
			CurrentPage += 1;
		}

		/// <summary>
		/// Go to previous page.
		/// </summary>
		public virtual void Prev()
		{
			if (CurrentPage==0)
			{
				return ;
			}
			CurrentPage -= 1;
		}

		protected virtual bool IsValidDrag(PointerEventData eventData)
		{
			if (!gameObject.activeInHierarchy)
			{
				return false;
			}
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return false;
			}

			if (!ScrollRect.IsActive())
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Happens when ScrollRect OnDragStart event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void OnScrollRectDragStart(PointerEventData eventData)
		{
			if (!IsValidDrag(eventData))
			{
				return ;
			}

			DragDelta = Vector2.zero;

			isDragging = true;

			CursorStartPosition = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(ScrollRect.transform as RectTransform, eventData.position, eventData.pressEventCamera, out CursorStartPosition);

			DragStarted = Time.unscaledTime;

			if (isAnimationRunning)
			{
				isAnimationRunning = false;
				if (currentAnimation!=null)
				{
					StopCoroutine(currentAnimation);
				}
			}
		}

		/// <summary>
		/// The drag delta.
		/// </summary>
		protected Vector2 DragDelta = Vector2.zero;

		/// <summary>
		/// Time when drag started.
		/// </summary>
		protected float DragStarted = 0f;

		/// <summary>
		/// Happens when ScrollRect OnDrag event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void OnScrollRectDrag(PointerEventData eventData)
		{
			if (!IsValidDrag(eventData))
			{
				return ;
			}

			Vector2 current_cursor;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(ScrollRect.transform as RectTransform, eventData.position, eventData.pressEventCamera, out current_cursor))
			{
				return ;
			}

			DragDelta = current_cursor - CursorStartPosition;
		}

		/// <summary>
		/// Happens when ScrollRect OnDragEnd event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void OnScrollRectDragEnd(PointerEventData eventData)
		{
			if (!IsValidDrag(eventData))
			{
				return ;
			}

			isDragging = false;
			if (ForceScrollOnPage)
			{
				ScrollChanged();
			}
		}

		/// <summary>
		/// Happens when ScrollRect onValueChanged event occurs.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void OnScrollRectValueChanged(Vector2 value)
		{
			if (isAnimationRunning || !gameObject.activeInHierarchy || isDragging)
			{
				return ;
			}
			if (ForceScrollOnPage)
			{
				//ScrollChanged();
			}
		}

		/// <summary>
		/// Handle scroll changes.
		/// </summary>
		protected virtual void ScrollChanged()
		{
			if (!gameObject.activeInHierarchy)
			{
				return ;
			}

			var distance = Mathf.Abs(IsHorizontal() ? DragDelta.x : DragDelta.y);
			var time = Time.unscaledTime - DragStarted;

			var is_fast = (distance >= FastDragDistance) && (time <= FastDragTime);
			if (!is_fast)
			{
				var pos = IsHorizontal() ? -ScrollRect.content.anchoredPosition.x : ScrollRect.content.anchoredPosition.y;
				var page = Mathf.RoundToInt(pos / GetPageSize());
				GoToPage(page, true);

				DragDelta = Vector2.zero;
				DragStarted = 0f;
			}
			else
			{
				var direction = IsHorizontal() ? DragDelta.x : -DragDelta.y;
				DragDelta = Vector2.zero;
				if (direction==0f)
				{
					return ;
				}
				var page = direction < 0 ? CurrentPage + 1 : CurrentPage - 1;
				GoToPage(page, false);
			}
		}

		/// <summary>
		/// Gets the size of the content.
		/// </summary>
		/// <returns>The content size.</returns>
		protected virtual float GetContentSize()
		{
			return IsHorizontal() ? ScrollRect.content.rect.width : ScrollRect.content.rect.height;
		}

		/// <summary>
		/// Recalculate the pages count.
		/// </summary>
		protected virtual void RecalculatePages()
		{
			SetScrollRectMaxDrag();
			
			Pages = Mathf.Max(1, Mathf.CeilToInt(GetContentSize() / GetPageSize()));
		}

		protected virtual void SetScrollRectMaxDrag()
		{
			var scrollRectDrag = ScrollRect as ScrollRectRestrictedDrag;
			if (scrollRectDrag!=null)
			{
				if (IsHorizontal())
				{
					scrollRectDrag.MaxDrag.x = GetPageSize();
					scrollRectDrag.MaxDrag.y = 0;
				}
				else
				{
					scrollRectDrag.MaxDrag.x = 0;
					scrollRectDrag.MaxDrag.y = GetPageSize();
				}
			}
		}

		/// <summary>
		/// Go to page.
		/// </summary>
		/// <param name="page">Page.</param>
		protected virtual void GoToPage(int page)
		{
			GoToPage(page, false);
		}

		/// <summary>
		/// Gets the page position.
		/// </summary>
		/// <returns>The page position.</returns>
		/// <param name="page">Page.</param>
		protected virtual float GetPagePosition(int page)
		{
			return page * GetPageSize();
		}

		/// <summary>
		/// Go to page.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="forceUpdate">If set to <c>true</c> force update.</param>
		protected virtual void GoToPage(int page, bool forceUpdate)
		{
			page = Mathf.Clamp(page, 0, Pages - 1);
			if ((currentPage==page) && (!forceUpdate))
			{
				return ;
			}

			if (isAnimationRunning)
			{
				isAnimationRunning = false;
				if (currentAnimation!=null)
				{
					StopCoroutine(currentAnimation);
				}
			}

			var endPosition = GetPagePosition(page);
			if (IsHorizontal())
			{
				endPosition *= -1;
			}
			if (Animation)
			{
				isAnimationRunning = true;
				var startPosition = IsHorizontal() ? ScrollRect.content.anchoredPosition.x : ScrollRect.content.anchoredPosition.y;
				currentAnimation = RunAnimation(IsHorizontal(), startPosition, endPosition, UnscaledTime);
				StartCoroutine(currentAnimation);
			}
			else
			{
				if (IsHorizontal())
				{
					ScrollRect.content.anchoredPosition = new Vector2(endPosition, ScrollRect.content.anchoredPosition.y);
				}
				else
				{
					ScrollRect.content.anchoredPosition = new Vector2(ScrollRect.content.anchoredPosition.x, endPosition);
				}
			}

			if ((SRDefaultPage!=null) && (currentPage!=page))
			{
				if (currentPage >= 0)
				{
					DefaultPages[currentPage].gameObject.SetActive(true);
				}
				DefaultPages[page].gameObject.SetActive(false);
				SRActivePage.SetPage(page);
				SRActivePage.transform.SetSiblingIndex(DefaultPages[page].transform.GetSiblingIndex());
			}
			if (SRPrevPage!=null)
			{
				SRPrevPage.gameObject.SetActive(page!=0);
			}
			if (SRNextPage!=null)
			{
				SRNextPage.gameObject.SetActive(page!=(Pages - 1));
			}

			currentPage = page;

			OnPageSelect.Invoke(currentPage);
		}

		/// <summary>
		/// Runs the animation.
		/// </summary>
		/// <returns>The animation.</returns>
		/// <param name="isHorizontal">If set to <c>true</c> is horizontal.</param>
		/// <param name="startPosition">Start position.</param>
		/// <param name="endPosition">End position.</param>
		/// <param name="unscaledTime">If set to <c>true</c> use unscaled time.</param>
		protected virtual IEnumerator RunAnimation(bool isHorizontal, float startPosition, float endPosition, bool unscaledTime)
		{
			float delta;

			var animationLength = Movement.keys[Movement.keys.Length - 1].time;
			var startTime = (unscaledTime ? Time.unscaledTime : Time.time);
			do
			{
				delta = ((unscaledTime ? Time.unscaledTime : Time.time) - startTime);
				var value = Movement.Evaluate(delta);

				var position = startPosition + ((endPosition - startPosition) * value);
				if (isHorizontal)
				{
					ScrollRect.content.anchoredPosition = new Vector2(position, ScrollRect.content.anchoredPosition.y);
				}
				else
				{
					ScrollRect.content.anchoredPosition = new Vector2(ScrollRect.content.anchoredPosition.x, position);
				}
				yield return null;
			}
			while (delta < animationLength);

			if (isHorizontal)
			{
				ScrollRect.content.anchoredPosition = new Vector2(endPosition, ScrollRect.content.anchoredPosition.y);
			}
			else
			{
				ScrollRect.content.anchoredPosition = new Vector2(ScrollRect.content.anchoredPosition.x, endPosition);
			}
			isAnimationRunning = false;
		}

		/// <summary>
		/// Removes the callback.
		/// </summary>
		/// <param name="page">Page.</param>
		protected virtual void RemoveCallback(ScrollRectPage page)
		{
			page.OnPageSelect.RemoveListener(GoToPage);
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			DefaultPages.RemoveAll(IsNullComponent);
			DefaultPages.ForEach(RemoveCallback);

			DefaultPagesCache.RemoveAll(IsNullComponent);
			DefaultPagesCache.ForEach(RemoveCallback);

			if (ScrollRect!=null)
			{
				var dragListener = ScrollRect.GetComponent<OnDragListener>();
				if (dragListener!=null)
				{
					dragListener.OnDragStartEvent.RemoveListener(OnScrollRectDragStart);
					dragListener.OnDragEvent.RemoveListener(OnScrollRectDrag);
					dragListener.OnDragEndEvent.RemoveListener(OnScrollRectDragEnd);
				}

				var resizeListener = ScrollRect.GetComponent<ResizeListener>();
				if (resizeListener!=null)
				{
					resizeListener.OnResize.RemoveListener(RecalculatePages);
				}

				if (ScrollRect.content!=null)
				{
					var contentResizeListener = ScrollRect.content.GetComponent<ResizeListener>();
					if (contentResizeListener!=null)
					{
						contentResizeListener.OnResize.RemoveListener(RecalculatePages);
					}
				}

				ScrollRect.onValueChanged.RemoveListener(OnScrollRectValueChanged);
			}

			if (SRPrevPage!=null)
			{
				SRPrevPage.OnPageSelect.RemoveListener(Prev);
			}
			if (SRNextPage!=null)
			{
				SRNextPage.OnPageSelect.RemoveListener(Next);
			}
		}
	}
}