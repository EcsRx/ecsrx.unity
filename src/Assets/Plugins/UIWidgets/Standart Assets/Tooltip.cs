using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// Tooltip.
	/// http://ilih.ru/images/unity-assets/UIWidgets/Tooltip.png
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/Tooltip")]
	[RequireComponent(typeof(RectTransform))]
	public class Tooltip : MonoBehaviour,
		IPointerEnterHandler,
		IPointerExitHandler,
		ISelectHandler,
		IDeselectHandler
	{
		[SerializeField]
		GameObject tooltipObject;

		/// <summary>
		/// Seconds before tooltip shown after pointer enter.
		/// </summary>
		[SerializeField]
		public float ShowDelay = 0.3f;

		/// <summary>
		/// The tooltip object.
		/// </summary>
		public GameObject TooltipObject {
			get {
				return tooltipObject;
			}
			set {
				tooltipObject = value;
				if (tooltipObject!=null)
				{
					tooltipObjectParent = tooltipObject.transform.parent;
				}
			}
		}

		Vector2 anchoredPosition;
		Transform canvasTransform;
		Transform tooltipObjectParent;

		void Start()
		{
			TooltipObject = tooltipObject;

			if (TooltipObject!=null)
			{
				canvasTransform = Utilites.FindTopmostCanvas(tooltipObjectParent);
				TooltipObject.SetActive(false);
			}
		}

		IEnumerator currentCorutine;

		IEnumerator ShowCorutine()
		{
			yield return new WaitForSeconds(ShowDelay);

			if (canvasTransform!=null)
			{
				anchoredPosition = (TooltipObject.transform as RectTransform).anchoredPosition;
				tooltipObjectParent = tooltipObject.transform.parent;
				TooltipObject.transform.SetParent(canvasTransform);
			}
			TooltipObject.SetActive(true);
		}

		/// <summary>
		/// Show this tooltip.
		/// </summary>
		public void Show()
		{
			if (TooltipObject==null)
			{
				return ;
			}

			currentCorutine = ShowCorutine();
			StartCoroutine(currentCorutine);
		}

		IEnumerator HideCoroutine()
		{
			if (currentCorutine!=null)
			{
				StopCoroutine(currentCorutine);
			}
			if (TooltipObject!=null)
			{
				TooltipObject.SetActive(false);
				yield return null;
				if (canvasTransform!=null)
				{
					TooltipObject.transform.SetParent(tooltipObjectParent);
					(TooltipObject.transform as RectTransform).anchoredPosition = anchoredPosition;
				}
			}
		}

		/// <summary>
		/// Hide this tooltip.
		/// </summary>
		public void Hide()
		{
			StartCoroutine(HideCoroutine());
		}

		/// <summary>
		/// Raises the pointer enter event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnPointerEnter(PointerEventData eventData)
		{
			Show();
		}

		/// <summary>
		/// Raises the pointer exit event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnPointerExit(PointerEventData eventData)
		{
			Hide();
		}

		/// <summary>
		/// Raises the select event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnSelect(BaseEventData eventData)
		{
			Show();
		}

		/// <summary>
		/// Raises the deselect event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnDeselect(BaseEventData eventData)
		{
			Hide();
		}

#if UNITY_EDITOR
		void CreateTooltipObject()
		{
			TooltipObject = Utilites.CreateWidgetFromAsset("Tooltip");
			TooltipObject.transform.SetParent(transform);

			var tooltipRectTransform = TooltipObject.transform as RectTransform;

			tooltipRectTransform.anchorMin = new Vector2(1, 1);
			tooltipRectTransform.anchorMax = new Vector2(1, 1);
			tooltipRectTransform.pivot = new Vector2(1, 0);
			
			tooltipRectTransform.anchoredPosition = new Vector2(0, 0);
		}

		/// <summary>
		/// Reset this instance.
		/// </summary>
		protected void Reset()
		{
			if (TooltipObject==null)
			{
				CreateTooltipObject();
			}

		}
#endif
	}
}