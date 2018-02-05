using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIWidgetsSamples {
	public class TableCell : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler {
		public UnityEvent CellClicked = new UnityEvent();

		public void OnPointerClick(PointerEventData eventData)
		{
			CellClicked.Invoke();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
		}

		public void OnPointerDown(PointerEventData eventData)
		{
		}
	}
}