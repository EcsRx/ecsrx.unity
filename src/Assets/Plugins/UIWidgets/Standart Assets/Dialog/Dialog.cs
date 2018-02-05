using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIWidgets {
	/// <summary>
	/// Dialog.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/Dialog")]
	public class Dialog : MonoBehaviour, ITemplatable
	{
		[SerializeField]
		Button defaultButton;

		/// <summary>
		/// Gets or sets the default button.
		/// </summary>
		/// <value>The default button.</value>
		public Button DefaultButton {
			get {
				return defaultButton;
			}
			set {
				defaultButton = value;
			}
		}
		
		[SerializeField]
		Text titleText;
		
		/// <summary>
		/// Gets or sets the text component.
		/// </summary>
		/// <value>The text.</value>
		public Text TitleText {
			get {
				return titleText;
			}
			set {
				titleText = value;
			}
		}

		[SerializeField]
		Text contentText;
		
		/// <summary>
		/// Gets or sets the text component.
		/// </summary>
		/// <value>The text.</value>
		public Text ContentText {
			get {
				return contentText;
			}
			set {
				contentText = value;
			}
		}

		[SerializeField]
		Image dialogIcon;

		/// <summary>
		/// Gets or sets the icon component.
		/// </summary>
		/// <value>The icon.</value>
		public Image Icon {
			get {
				return dialogIcon;
			}
			set {
				dialogIcon = value;
			}
		}

		DialogInfoBase dialogInfo;

		/// <summary>
		/// Gets the dialog info.
		/// </summary>
		/// <value>The dialog info.</value>
		public DialogInfoBase DialogInfo {
			get {
				if (dialogInfo==null)
				{
					dialogInfo = GetComponent<DialogInfoBase>();
				}
				return dialogInfo;
			}
		}

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

		static Templates<Dialog> templates;

		/// <summary>
		/// Dialog templates.
		/// </summary>
		public static Templates<Dialog> Templates {
			get {
				if (templates==null)
				{
					templates = new Templates<Dialog>();
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
			//if (IsTemplate)
			//{
			//	gameObject.SetActive(false);
			//}
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			DeactivateButtons();

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
		/// Return dialog instance by the specified template name.
		/// </summary>
		/// <param name="template">Template name.</param>
		static public Dialog Template(string template)
		{
			return Templates.Instance(template);
		}

		/// <summary>
		/// Return Dialog instance using current instance as template.
		/// </summary>
		public Dialog Template()
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
		/// The modal key.
		/// </summary>
		protected int? ModalKey;

		/// <summary>
		/// Show dialog.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="buttons">Buttons.</param>
		/// <param name="focusButton">Set focus on button with specified name.</param>
		/// <param name="position">Position.</param>
		/// <param name="icon">Icon.</param>
		/// <param name="modal">If set to <c>true</c> modal.</param>
		/// <param name="modalSprite">Modal sprite.</param>
		/// <param name="modalColor">Modal color.</param>
		/// <param name="canvas">Canvas.</param>
		public virtual void Show(string title = null,
		                 string message = null,
		                 DialogActions buttons = null,
		                 string focusButton = null,
		                 Vector3? position = null,
		                 Sprite icon = null,

		                 bool modal = false,
		                 Sprite modalSprite = null,
		                 Color? modalColor = null,

		                 Canvas canvas = null)
		{
			if (position==null)
			{
				position = new Vector3(0, 0, 0);
			}

			SetInfo(title, message, icon);

			var parent = (canvas!=null) ? canvas.transform : Utilites.FindTopmostCanvas(gameObject.transform);
			if (parent!=null)
			{
				transform.SetParent(parent, false);
			}

			if (modal)
			{
				ModalKey = ModalHelper.Open(this, modalSprite, modalColor);
			}
			else
			{
				ModalKey = null;
			}

			transform.SetAsLastSibling();

			transform.localPosition = (Vector3)position;
			//gameObject.SetActive(true);

			CreateButtons(buttons, focusButton);
		}

		/// <summary>
		/// Sets the info.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="icon">Icon.</param>
		public virtual void SetInfo(string title=null, string message=null, Sprite icon=null)
		{
			if (DialogInfo!=null)
			{
				DialogInfo.SetInfo(title, message, icon);
			}
			else
			{
				if ((title!=null) && (TitleText!=null))
				{
					TitleText.text = title;
				}
				if ((message!=null) && (ContentText!=null))
				{
					ContentText.text = message;
				}
				if ((icon!=null) && (Icon!=null))
				{
					Icon.sprite = icon;
				}
			}
		}

		/// <summary>
		/// Close dialog.
		/// </summary>
		public virtual void Hide()
		{
			if (ModalKey!=null)
			{
				ModalHelper.Close((int)ModalKey);
			}

			//Return();
		}

		/// <summary>
		/// The buttons cache.
		/// </summary>
		protected Stack<Button> buttonsCache = new Stack<Button>();

		/// <summary>
		/// The buttons in use.
		/// </summary>
		protected Dictionary<string,Button> buttonsInUse = new Dictionary<string,Button>();

		/// <summary>
		/// The buttons actions.
		/// </summary>
		protected Dictionary<string,UnityAction> buttonsActions = new Dictionary<string,UnityAction>();

		/// <summary>
		/// Creates the buttons.
		/// </summary>
		/// <param name="buttons">Buttons.</param>
		/// <param name="focusButton">Focus button.</param>
		protected virtual void CreateButtons(DialogActions buttons, string focusButton)
		{
			if (buttons==null)
			{
				return ;
			}

            defaultButton.gameObject.SetActive(false);

            buttons.ForEach(x => {
				var button = GetButton();

				UnityAction callback = () => {
					if (x.Value())
					{
						Hide();
					}
				};

				buttonsInUse.Add(x.Key, button);
				buttonsActions.Add(x.Key, callback);

				button.gameObject.SetActive(true);
				button.transform.SetAsLastSibling();

				var dialog_button = button.GetComponentInChildren<DialogButtonComponentBase>();
				if (dialog_button!=null)
				{
					dialog_button.SetButtonName(x.Key);
				}
				else
				{
					var text = button.GetComponentInChildren<Text>();
					if (text!=null)
					{
						text.text = x.Key;
					}
				}

				button.onClick.AddListener(buttonsActions[x.Key]);

				if (x.Key==focusButton)
				{
					button.Select();
				}
			});
		}

		/// <summary>
		/// Gets the button.
		/// </summary>
		/// <returns>The button.</returns>
		protected virtual Button GetButton()
		{
			if (buttonsCache.Count > 0)
			{
				return buttonsCache.Pop();
			}

			var button = Instantiate(DefaultButton) as Button;

			button.transform.SetParent(DefaultButton.transform.parent, false);

			//Utilites.FixInstantiated(DefaultButton, button);

			return button;
		}

		/// <summary>
		/// Return this instance to cache.
		/// </summary>
		protected virtual void Return()
		{
			Templates.ToCache(this);

			DeactivateButtons();
			ResetParametres();
		}

		/// <summary>
		/// Deactivates the buttons.
		/// </summary>
		protected virtual void DeactivateButtons()
		{
			buttonsInUse.ForEach(DeactivateButton);

			buttonsInUse.Clear();
			buttonsActions.Clear();
		}

		/// <summary>
		/// Deactivates the button.
		/// </summary>
		/// <param name="button">Button.</param>
		protected virtual void DeactivateButton(KeyValuePair<string,Button> button)
		{
			button.Value.gameObject.SetActive(false);
			button.Value.onClick.RemoveListener(buttonsActions[button.Key]);
			buttonsCache.Push(button.Value);
		}

		/// <summary>
		/// Resets the parametres.
		/// </summary>
		protected virtual void ResetParametres()
		{
			var template = Templates.Get(TemplateName);

			var title = template.TitleText!=null ? template.TitleText.text : "";
			var content = template.ContentText!=null ? template.ContentText.text : "";
			var icon = template.Icon!=null ? template.Icon.sprite : null;

			SetInfo(title, content, icon);
		}

		/// <summary>
		/// Default function to close dialog.
		/// </summary>
		static public bool Close()
		{
			return true;
		}
	}
}