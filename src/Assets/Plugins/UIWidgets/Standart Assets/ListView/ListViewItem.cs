using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace UIWidgets {
	/// <summary>
	/// List view item resize event.
	/// </summary>
	[Serializable]
	public class ListViewItemResize : UnityEvent<int,Vector2>
	{
		
	}

	/// <summary>
	/// List view item select event.
	/// </summary>
	[Serializable]
	public class ListViewItemSelect : UnityEvent<ListViewItem>
	{
		
	}

	/// <summary>
	/// List view item pointer move event.
	/// </summary>
	[Serializable]
	public class ListViewItemMove : UnityEvent<AxisEventData, ListViewItem>
	{

	}

	/// <summary>
	/// List view item click event.
	/// </summary>
	[Serializable]
	public class ListViewItemClick : UnityEvent<int>
	{

	}

	/// <summary>
	/// ListViewItem.
	/// Item for ListViewBase.
	/// </summary>
	[RequireComponent(typeof(Image))]
	public class ListViewItem : UIBehaviour,
		IPointerClickHandler,
		IPointerEnterHandler, IPointerExitHandler,
		ISubmitHandler, ICancelHandler,
		ISelectHandler, IDeselectHandler,
		IMoveHandler, IComparable<ListViewItem>
	{
		/// <summary>
		/// The index of item in ListView.
		/// </summary>
		[HideInInspector]
		public int Index = -1;

		/// <summary>
		/// What to do when the event system send a pointer click event.
		/// </summary>
		public UnityEvent onClick = new UnityEvent();

		/// <summary>
		/// What to do when the event system send a submit event.
		/// </summary>
		public ListViewItemSelect onSubmit = new ListViewItemSelect();

		/// <summary>
		/// What to do when the event system send a cancel event.
		/// </summary>
		public ListViewItemSelect onCancel = new ListViewItemSelect();

		/// <summary>
		/// What to do when the event system send a select event.
		/// </summary>
		public ListViewItemSelect onSelect = new ListViewItemSelect();

		/// <summary>
		/// What to do when the event system send a deselect event.
		/// </summary>
		public ListViewItemSelect onDeselect = new ListViewItemSelect();

		/// <summary>
		/// What to do when the event system send a move event.
		/// </summary>
		public ListViewItemMove onMove = new ListViewItemMove();

		/// <summary>
		/// What to do when the event system send a pointer click event.
		/// </summary>
		public PointerUnityEvent onPointerClick = new PointerUnityEvent();

		/// <summary>
		/// What to do when the event system send a pointer enter Event.
		/// </summary>
		public PointerUnityEvent onPointerEnter = new PointerUnityEvent();

		/// <summary>
		/// What to do when the event system send a pointer exit Event.
		/// </summary>
		public PointerUnityEvent onPointerExit = new PointerUnityEvent();

		/// <summary>
		/// OnResize event.
		/// </summary>
		public ListViewItemResize onResize = new ListViewItemResize();

		/// <summary>
		/// OnDoubleClick event.
		/// </summary>
		public ListViewItemClick onDoubleClick = new ListViewItemClick();

		Image background;

		/// <summary>
		/// The background.
		/// </summary>
		public Image Background {
			get {
				if (background==null)
				{
					background = GetComponent<Image>();
				}
				return background;
			}
		}

		RectTransform rectTransform;

		/// <summary>
		/// Gets the RectTransform.
		/// </summary>
		/// <value>The RectTransform.</value>
		protected RectTransform RectTransform {
			get {
				if (rectTransform==null)
				{
					rectTransform = transform as RectTransform;
				}
				return rectTransform;
			}
		}

		[SerializeField]
		protected bool LocalPositionZReset;

		/// <summary>
		/// Awake this instance.
		/// </summary>
		protected override void Awake()
		{
			if ((LocalPositionZReset) && (transform.localPosition.z!=0f))
			{
				var pos = transform.localPosition;
				pos.z = 0f;
				transform.localPosition = pos;
			}
		}

		/// <summary>
		/// Raises the move event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnMove(AxisEventData eventData)
		{
			onMove.Invoke(eventData, this);
		}

		/// <summary>
		/// Raises the submit event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnSubmit(BaseEventData eventData)
		{
			onSubmit.Invoke(this);
		}

		/// <summary>
		/// Raises the cancel event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnCancel(BaseEventData eventData)
		{
			onCancel.Invoke(this);
		}

		/// <summary>
		/// Raises the select event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnSelect(BaseEventData eventData)
		{
			Select();
			onSelect.Invoke(this);
		}

		/// <summary>
		/// Raises the deselect event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnDeselect(BaseEventData eventData)
		{
			onDeselect.Invoke(this);
		}

		/// <summary>
		/// Raises the pointer click event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if ((eventData.clickCount==2) && (eventData.button==PointerEventData.InputButton.Left))
			{
				eventData.clickCount = 0;

				onDoubleClick.Invoke(Index);
			}

			onPointerClick.Invoke(eventData);

			if (eventData.button!=PointerEventData.InputButton.Left)
			{
				return;
			}

			onClick.Invoke();
			Select();
		}

		/// <summary>
		/// Raises the pointer enter event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			onPointerEnter.Invoke(eventData);
		}
		
		/// <summary>
		/// Raises the pointer exit event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			onPointerExit.Invoke(eventData);
		}

		/// <summary>
		/// Select this instance.
		/// </summary>
		public virtual void Select()
		{
			if (EventSystem.current.alreadySelecting)
			{
				return;
			}

			var ev = new ListViewItemEventData(EventSystem.current) {
				NewSelectedObject = gameObject
			};
			EventSystem.current.SetSelectedGameObject(ev.NewSelectedObject, ev);
		}

		Rect oldRect;

		/// <summary>
		/// Implementation of a callback that is sent if an associated RectTransform has it's dimensions changed..
		/// </summary>
		protected override void OnRectTransformDimensionsChange()
		{
			if (oldRect.Equals(RectTransform.rect))
			{
				return ;
			}
			oldRect = RectTransform.rect;
			onResize.Invoke(Index, oldRect.size);
		}
		
		/// <summary>
		/// Compares the current object with another object of the same type by Index.
		/// </summary>
		/// <returns>Another object.</returns>
		/// <param name="compareItem">Compare item.</param>
		public int CompareTo(ListViewItem compareItem)
		{
			return (compareItem == null) ? 1 : Index.CompareTo(compareItem.Index);
		}

		/// <summary>
		/// Called when item moved to cache, you can use it free used resources.
		/// </summary>
		public virtual void MovedToCache()
		{

		}
	}
}