using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace UIWidgets
{
	/// <summary>
	/// Resizable event.
	/// </summary>
	[Serializable]
	public class ResizableEvent : UnityEvent<Resizable>
	{

	}

	/// <summary>
	/// Resizable.
	/// N - north (top).
	/// S - south (bottom).
	/// E - east (right).
	/// W - west (left).
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/Resizable")]
	public class Resizable : MonoBehaviour,
		IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,
		IPointerEnterHandler, IPointerExitHandler
	{
		/// <summary>
		/// Resize directions.
		/// </summary>
		[Serializable]
		public struct Directions {
			/// <summary>
			/// Allow resize with top side.
			/// </summary>
			public bool Top;

			/// <summary>
			/// Allow resize with bottom side.
			/// </summary>
			public bool Bottom;

			/// <summary>
			/// Allow resize with left side.
			/// </summary>
			public bool Left;

			/// <summary>
			/// Allow resize with right side.
			/// </summary>
			public bool Right;

			/// <summary>
			/// Allow resize with top left corner.
			/// </summary>
			public bool TopLeft;

			/// <summary>
			/// Allow resize with top right corner.
			/// </summary>
			public bool TopRight;

			/// <summary>
			/// Allow resize with bottom left corner.
			/// </summary>
			public bool BottomLeft;

			/// <summary>
			/// Allow resize with bottom right corner.
			/// </summary>
			public bool BottomRight;

			/// <summary>
			/// Initializes a new instance of the <see cref="UIWidgets.Resizable+Directions"/> struct.
			/// </summary>
			/// <param name="top">If set to <c>true</c> allow resize from top.</param>
			/// <param name="bottom">If set to <c>true</c> allow resize from bottom.</param>
			/// <param name="left">If set to <c>true</c> allow resize from left.</param>
			/// <param name="right">If set to <c>true</c> allow resize from right.</param>
			/// <param name="topLeft">If set to <c>true</c> allow resize from top left corner.</param>
			/// <param name="topRight">If set to <c>true</c> allow resize from top right corner.</param>
			/// <param name="bottomLeft">If set to <c>true</c> allow resize from bottom left corner.</param>
			/// <param name="bottomRight">If set to <c>true</c> allow resize from bottom right corner.</param>
			public Directions(bool top, bool bottom, bool left, bool right, bool topLeft=true, bool topRight=true, bool bottomLeft=true, bool bottomRight=true)
			{
				Top = top;
				Bottom = bottom;
				Left = left;
				Right = right;

				TopLeft = topLeft;
				TopRight = topRight;

				BottomLeft = bottomLeft;
				BottomRight = bottomRight;
			}

			/// <summary>
			/// Gets a value indicating whether any direction is allowed.
			/// </summary>
			/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
			public bool Active	{
				get {
					return Top || Bottom || Left || Right;
				}
			}

			/// <summary>
			/// NWSE
			/// </summary>
			/// <value><c>true</c> if allowed direction is NWSE; otherwise, <c>false</c>.</value>
			public bool NWSE {
				get {
					return TopLeft || BottomRight;
				}
			}

			/// <summary>
			/// NESW.
			/// </summary>
			/// <value><c>true</c> if allowed direction is NESW; otherwise, <c>false</c>.</value>
			public bool NESW {
				get {
					return TopRight || BottomLeft;
				}
			}

			/// <summary>
			/// NS
			/// </summary>
			/// <value><c>true</c> if allowed direction is NS; otherwise, <c>false</c>.</value>
			public bool NS {
				get {
					return Top || Bottom;
				}
			}

			/// <summary>
			/// EW.
			/// </summary>
			/// <value><c>true</c> if allowed direction is EW; otherwise, <c>false</c>.</value>
			public bool EW {
				get {
					return Left || Right;
				}
			}
		}

		/// <summary>
		/// Active resize region.
		/// </summary>
		public struct Regions {
			/// <summary>
			/// The top.
			/// </summary>
			public bool Top;

			/// <summary>
			/// The bottom.
			/// </summary>
			public bool Bottom;

			/// <summary>
			/// The left.
			/// </summary>
			public bool Left;

			/// <summary>
			/// The right.
			/// </summary>
			public bool Right;

			/// <summary>
			/// NWSE
			/// </summary>
			/// <value><c>true</c> if cursor mode is NWSE; otherwise, <c>false</c>.</value>
			public bool NWSE {
				get {
					return (Top && Left) || (Bottom && Right);
				}
			}

			/// <summary>
			/// NESW.
			/// </summary>
			/// <value><c>true</c> if cursor mode is NESW; otherwise, <c>false</c>.</value>
			public bool NESW {
				get {
					return (Top && Right) || (Bottom && Left);
				}
			}

			/// <summary>
			/// NS
			/// </summary>
			/// <value><c>true</c> if cursor mode is NS; otherwise, <c>false</c>.</value>
			public bool NS {
				get {
					return (Top && !Right) || (Bottom && !Left);
				}
			}

			/// <summary>
			/// EW.
			/// </summary>
			/// <value><c>true</c> if cursor mode is EW; otherwise, <c>false</c>.</value>
			public bool EW {
				get {
					return (!Top && Right) || (!Bottom && Left);
				}
			}

			/// <summary>
			/// Is any region active.
			/// </summary>
			/// <value><c>true</c> if any region active; otherwise, <c>false</c>.</value>
			public bool Active {
				get {
					return Top || Bottom || Left || Right;
				}
			}

			/// <summary>
			/// Reset this instance.
			/// </summary>
			public void Reset()
			{
				Top = false;
				Bottom = false;
				Left = false;
				Right = false;
			}

			/// <summary>
			/// Returns a string that represents the current object.
			/// </summary>
			/// <returns>A string that represents the current object.</returns>
			public override string ToString()
			{
				return String.Format("Top: {0}; Bottom: {1}; Left: {2}; Right: {3}", Top, Bottom, Left, Right);
			}
		}

		/// <summary>
		/// Is need to update RectTransform on Resize.
		/// </summary>
		[SerializeField]
		public bool UpdateRectTransform = true;

		/// <summary>
		/// Is need to update LayoutElement on Resize.
		/// </summary>
		[SerializeField]
		public bool UpdateLayoutElement = true;

		/// <summary>
		/// The active region in points from left or right border where resize allowed.
		/// </summary>
		[SerializeField]
		[Tooltip("Maximum padding from border where resize active.")]
		public float ActiveRegion = 5;

		/// <summary>
		/// The minimum size.
		/// </summary>
		[SerializeField]
		public Vector2 MinSize;
		
		/// <summary>
		/// The maximum size.
		/// </summary>
		[SerializeField]
		[Tooltip("Set 0 to unlimit.")]
		public Vector2 MaxSize;

		/// <summary>
		/// The keep aspect ratio.
		/// Aspect ratio applied after MinSize and MaxSize, so if RectTransform aspect ratio not equal MinSize and MaxSize aspect ratio then real size may be outside limit with one of axis.
		/// </summary>
		[SerializeField]
		public bool KeepAspectRatio;

		/// <summary>
		/// Resize directions.
		/// </summary>
		[SerializeField]
		public Directions ResizeDirections = new Directions(true, true, true, true);

		/// <summary>
		/// The current camera. For Screen Space - Overlay let it empty.
		/// </summary>
		[SerializeField]
		public Camera CurrentCamera;
		
		/// <summary>
		/// The cursor EW texture.
		/// </summary>
		[SerializeField]
		public Texture2D CursorEWTexture;
		
		/// <summary>
		/// The cursor EW hot spot.
		/// </summary>
		[SerializeField]
		public Vector2 CursorEWHotSpot = new Vector2(16, 16);

		/// <summary>
		/// The cursor NS texture.
		/// </summary>
		[SerializeField]
		public Texture2D CursorNSTexture;
		
		/// <summary>
		/// The cursor NS hot spot.
		/// </summary>
		[SerializeField]
		public Vector2 CursorNSHotSpot = new Vector2(16, 16);
		
		/// <summary>
		/// The cursor NESW texture.
		/// </summary>
		[SerializeField]
		public Texture2D CursorNESWTexture;
		
		/// <summary>
		/// The cursor NESW hot spot.
		/// </summary>
		[SerializeField]
		public Vector2 CursorNESWHotSpot = new Vector2(16, 16);

		/// <summary>
		/// The cursor NWSE texture.
		/// </summary>
		[SerializeField]
		public Texture2D CursorNWSETexture;
		
		/// <summary>
		/// The cursor NWSE hot spot.
		/// </summary>
		[SerializeField]
		public Vector2 CursorNWSEHotSpot = new Vector2(16, 16);

		/// <summary>
		/// The default cursor texture.
		/// </summary>
		[SerializeField]
		public Texture2D DefaultCursorTexture;
		
		/// <summary>
		/// The default cursor hot spot.
		/// </summary>
		[SerializeField]
		public Vector2 DefaultCursorHotSpot;

		/// <summary>
		/// OnStartResize event.
		/// </summary>
		public ResizableEvent OnStartResize = new ResizableEvent();

		/// <summary>
		/// OnEndResize event.
		/// </summary>
		public ResizableEvent OnEndResize = new ResizableEvent();

		RectTransform rectTransform;

		/// <summary>
		/// Gets the RectTransform.
		/// </summary>
		/// <value>RectTransform.</value>
		public RectTransform RectTransform {
			get {
				if (rectTransform==null)
				{
					rectTransform = transform as RectTransform;
				}
				return rectTransform;
			}
		}

		LayoutElement layoutElement;

		/// <summary>
		/// Gets the LayoutElement.
		/// </summary>
		/// <value>LayoutElement.</value>
		public LayoutElement LayoutElement {
			get {
				if (layoutElement==null)
				{
					layoutElement = GetComponent<LayoutElement>();
					if (layoutElement==null)
					{
						layoutElement = gameObject.AddComponent<LayoutElement>();
					}
				}
				return layoutElement;
			}
		}

		/// <summary>
		/// Current drag regions.
		/// </summary>
		protected Regions regions;

		/// <summary>
		/// Processed drag regions.
		/// </summary>
		protected Regions dragRegions;

		/// <summary>
		/// The canvas.
		/// </summary>
		protected Canvas canvas;

		/// <summary>
		/// The canvas rect.
		/// </summary>
		protected RectTransform canvasRect;

		/// <summary>
		/// Allow to process drag.
		/// </summary>
		protected bool processDrag;

		void Start()
		{
			var layout = GetComponent<HorizontalOrVerticalLayoutGroup>();
			if (layout)
			{
				Utilites.UpdateLayout(layout);
			}
			
			Init();
		}
		
		/// <summary>
		/// Raises the initialize potential drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnInitializePotentialDrag(PointerEventData eventData)
		{
			Init();
		}
		
		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init()
		{
			canvasRect = Utilites.FindTopmostCanvas(transform) as RectTransform;
			canvas = canvasRect.GetComponent<Canvas>();
		}

		/// <summary>
		/// The global cursor changed.
		/// </summary>
		protected static bool globalCursorSetted;

		/// <summary>
		/// The cursor changed.
		/// </summary>
		protected bool cursorChanged;

		/// <summary>
		/// Is cursor over.
		/// </summary>
		protected bool IsCursorOver;

		/// <summary>
		/// Called by a BaseInputModule when an OnPointerEnter event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerEnter(PointerEventData eventData)
		{
			IsCursorOver = true;
		}

		/// <summary>
		/// Called by a BaseInputModule when an OnPointerExit event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerExit(PointerEventData eventData)
		{
			IsCursorOver = false;

			if (!processDrag)
			{
				ResetCursor();
			}
		}

		void LateUpdate()
		{
			if (globalCursorSetted && !cursorChanged)
			{
				return ;
			}
			globalCursorSetted = false;
			if (!IsCursorOver)
			{
				return ;
			}

			if (processDrag)
			{
				return ;
			}
			if (!Input.mousePresent)
			{
				return ;
			}
			
			Vector2 point;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, Input.mousePosition, CurrentCamera, out point))
			{
				return ;
			}
			var r = RectTransform.rect;
			if (!r.Contains(point))
			{
				regions.Reset();
				UpdateCursor();
				return ;
			}

			UpdateRegions(point);

			UpdateCursor();
		}

		void UpdateRegions(Vector2 point)
		{
			regions.Top = CheckTop(point);
			regions.Bottom = CheckBottom(point);
			regions.Left = CheckLeft(point);
			regions.Right = CheckRight(point);
		}

		/// <summary>
		/// Updates the cursor.
		/// </summary>
		protected virtual void UpdateCursor()
		{
			if (regions.NWSE && ResizeDirections.NWSE)
			{
				globalCursorSetted = true;
				cursorChanged = true;
				Cursor.SetCursor(CursorNWSETexture, CursorNWSEHotSpot, Utilites.GetCursorMode());
			}
			else if (regions.NESW && ResizeDirections.NESW)
			{
				globalCursorSetted = true;
				cursorChanged = true;
				Cursor.SetCursor(CursorNESWTexture, CursorNESWHotSpot, Utilites.GetCursorMode());
			}
			else if (regions.NS && ResizeDirections.NS)
			{
				globalCursorSetted = true;
				cursorChanged = true;
				Cursor.SetCursor(CursorNSTexture, CursorNSHotSpot, Utilites.GetCursorMode());
			}
			else if (regions.EW && ResizeDirections.EW)
			{
				globalCursorSetted = true;
				cursorChanged = true;
				Cursor.SetCursor(CursorEWTexture, CursorEWHotSpot, Utilites.GetCursorMode());
			}
			else if (cursorChanged)
			{
				ResetCursor();
			}
		}

		/// <summary>
		/// Checks if point in the top region.
		/// </summary>
		/// <returns><c>true</c>, if point in the top region, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		bool CheckTop(Vector2 point)
		{
			var rect = RectTransform.rect;

			rect.position = new Vector2(rect.position.x, rect.position.y + rect.height - ActiveRegion);
			rect.height = ActiveRegion;

			return rect.Contains(point);
		}

		/// <summary>
		/// Checks if point in the right region.
		/// </summary>
		/// <returns><c>true</c>, if right was checked, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		bool CheckBottom(Vector2 point)
		{
			var rect = RectTransform.rect;
			rect.height = ActiveRegion;

			return rect.Contains(point);
		}

		/// <summary>
		/// Checks if point in the left region.
		/// </summary>
		/// <returns><c>true</c>, if point in the left region, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		bool CheckLeft(Vector2 point)
		{
			var rect = RectTransform.rect;
			rect.width = ActiveRegion;

			return rect.Contains(point);
		}
		
		/// <summary>
		/// Checks if point in the right region.
		/// </summary>
		/// <returns><c>true</c>, if right was checked, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		bool CheckRight(Vector2 point)
		{
			var rect = RectTransform.rect;
			
			rect.position = new Vector2(rect.position.x + rect.width - ActiveRegion, rect.position.y);
			rect.width = ActiveRegion;

			return rect.Contains(point);
		}

		/// <summary>
		/// Raises the begin drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			Vector2 point;
			processDrag = false;
			
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.pressPosition, eventData.pressEventCamera, out point))
			{
				return ;
			}
			
			UpdateRegions(point);

			processDrag = IsAllowedDrag();

			dragRegions = regions;

			UpdateCursor();

			LayoutElement.preferredHeight = RectTransform.rect.height;
			LayoutElement.preferredWidth = RectTransform.rect.width;

			OnStartResize.Invoke(this);
		}

		/// <summary>
		/// Determines whether drag allowed with current active regions and specified directions.
		/// </summary>
		/// <returns><c>true</c> if this instance is allowed drag; otherwise, <c>false</c>.</returns>
		protected virtual bool IsAllowedDrag()
		{
			return (regions.Top && ResizeDirections.Top)
				|| (regions.Bottom && ResizeDirections.Bottom)
				|| (regions.Left && ResizeDirections.Left)
				|| (regions.Right && ResizeDirections.Right)

				|| (regions.NESW && ResizeDirections.NESW)
				|| (regions.NWSE && ResizeDirections.NWSE)
				|| (regions.NS && ResizeDirections.NS)
				|| (regions.EW && ResizeDirections.EW);
		}

		/// <summary>
		/// Determines whether this instance is allowed resize horizontal.
		/// </summary>
		/// <returns><c>true</c> if this instance is allowed resize horizontal; otherwise, <c>false</c>.</returns>
		protected virtual bool IsAllowedResizeHorizontal()
		{
			return (dragRegions.Left && ResizeDirections.Left)
				|| (dragRegions.Right && ResizeDirections.Right)

				|| (dragRegions.NESW && ResizeDirections.NESW)
				|| (dragRegions.NWSE && ResizeDirections.NWSE)
				|| (dragRegions.EW && ResizeDirections.EW);
		}

		/// <summary>
		/// Determines whether this instance is allowed resize vertical.
		/// </summary>
		/// <returns><c>true</c> if this instance is allowed resize vertical; otherwise, <c>false</c>.</returns>
		protected virtual bool IsAllowedResizeVertical()
		{
			return (dragRegions.Top && ResizeDirections.Top)
				|| (dragRegions.Bottom && ResizeDirections.Bottom)

				|| (dragRegions.NESW && ResizeDirections.NESW)
				|| (dragRegions.NWSE && ResizeDirections.NWSE)
				|| (dragRegions.NS && ResizeDirections.NS);
		}

		void ResetCursor()
		{
			globalCursorSetted = false;
			cursorChanged = false;

			Cursor.SetCursor(DefaultCursorTexture, DefaultCursorHotSpot, Utilites.GetCursorMode());
		}

		/// <summary>
		/// Raises the end drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnEndDrag(PointerEventData eventData)
		{
			ResetCursor();

			processDrag = false;

			OnEndResize.Invoke(this);
		}
		
		/// <summary>
		/// Raises the drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!processDrag)
			{
				return ;
			}
			if (canvas==null)
			{
				throw new MissingComponentException(gameObject.name + " not in Canvas hierarchy.");
			}

			Vector2 p1;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, CurrentCamera, out p1);
			Vector2 p2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position - eventData.delta, CurrentCamera, out p2);
			var delta = p1 - p2;

			if (UpdateRectTransform)
			{
				PerformUpdateRectTransform(delta);
			}
			if (UpdateLayoutElement)
			{
				PerformUpdateLayoutElement(delta);
			}
		}

		void PerformUpdateRectTransform(Vector2 delta)
		{
			var pivot = RectTransform.pivot;
			var size = RectTransform.rect.size;
			var prev_size = size;
			var sign = new Vector2(1, 1);
			if (IsAllowedResizeHorizontal())
			{
				sign.x = dragRegions.Right ? +1 : -1;
				size.x = Mathf.Max(MinSize.x, size.x + (sign.x * delta.x));
				if (MaxSize.x!=0f)
				{
					size.x = Mathf.Min(MaxSize.x, size.x);
				}
			}
			if (IsAllowedResizeVertical())
			{
				sign.y = dragRegions.Top ? +1 : -1;
				size.y = Mathf.Max(MinSize.y, size.y + (sign.y * delta.y));
				if (MaxSize.y!=0f)
				{
					size.y = Mathf.Min(MaxSize.y, size.y);
				}
			}

			if (KeepAspectRatio)
			{
				size = FixAspectRatio(size, prev_size);
			}

			var anchorSign = new Vector2(dragRegions.Right ? pivot.x : pivot.x - 1, dragRegions.Top ? pivot.y : pivot.y - 1);
			var anchorDelta = size - prev_size;
			anchorDelta = new Vector2(anchorDelta.x * anchorSign.x, anchorDelta.y * anchorSign.y);

			RectTransform.anchoredPosition += anchorDelta;
			RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
			RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
		}

		/// <summary>
		/// Fixs the aspect ratio.
		/// </summary>
		/// <returns>The aspect ratio.</returns>
		/// <param name="newSize">New size.</param>
		/// <param name="baseSize">Base size.</param>
		protected Vector2 FixAspectRatio(Vector2 newSize, Vector2 baseSize)
		{
			var result = newSize;

			var aspectRatio = baseSize.x / baseSize.y;
			var sizeDelta = new Vector2(Mathf.Abs(newSize.x - baseSize.x), Mathf.Abs(newSize.y - baseSize.y));
			if (sizeDelta.x >= sizeDelta.y)
			{
				result.y = result.x / aspectRatio;
			}
			else
			{
				result.x = result.y * aspectRatio;
			}

			return result;
		}

		void PerformUpdateLayoutElement(Vector2 delta)
		{
			var size = new Vector2(LayoutElement.preferredWidth, LayoutElement.preferredHeight);
			var prev_size = size;
			if (dragRegions.Left || dragRegions.Right)
			{
				var sign = (dragRegions.Right) ? +1 : -1;
				var width = Mathf.Max(MinSize.x, LayoutElement.preferredWidth + (sign * delta.x));
				if (MaxSize.x!=0f)
				{
					size.x = Mathf.Min(MaxSize.x, width);
				}
			}
			if (dragRegions.Top || dragRegions.Bottom)
			{
				var sign = (dragRegions.Top) ? +1 : -1;
				var height = Mathf.Max(MinSize.y, LayoutElement.preferredHeight + (sign * delta.y));
				if (MaxSize.y!=0f)
				{
					size.y = Mathf.Min(MaxSize.y, height);
				}
			}

			if (KeepAspectRatio)
			{
				size = FixAspectRatio(size, prev_size);
			}

			LayoutElement.preferredWidth = size.x;
			LayoutElement.preferredHeight = size.y;
		}
	}
}