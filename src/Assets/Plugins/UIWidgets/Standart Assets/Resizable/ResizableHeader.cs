using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

namespace UIWidgets
{
	/// <summary>
	/// IResizableItem.
	/// </summary>
	public interface IResizableItem {
		/// <summary>
		/// Gets the objects to resize.
		/// </summary>
		/// <value>The objects to resize.</value>
		GameObject[] ObjectsToResize {
			get;
		}
	}

	/// <summary>
	/// ResizableHeader.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ResizableHeader")]
	[RequireComponent(typeof(LayoutGroup))]
	public class ResizableHeader : MonoBehaviour, IDropSupport<ResizableHeaderDragCell>, IPointerEnterHandler, IPointerExitHandler
	{
		/// <summary>
		/// ListView instance.
		/// </summary>
		[SerializeField]
		public ListViewBase List;

		/// <summary>
		/// Allow resize.
		/// </summary>
		[SerializeField]
		public bool AllowResize = true;

		/// <summary>
		/// Allow reorder.
		/// </summary>
		[SerializeField]
		public bool AllowReorder = true;

		/// <summary>
		/// Is now processed cell reorder?
		/// </summary>
		[System.NonSerialized]
		[HideInInspector]
		public bool ProcessCellReorder = false;

		/// <summary>
		/// Update ListView columns width on drag.
		/// </summary>
		[SerializeField]
		public bool OnDragUpdate = true;

		/// <summary>
		/// The active region in points from left or right border where resize allowed.
		/// </summary>
		[SerializeField]
		public float ActiveRegion = 5;

		/// <summary>
		/// The current camera. For Screen Space - Overlay let it empty.
		/// </summary>
		[SerializeField]
		public Camera CurrentCamera;

		/// <summary>
		/// The cursor texture.
		/// </summary>
		[SerializeField]
		public Texture2D CursorTexture;

		/// <summary>
		/// The cursor hot spot.
		/// </summary>
		[SerializeField]
		public Vector2 CursorHotSpot = new Vector2(16, 16);

		/// <summary>
		/// The cursor texture.
		/// </summary>
		[SerializeField]
		public Texture2D AllowDropCursor;
		
		/// <summary>
		/// The cursor hot spot.
		/// </summary>
		[SerializeField]
		public Vector2 AllowDropCursorHotSpot = new Vector2(4, 2);
		
		/// <summary>
		/// The cursor texture.
		/// </summary>
		[SerializeField]
		public Texture2D DeniedDropCursor;
		
		/// <summary>
		/// The cursor hot spot.
		/// </summary>
		[SerializeField]
		public Vector2 DeniedDropCursorHotSpot = new Vector2(4, 2);

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

		RectTransform rectTransform;
		
		/// <summary>
		/// Gets the rect transform.
		/// </summary>
		/// <value>The rect transform.</value>
		public RectTransform RectTransform {
			get {
				if (rectTransform==null)
				{
					rectTransform = transform as RectTransform;
				}
				return rectTransform;
			}
		}
		
		Canvas canvas;
		RectTransform canvasRect;
		
		List<LayoutElement> childrenLayouts = new List<LayoutElement>();
		List<RectTransform> children = new List<RectTransform>();
		List<int> positions;
		LayoutElement leftTarget;
		LayoutElement rightTarget;
		bool processDrag;
		List<float> widths;

		LayoutGroup layout;

		void Start()
		{
			layout = GetComponent<LayoutGroup>();
			if (layout!=null)
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
			//Init();
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init()
		{
			canvasRect = Utilites.FindTopmostCanvas(transform) as RectTransform;
			canvas = canvasRect.GetComponent<Canvas>();
			
			children.Clear();
			childrenLayouts.Clear();
			int i = 0;
			foreach (Transform child in transform)
			{
				var element = child.GetComponent<LayoutElement>();
				if (element==null)
				{
					element = child.gameObject.AddComponent<LayoutElement>();
				}
				children.Add(child as RectTransform);
				childrenLayouts.Add(element);

				var cell = child.gameObject.AddComponent<ResizableHeaderDragCell>();
				cell.Position = i;
				cell.ResizableHeader = this;
				cell.AllowDropCursor = AllowDropCursor;
				cell.AllowDropCursorHotSpot = AllowDropCursorHotSpot;
				cell.DeniedDropCursor = DeniedDropCursor;
				cell.DeniedDropCursorHotSpot = DeniedDropCursorHotSpot;

				var events = child.gameObject.AddComponent<ResizableHeaderCell>();
				events.OnInitializePotentialDragEvent.AddListener(OnInitializePotentialDrag);
				events.OnBeginDragEvent.AddListener(OnBeginDrag);
				events.OnDragEvent.AddListener(OnDrag);
				events.OnEndDragEvent.AddListener(OnEndDrag);

				i++;
			}
			positions = Enumerable.Range(0, i).ToList();

			CalculateWidths();
			//ResetChildren();
			//Resize();
		}

		bool inActiveRegion;

		/// <summary>
		/// Gets a value indicating whether mouse position in active region.
		/// </summary>
		/// <value><c>true</c> if in active region; otherwise, <c>false</c>.</value>
		public bool InActiveRegion {
			get {
				return inActiveRegion;
			}
		}

		/// <summary>
		/// Is cursor over gameobject?
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

			cursorChanged = false;
			Cursor.SetCursor(DefaultCursorTexture, DefaultCursorHotSpot, Utilites.GetCursorMode());
		}

		/// <summary>
		/// Is cursor changed?
		/// </summary>
		protected bool cursorChanged;

		void LateUpdate()
		{
			if (!AllowResize)
			{
				return ;
			}
			if (!IsCursorOver)
			{
				return ;
			}
			if (processDrag || ProcessCellReorder)
			{
				return ;
			}
			if ((CursorTexture==null) || (!Input.mousePresent))
			{
				return ;
			}

			inActiveRegion = CheckInActiveRegion(Input.mousePosition, CurrentCamera);
			if (inActiveRegion)
			{
				Cursor.SetCursor(CursorTexture, CursorHotSpot, Utilites.GetCursorMode());
				cursorChanged = true;
			}
			else if (cursorChanged)
			{
				Cursor.SetCursor(DefaultCursorTexture, DefaultCursorHotSpot, Utilites.GetCursorMode());
				cursorChanged = false;
			}
		}

		bool CheckInActiveRegion(Vector2 position, Camera currentCamera)
		{
			Vector2 point;
			bool in_active_region = false;

			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, position, currentCamera, out point))
			{
				return false;
			}
			var rect = RectTransform.rect;
			if (!rect.Contains(point))
			{
				return false;
			}
			
			point += new Vector2(rect.width * RectTransform.pivot.x, 0);
			
			int i = 0;
			foreach (var child in children)
			{
				var is_first = i==0;
				if (!is_first)
				{
					in_active_region = CheckLeft(child, point);
					if (in_active_region)
					{
						break;
					}
				}
				var is_last = i==(children.Count - 1);
				if (!is_last)
				{
					in_active_region = CheckRight(child, point);
					if (in_active_region)
					{
						break;
					}
				}
				
				i++;
			}

			return in_active_region;
		}

		float widthLimit;

		/// <summary>
		/// Raises the begin drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (!AllowResize)
			{
				return ;
			}
			if (ProcessCellReorder)
			{
				return ;
			}

			Vector2 point;
			processDrag = false;

			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.pressPosition, eventData.pressEventCamera, out point))
			{
				return ;
			}
			
			var r = RectTransform.rect;
			point += new Vector2(r.width * RectTransform.pivot.x, 0);
			
			int i = 0;
			foreach (var child in children)
			{
				var is_first = i==0;
				if (!is_first)
				{
					processDrag = CheckLeft(child, point);
					if (processDrag)
					{
						leftTarget = childrenLayouts[i - 1];
						rightTarget = childrenLayouts[i];
						widthLimit = children[i - 1].rect.width + children[i].rect.width;
						break;
					}
				}
				var is_last = i==(children.Count - 1);
				if (!is_last)
				{
					processDrag = CheckRight(child, point);
					if (processDrag)
					{
						leftTarget = childrenLayouts[i];
						rightTarget = childrenLayouts[i + 1];
						widthLimit = children[i].rect.width + children[i + 1].rect.width;
						break;
					}
				}
				
				i++;
			}
		}

		/// <summary>
		/// Checks if point in the left region.
		/// </summary>
		/// <returns><c>true</c>, if point in the left region, <c>false</c> otherwise.</returns>
		/// <param name="childRectTransform">RectTransform.</param>
		/// <param name="point">Point.</param>
		bool CheckLeft(RectTransform childRectTransform, Vector2 point)
		{
			var r = childRectTransform.rect;
			r.position += new Vector2(childRectTransform.anchoredPosition.x, 0);
			r.width = ActiveRegion;

			return r.Contains(point);
		}

		/// <summary>
		/// Checks if point in the right region.
		/// </summary>
		/// <returns><c>true</c>, if right was checked, <c>false</c> otherwise.</returns>
		/// <param name="childRectTransform">Child rect transform.</param>
		/// <param name="point">Point.</param>
		bool CheckRight(RectTransform childRectTransform, Vector2 point)
		{
			var r = childRectTransform.rect;
			
			r.position += new Vector2(childRectTransform.anchoredPosition.x, 0);
			r.position = new Vector2(r.position.x + r.width - ActiveRegion - 1, r.position.y);
			r.width = ActiveRegion + 1;
			
			return r.Contains(point);
		}

		/// <summary>
		/// Raises the end drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnEndDrag(PointerEventData eventData)
		{
			if (!processDrag)
			{
				return ;
			}

			Cursor.SetCursor(DefaultCursorTexture, DefaultCursorHotSpot, Utilites.GetCursorMode());

			CalculateWidths();
			ResetChildren();
			if (!OnDragUpdate)
			{
				Resize();
			}
			processDrag = false;
		}

		/// <summary>
		/// Raises the drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnDrag(PointerEventData eventData)
		{
			if (!processDrag)
			{
				return ;
			}
			if (canvas==null)
			{
				throw new MissingComponentException(gameObject.name + " not in Canvas hierarchy.");
			}
			Cursor.SetCursor(CursorTexture, CursorHotSpot, Utilites.GetCursorMode());

			Vector2 p1;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, CurrentCamera, out p1);
			Vector2 p2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position - eventData.delta, CurrentCamera, out p2);
			var delta = p1 - p2;

			if (delta.x > 0)
			{
				leftTarget.preferredWidth = Mathf.Min(leftTarget.preferredWidth + delta.x, widthLimit - rightTarget.minWidth);
				rightTarget.preferredWidth = widthLimit - leftTarget.preferredWidth;
			}
			else
			{
				rightTarget.preferredWidth = Mathf.Min(rightTarget.preferredWidth - delta.x, widthLimit - leftTarget.minWidth);
				leftTarget.preferredWidth = widthLimit - rightTarget.preferredWidth;
			}

			if (layout!=null)
			{
				Utilites.UpdateLayout(layout);
			}

			if (OnDragUpdate)
			{
				CalculateWidths();
				Resize();
			}
		}

		float GetRectWidth(RectTransform rect)
		{
			return rect.rect.width;
		}

		/// <summary>
		/// Calculates the widths.
		/// </summary>
		void CalculateWidths()
		{
			widths = children.Select<RectTransform,float>(GetRectWidth).ToList();
		}

		/// <summary>
		/// Resets the children widths.
		/// </summary>
		void ResetChildren()
		{
			childrenLayouts.ForEach(ResetChildrenWidth);
		}

		void ResetChildrenWidth(LayoutElement element, int index)
		{
			element.preferredWidth = widths[index];
		}

		/// <summary>
		/// Resize items in ListView.
		/// </summary>
		public void Resize()
		{
			if (List==null)
			{
				return ;
			}
			if (widths.Count < 2)
			{
				return ;
			}
			List.Start();
			List.ForEachComponent(ResizeComponent);
		}

		void Reorder()
		{
			if (List==null)
			{
				return ;
			}
			if (widths.Count < 2)
			{
				return ;
			}
			List.Start();
			List.ForEachComponent(ReorderComponent);
		}

		/// <summary>
		/// Resizes the game object.
		/// </summary>
		/// <param name="go">Go.</param>
		/// <param name="i">The index.</param>
		void ResizeGameObject(GameObject go, int i)
		{
			var layoutElement = go.GetComponent<LayoutElement>();
			var position = positions.IndexOf(i);
			if (layoutElement)
			{
				layoutElement.preferredWidth = widths[position];
			}
			else
			{
				(go.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widths[position]);
			}
		}

		/// <summary>
		/// Resizes the component.
		/// </summary>
		/// <param name="component">Component.</param>
		void ResizeComponent(ListViewItem component)
		{
			var resizable_item = component as IResizableItem;
			if (resizable_item!=null)
			{
				resizable_item.ObjectsToResize.ForEach(ResizeGameObject);
			}
		}

		void ReorderComponent(ListViewItem component)
		{
			var resizable_item = component as IResizableItem;
			if (resizable_item!=null)
			{
				var objects = resizable_item.ObjectsToResize;
				int i = 0;
				foreach (var pos in positions)
				{
					objects[pos].transform.SetSiblingIndex(i);
					i++;
				}
			}
		}

		/// <summary>
		/// Move column from oldColumnPosition to newColumnPosition.
		/// </summary>
		/// <param name="oldColumnPosition">Old column position.</param>
		/// <param name="newColumnPosition">New column position.</param>
		public void Reorder(int oldColumnPosition, int newColumnPosition)
		{
			var old_pos = positions.IndexOf(oldColumnPosition);
			var new_pos = positions.IndexOf(newColumnPosition);
			
			children[old_pos].SetSiblingIndex(new_pos);
			ChangePosition(childrenLayouts, old_pos, new_pos);
			ChangePosition(children, old_pos, new_pos);
			ChangePosition(positions, old_pos, new_pos);
			
			Reorder();
		}

		#region IDropSupport<ResizableHeaderCell>
		/// <summary>
		/// Determines whether this instance can receive drop with the specified data and eventData.
		/// </summary>
		/// <returns><c>true</c> if this instance can receive drop with the specified data and eventData; otherwise, <c>false</c>.</returns>
		/// <param name="cell">Cell.</param>
		/// <param name="eventData">Event data.</param>
		public bool CanReceiveDrop(ResizableHeaderDragCell cell, PointerEventData eventData)
		{
			if (!AllowReorder)
			{
				return false;
			}
			var target = FindTarget(eventData);
			return target!=null && target!=cell;
		}

		/// <summary>
		/// Process dropped data.
		/// </summary>
		/// <param name="cell">Cell.</param>
		/// <param name="eventData">Event data.</param>
		public void Drop(ResizableHeaderDragCell cell, PointerEventData eventData)
		{
			var target = FindTarget(eventData);

			Reorder(cell.Position, target.Position);
		}

		/// <summary>
		/// Process canceled drop.
		/// </summary>
		/// <param name="cell">Cell.</param>
		/// <param name="eventData">Event data.</param>
		public void DropCanceled(ResizableHeaderDragCell cell, PointerEventData eventData)
		{
		}

		protected static void ChangePosition<T>(List<T> list, int oldPosition, int newPosition)
		{
			var item = list[oldPosition];
			list.RemoveAt(oldPosition);
			list.Insert(newPosition, item);
		}

		List<RaycastResult> raycastResults = new List<RaycastResult>();

		protected ResizableHeaderDragCell FindTarget(PointerEventData eventData)
		{
			raycastResults.Clear();
			
			EventSystem.current.RaycastAll(eventData, raycastResults);
			
			foreach (var raycastResult in raycastResults)
			{
				if (!raycastResult.isValid)
				{
					continue ;
				}
				
				#if UNITY_4_6 || UNITY_4_7
				var target = raycastResult.gameObject.GetComponent(typeof(ResizableHeaderDragCell)) as ResizableHeaderDragCell;
				#else
				var target = raycastResult.gameObject.GetComponent<ResizableHeaderDragCell>();
				#endif
				if ((target!=null) && target.transform.IsChildOf(transform))
				{
					return target;
				}
			}
			return null;
		}
		#endregion

		protected virtual void OnDestroy()
		{
			children.Clear();
			childrenLayouts.Clear();
			foreach (Transform child in transform)
			{
				var events = child.GetComponent<ResizableHeaderCell>();
				if (events==null)
				{
					continue ;
				}
				events.OnInitializePotentialDragEvent.RemoveListener(OnInitializePotentialDrag);
				events.OnBeginDragEvent.RemoveListener(OnBeginDrag);
				events.OnDragEvent.RemoveListener(OnDrag);
				events.OnEndDragEvent.RemoveListener(OnEndDrag);
			}
		}
	}
}