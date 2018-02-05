using UnityEngine;
using System;

namespace UIWidgets {
	/// <summary>
	/// Base class for Pickers.
	/// </summary>
	public abstract class Picker<TValue,TPicker> : MonoBehaviour, ITemplatable
		where TPicker : Picker<TValue,TPicker>
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

		static Templates<TPicker> templates;

		/// <summary>
		/// Picker templates.
		/// </summary>
		public static Templates<TPicker> Templates {
			get {
				if (templates==null)
				{
					templates = new Templates<TPicker>();
				}
				return templates;
			}
			set {
				templates = value;
			}
		}

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		protected virtual void Awake()
		{
			if (IsTemplate)
			{
				gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			if (!IsTemplate)
			{
				templates = null;
				return ;
			}
			//if FindTemplates never called than TemplateName==null
			if (TemplateName!=null)
			{
				Templates.Delete(TemplateName);
			}
		}

		/// <summary>
		/// Return picker instance using current instance as template.
		/// </summary>
		public virtual TPicker Template()
		{
			if ((TemplateName!=null) && Templates.Exists(TemplateName))
			{
				//do nothing
			}
			else if (!Templates.Exists(gameObject.name))
			{
				Templates.Add(gameObject.name, this as TPicker);
			}
			else if (Templates.Get(gameObject.name)!=this)
			{
				Templates.Add(gameObject.name, this as TPicker);
			}

			var id = gameObject.GetInstanceID().ToString();
			if (!Templates.Exists(id))
			{
				Templates.Add(id, this as TPicker);
			}
			else if (Templates.Get(id)!=this)
			{
				Templates.Add(id, this as TPicker);
			}

			return Templates.Instance(id);
		}

		/// <summary>
		/// The modal key.
		/// </summary>
		protected int? ModalKey;

		/// <summary>
		/// Callback with selected value.
		/// </summary>
		protected Action<TValue> OnSelect;

		/// <summary>
		/// Callback when picker closed without any value selected.
		/// </summary>
		protected Action OnCancel;

		/// <summary>
		/// Show picker.
		/// </summary>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="onSelect">Callback with selected value.</param>
		/// <param name="onCancel">Callback when picker closed without any value selected.</param>
		/// <param name="modalSprite">Modal sprite.</param>
		/// <param name="modalColor">Modal color.</param>
		/// <param name="canvas">Canvas.</param>
		public virtual void Show(
						TValue defaultValue,
						Action<TValue> onSelect,
						Action onCancel = null,
		                Sprite modalSprite = null,
						Color? modalColor = null,

		                Canvas canvas = null)
		{
			OnSelect = onSelect;
			OnCancel = onCancel;

			var parent = (canvas!=null) ? canvas.transform : Utilites.FindTopmostCanvas(gameObject.transform);
			if (parent!=null)
			{
				transform.SetParent(parent, false);
			}

			if (modalColor==null)
			{
				modalColor = new Color(0, 0, 0, 0.8f);
			}

			ModalKey = ModalHelper.Open(this, modalSprite, modalColor, Close);

			transform.SetAsLastSibling();

			gameObject.SetActive(true);

			BeforeOpen(defaultValue);
		}

		/// <summary>
		/// Close picker with specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public virtual void Selected(TValue value)
		{
			OnSelect(value);
			Close();
		}

		/// <summary>
		/// Close picker without specified value.
		/// </summary>
		public virtual void Cancel()
		{
			if (OnCancel!=null)
			{
				OnCancel();
			}
			Close();
		}

		/// <summary>
		/// Prepare picker to open.
		/// </summary>
		public abstract void BeforeOpen(TValue defaultValue);

		/// <summary>
		/// Prepare picker to close.
		/// </summary>
		public abstract void BeforeClose();

		/// <summary>
		/// Close picker.
		/// </summary>
		protected virtual void Close()
		{
			BeforeClose();

			if (ModalKey!=null)
			{
				ModalHelper.Close((int)ModalKey);
			}

			Return();
		}

		/// <summary>
		/// Return this instance to cache.
		/// </summary>
		protected virtual void Return()
		{
			Templates.ToCache(this as TPicker);
		}
	}
}