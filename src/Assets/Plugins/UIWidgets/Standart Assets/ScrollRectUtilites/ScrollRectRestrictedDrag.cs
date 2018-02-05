using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	public class ScrollRectRestrictedDrag : ScrollRect
	{
		public Vector2 MaxDrag = Vector2.zero;

		protected Vector2 CursorStartPosition = Vector2.zero;

		public override void OnBeginDrag(PointerEventData eventData)
        {
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return ;
			}
			
			if (!IsActive())
			{
				return ;
			}

			CursorStartPosition = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out CursorStartPosition);
            
			base.OnBeginDrag(eventData);
        }
		
		public override void OnDrag(PointerEventData eventData)
        {
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return ;
			}

			if (!IsActive())
			{
				return ;
			}
			
			Vector2 localCursor;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out localCursor))
			{
				return ;
			}

			if (MaxDrag!=Vector2.zero)
			{
				var pointerDelta = localCursor - CursorStartPosition;
				var delta = Vector2.zero;

				if ((MaxDrag.x > 0f) && (Mathf.Abs(pointerDelta.x) > MaxDrag.x) && (horizontal))
				{
					delta.x = pointerDelta.x > 0 ? MaxDrag.x - pointerDelta.x : - (MaxDrag.x + pointerDelta.x);
				}
				if ((MaxDrag.y > 0f) && (Mathf.Abs(pointerDelta.y) > MaxDrag.y) && (vertical))
				{
					delta.y = pointerDelta.y > 0 ? MaxDrag.y - pointerDelta.y : - (MaxDrag.y + pointerDelta.y);
				}
				Vector3 source_point;
				RectTransformUtility.ScreenPointToWorldPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out source_point);

				var result_point = new Vector3(source_point.x + delta.x, source_point.y + delta.y);
				eventData.position = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, result_point);
			}

			base.OnDrag(eventData);
        }
	}
}