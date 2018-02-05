using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UIWidgets
{
	[RequireComponent(typeof(RectTransform))]
	/// <summary>
	/// Modal helper for UI widgets.
	/// <example>modalKey = ModalHelper.Open(this, modalSprite, modalColor);
	/// //...
	/// ModalHelper.Close(modalKey);</example>
	/// </summary>
	public class ModalHelper : MonoBehaviour, ITemplatable, IPointerClickHandler
	{
		bool isTemplate = true;
		
		/// <summary>
		/// Gets a value indicating whether this instance is template.
		/// </summary>
		/// <value><c>true</c> if this instance is template; otherwise, <c>false</c>.</value>
		public bool IsTemplate {
			get {
				return isTemplate;
			}
			set {
				isTemplate = value;
			}
		}
		
		/// <summary>
		/// Gets the name of the template.
		/// </summary>
		/// <value>The name of the template.</value>
		public string TemplateName {
			get;
			set;
		}

		UnityEvent OnClick = new UnityEvent();
		
		static Templates<ModalHelper> Templates = new Templates<ModalHelper>();
		
		static Dictionary<int,ModalHelper> used = new Dictionary<int,ModalHelper>();
		
		static string key = "ModalTemplate";
		
		void OnDestroy()
		{
			Templates.Delete(key);
		}

		/// <summary>
		/// Raises the pointer click event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button!=PointerEventData.InputButton.Left)
			{
				return;
			}

			OnClick.Invoke();
		}

		/// <summary>
		/// Create modal helper with the specified parent, sprite and color.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="sprite">Sprite.</param>
		/// <param name="color">Color.</param>
		/// <param name="onClick">onClick callback</param>
		/// <returns>Modal helper index</returns>
		public static int Open(MonoBehaviour parent, Sprite sprite = null, Color? color = null, UnityAction onClick = null)
		{
			//check if in cache
			if (!Templates.Exists(key))
			{
				Templates.FindTemplates();
				CreateTemplate();
			}
			
			var modal = Templates.Instance(key);
			
			modal.transform.SetParent(Utilites.FindTopmostCanvas(parent.transform), false);
			modal.gameObject.SetActive(true);
			modal.transform.SetAsLastSibling();
			
			var rect = modal.transform as RectTransform;
			rect.sizeDelta = new Vector2(0, 0);
			rect.anchorMin = new Vector2(0, 0);
			rect.anchorMax = new Vector2(1, 1);
			rect.anchoredPosition = new Vector2(0, 0);
			
			var img = modal.GetComponent<Image>();
			if (sprite!=null)
			{
				img.sprite = sprite;
			}
			if (color!=null)
			{
				img.color = (Color)color;
			}

			modal.OnClick.RemoveAllListeners();
			if (onClick!=null)
			{
				modal.OnClick.AddListener(onClick);
			}
			
			used.Add(modal.GetInstanceID(), modal);
			return modal.GetInstanceID();
		}
		
		/// <summary>
		/// Creates the template.
		/// </summary>
		static void CreateTemplate()
		{
			var template = new GameObject(key);
			
			var modal = template.AddComponent<ModalHelper>();
			template.AddComponent<Image>();
			
			Templates.Add(key, modal);

			template.gameObject.SetActive(false);
		}
		
		/// <summary>
		/// Close modal helper with the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public static void Close(int index)
		{
			if ((used!=null) && (used.ContainsKey(index)))
			{
				used[index].OnClick.RemoveAllListeners();
				Templates.ToCache(used[index]);
				used.Remove(index);
			}
		}
	}
}