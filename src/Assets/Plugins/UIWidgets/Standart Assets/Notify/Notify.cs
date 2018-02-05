using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Events;

namespace UIWidgets
{
	/// <summary>
	/// Notify.
	/// Manage notifications.
	/// 
	/// How to use:
	/// 1. Create container or containers with layout component. Notifications will be shown in those containers. You can check how it works with NotifyContainer in sample scene.
	/// 2. Create template for notification with Notify component.
	/// 3. If you want change text in runtime set Text property in Notify component.
	/// 4. If you want close notification by button set Hide button property in Notify component.
	/// 5. Write code to show notification
	/// <example>
	/// notifyPrefab.Template().Show("Sticky Notification. Click on the × above to close.");
	/// </example>
	/// notifyPrefab.Template() - return the notification instance by template name.
	/// Show("Sticky Notification. Click on the × above to close.") - show notification with following text;
	/// or
	/// Show(message: "Simple Notification.", customHideDelay = 4.5f, hideAnimation = UIWidgets.Notify.AnimationCollapse, slideUpOnHide = false);
	/// Show notification with following text, hide it after 4.5 seconds, run specified animation on hide without SlideUpOnHide.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/Notify")]
	public class Notify : MonoBehaviour, ITemplatable
	{
        public UnityEvent ClosedEvent { get; set; }

        [SerializeField]
		Button hideButton;

		/// <summary>
		/// Gets or sets the button that close current notification.
		/// </summary>
		/// <value>The hide button.</value>
		public Button HideButton {
			get {
				return hideButton;
			}
			set {
				if (hideButton!=null)
				{
					hideButton.onClick.RemoveListener(Hide);
				}
				hideButton = value;
				if (hideButton!=null)
				{
					hideButton.onClick.AddListener(Hide);
				}
			}
		}

		[SerializeField]
		Text text;

		/// <summary>
		/// Gets or sets the text component.
		/// </summary>
		/// <value>The text.</value>
		public Text Text {
			get {
				return text;
			}
			set {
				text = value;
			}
		}

		[SerializeField]
		float HideDelay = 10f;

		[SerializeField]
		bool unscaledTime;

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

		static Templates<Notify> templates;

		/// <summary>
		/// Notify templates.
		/// </summary>
		public static Templates<Notify> Templates {
			get {
				if (templates==null)
				{
					templates = new Templates<Notify>(AddCloseCallback);
				}
				return templates;
			}
			set {
				templates = value;
			}
		}

		/// <summary>
		/// Function used to run show animation.
		/// </summary>
		public Func<Notify,IEnumerator> ShowAnimation;

		/// <summary>
		/// Function used to run hide animation.
		/// </summary>
		public Func<Notify,IEnumerator> HideAnimation;
		Func<Notify,IEnumerator> oldShowAnimation;
		Func<Notify,IEnumerator> oldHideAnimation;

		IEnumerator showCorutine;
		IEnumerator hideCorutine;

		/// <summary>
		/// Start slide up animations after hide current notification. Turn it off if its managed with HideAnimation.
		/// </summary>
		public bool SlideUpOnHide = true;

		NotifyInfoBase notifyInfo;

		/// <summary>
		/// Gets the dialog info.
		/// </summary>
		/// <value>The dialog info.</value>
		public NotifyInfoBase NotifyInfo {
			get {
				if (notifyInfo==null)
				{
					notifyInfo = GetComponent<NotifyInfoBase>();
				}
				return notifyInfo;
			}
		}

		void Awake()
		{
			if (IsTemplate)
			{
				gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Finds the templates.
		/// </summary>
		static void FindTemplates()
		{
			Templates.FindTemplates();
		}

		void OnDestroy()
		{
			HideButton = null;
			if (!IsTemplate)
			{
				templates = null;
				return ;
			}
			//if FindTemplates never called than TemplateName==null
			if (TemplateName!=null)
			{
				DeleteTemplate(TemplateName);
			}
		}

		/// <summary>
		/// Clears the cached instance of templates.
		/// </summary>
		static public void ClearCache()
		{
			Templates.ClearCache();
		}

		/// <summary>
		/// Clears the cached instance of specified template.
		/// </summary>
		/// <param name="templateName">Template name.</param>
		static public void ClearCache(string templateName)
		{
			Templates.ClearCache(templateName);
		}

		/// <summary>
		/// Gets the template by name.
		/// </summary>
		/// <returns>The template.</returns>
		/// <param name="template">Template name.</param>
		static public Notify GetTemplate(string template)
		{
			return Templates.Get(template);
		}

		/// <summary>
		/// Deletes the template by name.
		/// </summary>
		/// <param name="template">Template.</param>
		static public void DeleteTemplate(string template)
		{
			Templates.Delete(template);
		}

		/// <summary>
		/// Adds the template.
		/// </summary>
		/// <param name="template">Template name.</param>
		/// <param name="notifyTemplate">Notify template object.</param>
		/// <param name="replace">If set to <c>true</c> replace.</param>
		static public void AddTemplate(string template, Notify notifyTemplate, bool replace = true)
		{
			Templates.Add(template, notifyTemplate, replace);
		}

		/// <summary>
		/// Return notification by the specified template name.
		/// </summary>
		/// <param name="template">Template name.</param>
		static public Notify Template(string template)
		{
			return Templates.Instance(template);
		}

		/// <summary>
		/// Return Notify instance using current instance as template.
		/// </summary>
		public Notify Template()
		{
			if ((TemplateName!=null) && Templates.Exists(TemplateName))
			{
				//do nothing
			}
			else if (!Templates.Exists(gameObject.name))
			{
				Templates.Add(gameObject.name, this);
			}
			else if (Templates.Get(gameObject.name)!=this)
			{
				Templates.Add(gameObject.name, this);
			}

			var id = gameObject.GetInstanceID().ToString();
			if (!Templates.Exists(id))
			{
				Templates.Add(id, this);
			}
			else if (Templates.Get(id)!=this)
			{
				Templates.Add(id, this);
			}

			return Templates.Instance(id);
		}

		/// <summary>
		/// Adds the close callback.
		/// </summary>
		/// <param name="notify">Notify.</param>
		static void AddCloseCallback(Notify notify)
		{
			if (notify.hideButton==null)
			{
				return ;
			}
			notify.hideButton.onClick.AddListener(notify.Hide);
		}

		/// <summary>
		/// Time between previous notification was hidden and next will be showed.
		/// </summary>
		public float SequenceDelay;

		/// <summary>
		/// The notify manager.
		/// </summary>
		static NotifySequenceManager notifyManager;

		/// <summary>
		/// Gets the notify manager.
		/// </summary>
		/// <value>The notify manager.</value>
		static public NotifySequenceManager NotifyManager {
			get {
				if (notifyManager==null)
				{
					var go = new GameObject("NotifySequenceManager");
					notifyManager = go.AddComponent<NotifySequenceManager>();
				}
				return notifyManager;
			}
		}

		/// <summary>
		/// Show the notification.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="customHideDelay">Custom hide delay.</param>
		/// <param name="container">Container. Parent object for current notification.</param>
		/// <param name="showAnimation">Function used to run show animation.</param>
		/// <param name="hideAnimation">Function used to run hide animation.</param>
		/// <param name="slideUpOnHide">Start slide up animations after hide current notification.</param>
		/// <param name="sequenceType">Add notification to sequence and display in order according specified sequenceType.</param>
		/// <param name="sequenceDelay">Time between previous notification was hidden and next will be showed.</param>
		/// <param name="clearSequence">Clear notifications sequence and hide current notification.</param>
		/// <param name="newUnscaledTime">Use unscaled time.</param>
		public void Show(string message = null,
		                 float? customHideDelay = null,
		                 Transform container = null,
		                 Func<Notify,IEnumerator> showAnimation = null,
		                 Func<Notify,IEnumerator> hideAnimation = null,
		                 bool? slideUpOnHide = null,
		                 NotifySequence sequenceType = NotifySequence.None,
		                 float sequenceDelay = 0.3f,
		                 bool clearSequence = false,
		                 bool? newUnscaledTime = null)
		{
            if (ClosedEvent == null)
            {
                ClosedEvent = new UnityEvent();
            }

            if (clearSequence)
			{
				NotifyManager.Clear();
			}

			SequenceDelay = sequenceDelay;

			oldShowAnimation = ShowAnimation;
			oldHideAnimation = HideAnimation;

			SetMessage(message);

			if (container!=null)
			{
				transform.SetParent(container, false);
			}

			if (newUnscaledTime!=null)
			{
				unscaledTime = (bool)newUnscaledTime;
			}

			if (customHideDelay!=null)
			{
				HideDelay = (float)customHideDelay;
			}

			if (slideUpOnHide!=null)
			{
				SlideUpOnHide = (bool)slideUpOnHide;
			}

			if (showAnimation!=null)
			{
				ShowAnimation = showAnimation;
			}

			if (hideAnimation!=null)
			{
				HideAnimation = hideAnimation;
			}

			if (sequenceType!=NotifySequence.None)
			{
				NotifyManager.Add(this, sequenceType);
			}
			else
			{

				Display();
			}
		}

		public virtual void SetMessage(string message)
		{
			if (NotifyInfo!=null)
			{
				NotifyInfo.SetInfo(message);
			}
			else
			{
				if ((message!=null) && (Text!=null))
				{
					Text.text = message;
				}
			}
		}

		Action OnHideCallback;

		/// <summary>
		/// Display notification.
		/// </summary>
		/// <param name="onHideCallback">On hide callback.</param>
		public void Display(Action onHideCallback=null)
		{
			transform.SetAsLastSibling();
			gameObject.SetActive(true);

			OnHideCallback = onHideCallback;

			if (ShowAnimation!=null)
			{
				showCorutine = ShowAnimation(this);
				StartCoroutine(showCorutine);
			}
			else
			{
				showCorutine = null;
			}
			
			if (HideDelay > 0.0f)
			{
				hideCorutine = HideCorutine();
				StartCoroutine(hideCorutine);
			}
			else
			{
				hideCorutine = null;
			}
		}

		IEnumerator HideCorutine()
		{
			yield return new WaitForSeconds(HideDelay);
			if (HideAnimation!=null)
			{
				yield return StartCoroutine(HideAnimation(this));
			}
			Hide();
		}

		/// <summary>
		/// Hide notification.
		/// </summary>
		public void Hide()
		{
			if (SlideUpOnHide)
			{
				SlideUp();
			}
			if (OnHideCallback!=null)
			{
				OnHideCallback();
			}

            if (ClosedEvent != null)
            {
                ClosedEvent.Invoke();
            }

            Return();
		}

		/// <summary>
		/// Return this instance to cache.
		/// </summary>
		public void Return()
		{
			//Templates.ToCache(this);

			//ShowAnimation = oldShowAnimation;
			//HideAnimation = oldHideAnimation;

			//SetMessage(Templates.Get(TemplateName).text.text);
		}

		static Stack<RectTransform> slides = new Stack<RectTransform>();

		RectTransform GetSlide()
		{
			RectTransform rect;

			if (slides.Count==0)
			{
				var obj = new GameObject("SlideUp");
				obj.SetActive(false);
				rect = obj.AddComponent<RectTransform>();
				obj.AddComponent<SlideUp>();
				
				//change height don't work without graphic component
				var image = obj.AddComponent<Image>();
				image.color = Color.clear;
			}
			else
			{
				do
				{
					rect = (slides.Count > 0) ? slides.Pop() : GetSlide();
				}
				while (rect==null);
			}
			return rect;
		}

		/// <summary>
		/// Slides up.
		/// </summary>
		void SlideUp()
		{
			var rect = GetSlide();
			SlideUp slide = rect.GetComponent<SlideUp>();
			slide.UnscaledTime = unscaledTime;

			var sourceRect = transform as RectTransform;
			
			rect.localRotation = sourceRect.localRotation;
			rect.localPosition = sourceRect.localPosition;
			rect.localScale = sourceRect.localScale;
			rect.anchorMin = sourceRect.anchorMin;
			rect.anchorMax = sourceRect.anchorMax;
			rect.anchoredPosition = sourceRect.anchoredPosition;
			rect.anchoredPosition3D = sourceRect.anchoredPosition3D;
			rect.sizeDelta = sourceRect.sizeDelta;
			rect.pivot = sourceRect.pivot;
			
			rect.transform.SetParent(transform.parent, false);
			rect.transform.SetSiblingIndex(transform.GetSiblingIndex());
			
			rect.gameObject.SetActive(true);
			slide.Run();
		}

		/// <summary>
		/// Returns slide to cache.
		/// </summary>
		/// <param name="slide">Slide.</param>
		public static void FreeSlide(RectTransform slide)
		{
			slides.Push(slide);
		}

		/// <summary>
		/// Rotate animation.
		/// </summary>
		/// <param name="notify">Notify.</param>
		static public IEnumerator AnimationRotate(Notify notify)
		{
			var rect = notify.transform as RectTransform;
			var start_rotarion = rect.localRotation.eulerAngles;
			var length = 0.5f;
			
			var end_time = Time.time + length;

			while (Time.time <= end_time)
			{
				var rotation_x = Mathf.Lerp(0, 90, 1 - (end_time - Time.time) / length);

				rect.localRotation = Quaternion.Euler(rotation_x, start_rotarion.y, start_rotarion.z);
				yield return null;
			}
			
			//return rotation back for future use
			rect.localRotation = Quaternion.Euler(start_rotarion);
		}

		/// <summary>
		/// Rotate animation.
		/// </summary>
		/// <param name="notify">Notify.</param>
		static public IEnumerator AnimationRotateUnscaledTime(Notify notify)
		{
			var rect = notify.transform as RectTransform;
			var start_rotarion = rect.localRotation.eulerAngles;
			var length = 0.5f;

			var end_time = Time.unscaledTime + length;

			while (Time.unscaledTime <= end_time)
			{
				var rotation_x = Mathf.Lerp(0, 90, 1 - (end_time - Time.unscaledTime) / length);

				rect.localRotation = Quaternion.Euler(rotation_x, start_rotarion.y, start_rotarion.z);
				yield return null;
			}
			
			//return rotation back for future use
			rect.localRotation = Quaternion.Euler(start_rotarion);
		}

		/// <summary>
		/// Collapse animation.
		/// </summary>
		/// <param name="notify">Notify.</param>
		static public IEnumerator AnimationCollapse(Notify notify)
		{
			var rect = notify.transform as RectTransform;
			var layout = notify.GetComponentInParent<EasyLayout.EasyLayout>();
			var max_height = rect.rect.height;

			var speed = 200f;
			
			var time = max_height / speed;
			var end_time = Time.time + time;

			while (Time.time <= end_time)
			{
				var height = Mathf.Lerp(max_height, 0, 1 - (end_time - Time.time) / time);
				rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
				if (layout!=null)
				{
					layout.UpdateLayout();
				}
				yield return null;
			}

			//return height back for future use
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, max_height);
		}

		/// <summary>
		/// Collapse animation.
		/// </summary>
		/// <param name="notify">Notify.</param>
		static public IEnumerator AnimationCollapseUnscaledTime(Notify notify)
		{
			var rect = notify.transform as RectTransform;
			var layout = notify.GetComponentInParent<EasyLayout.EasyLayout>();
			var max_height = rect.rect.height;

			var speed = 200f;

			var time = max_height / speed;
			var end_time = Time.unscaledTime + time;

			while (Time.unscaledTime <= end_time)
			{
				var height = Mathf.Lerp(max_height, 0, 1 - (end_time - Time.unscaledTime) / time);
				rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
				if (layout!=null)
				{
					layout.UpdateLayout();
				}
				yield return null;
			}

			//return height back for future use
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, max_height);
		}
	}
}