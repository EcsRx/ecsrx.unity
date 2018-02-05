using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets {

	/// <summary>
	/// Dialog info.
	/// </summary>
	public class DialogInfo : DialogInfoBase {
		/// <summary>
		/// The title.
		/// </summary>
		[SerializeField]
		public Text Title;

		/// <summary>
		/// The message.
		/// </summary>
		[SerializeField]
		public Text Message;

		/// <summary>
		/// The icon.
		/// </summary>
		[SerializeField]
		public Image Icon;

		/// <summary>
		/// Sets the info.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="icon">Icon.</param>
		public override void SetInfo(string title, string message, Sprite icon)
		{
			if (Title!=null)
			{
				Title.text = title;
			}
			if (Message!=null)
			{
				Message.text = message;
			}
			if (Icon!=null)
			{
				Icon.sprite = icon;
			}
		}

	}
}