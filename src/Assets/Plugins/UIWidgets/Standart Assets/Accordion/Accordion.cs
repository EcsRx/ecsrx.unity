using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// AccordionDirection.
	/// </summary>
	public enum AccordionDirection {
		Horizontal = 0,
		Vertical = 1,
	}

	/// <summary>
	/// Accordion.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/Accordion")]
	public class Accordion : MonoBehaviour {
		/// <summary>
		/// The items.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("Items")]
		protected List<AccordionItem> items = new List<AccordionItem>();

		/// <summary>
		/// Gets the items.
		/// </summary>
		/// <value>The items.</value>
		public List<AccordionItem> Items {
			get {
				return items;
			}
			protected set {
				if (items!=null)
				{
					RemoveCallbacks();
				}
				items = value;
				if (items!=null)
				{
					AddCallbacks();
				}
			}
		}
		
		/// <summary>
		/// Only one item can be opened.
		/// </summary>
		[SerializeField]
		public bool OnlyOneOpen = true;
		
		/// <summary>
		/// Animate open and close.
		/// </summary>
		[SerializeField]
		public bool Animate = true;

		/// <summary>
		/// The duration of the animation.
		/// </summary>
		[SerializeField]
		public float AnimationDuration = 0.5f;

		/// <summary>
		/// The direction.
		/// </summary>
		[SerializeField]
		public AccordionDirection Direction = AccordionDirection.Vertical;

		/// <summary>
		/// OnToggleItem event.
		/// </summary>
		[SerializeField]
		public AccordionEvent OnToggleItem = new AccordionEvent();

		/// <summary>
		/// The callbacks.
		/// </summary>
		protected List<UnityAction> callbacks = new List<UnityAction>();

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected virtual void Start()
		{
			AddCallbacks();
			UpdateLayout();
		}

		/// <summary>
		/// Adds the callback.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void AddCallback(AccordionItem item)
		{
			if (item.Open)
			{
				Open(item, false);
			}
			else
			{
				Close(item, false);
			}
			UnityAction callback = () => ToggleItem(item);
			
			item.ToggleObject.AddComponent<AccordionItemComponent>().OnClick.AddListener(callback);
			item.ContentObjectRect = item.ContentObject.transform as RectTransform;
			item.ContentLayoutElement = item.ContentObject.GetComponent<LayoutElement>();
			if (item.ContentLayoutElement==null)
			{
				item.ContentLayoutElement = item.ContentObject.AddComponent<LayoutElement>();
			}
			item.ContentObjectHeight = item.ContentObjectRect.rect.height;
			if (item.ContentObjectHeight==0f)
			{
				item.ContentObjectHeight = item.ContentLayoutElement.preferredHeight;
			}
			item.ContentObjectWidth = item.ContentObjectRect.rect.width;
			if (item.ContentObjectWidth==0f)
			{
				item.ContentObjectWidth = item.ContentLayoutElement.preferredWidth;
			}

			callbacks.Add(callback);
		}

		/// <summary>
		/// Adds the callbacks.
		/// </summary>
		protected virtual void AddCallbacks()
		{
			Items.ForEach(AddCallback);
		}

		/// <summary>
		/// Removes the callback.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="index">Index.</param>
		protected virtual void RemoveCallback(AccordionItem item, int index)
		{
			if (item==null)
			{
				return ;
			}
			if (item.ToggleObject==null)
			{
				return ;
			}
			
			var component = item.ToggleObject.GetComponent<AccordionItemComponent>();
			if ((component!=null) && (index < callbacks.Count))
			{
				component.OnClick.RemoveListener(callbacks[index]);
			}
		}

		/// <summary>
		/// Removes the callbacks.
		/// </summary>
		protected virtual void RemoveCallbacks()
		{
			Items.ForEach(RemoveCallback);
			callbacks.Clear();
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			RemoveCallbacks();
		}

		/// <summary>
		/// Toggles the item.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual void ToggleItem(AccordionItem item)
		{
			if (item.Open)
			{
				if (!OnlyOneOpen)
				{
					Close(item);
				}
			}
			else
			{
				if (OnlyOneOpen)
				{
					Items.Where(IsOpen).ForEach(Close);
				}

				Open(item);
			}
		}

		/// <summary>
		/// Open the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual void Open(AccordionItem item)
		{
			if (item.Open)
			{
				return ;
			}
			if (OnlyOneOpen)
			{
				Items.Where(IsOpen).ForEach(Close);
			}
			Open(item, Animate);
		}

		/// <summary>
		/// Close the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual void Close(AccordionItem item)
		{
			if (!item.Open)
			{
				return ;
			}
			Close(item, Animate);
		}

		/// <summary>
		/// Determines whether this instance is open the specified item.
		/// </summary>
		/// <returns><c>true</c> if this instance is open the specified item; otherwise, <c>false</c>.</returns>
		/// <param name="item">Item.</param>
		protected bool IsOpen(AccordionItem item)
		{
			return item.Open;
		}

		/// <summary>
		/// Determines whether this instance is horizontal.
		/// </summary>
		/// <returns><c>true</c> if this instance is horizontal; otherwise, <c>false</c>.</returns>
		protected bool IsHorizontal()
		{
			return Direction==AccordionDirection.Horizontal;
		}

		/// <summary>
		/// Open the specified item and animate.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="animate">If set to <c>true</c> animate.</param>
		protected virtual void Open(AccordionItem item, bool animate)
		{
			if (item.CurrentCorutine!=null)
			{
				StopCoroutine(item.CurrentCorutine);

				if (IsHorizontal())
				{
					item.ContentObjectRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.ContentObjectWidth);
					item.ContentLayoutElement.preferredWidth = item.ContentObjectWidth;
				}
				else
				{
					item.ContentObjectRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.ContentObjectHeight);
					item.ContentLayoutElement.preferredHeight = item.ContentObjectHeight;
				}
				item.ContentObject.SetActive(false);
			}
			if (animate)
			{
				item.CurrentCorutine = StartCoroutine(OpenCorutine(item));
			}
			else
			{
				item.ContentObject.SetActive(true);
				OnToggleItem.Invoke(item);
			}

			item.ContentObject.SetActive(true);
			item.Open = true;
		}

		/// <summary>
		/// Close the specified item and animate.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="animate">If set to <c>true</c> animate.</param>
		protected virtual void Close(AccordionItem item, bool animate)
		{
			if (item.CurrentCorutine!=null)
			{
				StopCoroutine(item.CurrentCorutine);
				item.ContentObject.SetActive(true);
				if (IsHorizontal())
				{
					item.ContentObjectRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.ContentObjectWidth);
					item.ContentLayoutElement.preferredWidth = item.ContentObjectWidth;
				}
				else
				{
					item.ContentObjectRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.ContentObjectHeight);
					item.ContentLayoutElement.preferredHeight = item.ContentObjectHeight;
				}
			}
			if (item.ContentObjectRect!=null)
			{
				item.ContentObjectHeight = item.ContentObjectRect.rect.height;
				item.ContentObjectWidth = item.ContentObjectRect.rect.width;
			}

			if (animate)
			{
				item.CurrentCorutine = StartCoroutine(HideCorutine(item));
			}
			else
			{
				item.ContentObject.SetActive(false);
				item.Open = false;
				OnToggleItem.Invoke(item);
			}

		}

		/// <summary>
		/// Opens the corutine.
		/// </summary>
		/// <returns>The corutine.</returns>
		/// <param name="item">Item.</param>
		protected virtual IEnumerator OpenCorutine(AccordionItem item)
		{
			item.ContentObject.SetActive(true);
			item.Open = true;

			yield return StartCoroutine(Animations.Open(item.ContentObjectRect, AnimationDuration, IsHorizontal()));

			UpdateLayout();

			OnToggleItem.Invoke(item);
		}

		/// <summary>
		/// Updates the layout.
		/// </summary>
		protected void UpdateLayout()
		{
			Utilites.UpdateLayout(GetComponent<LayoutGroup>());
		}

		/// <summary>
		/// Hides the corutine.
		/// </summary>
		/// <returns>The corutine.</returns>
		/// <param name="item">Item.</param>
		protected virtual IEnumerator HideCorutine(AccordionItem item)
		{
			yield return StartCoroutine(Animations.Collapse(item.ContentObjectRect, AnimationDuration, IsHorizontal()));

			item.Open = false;
			item.ContentObject.SetActive(false);

			UpdateLayout();

			OnToggleItem.Invoke(item);
		}
	}
}