using UnityEngine;
using UnityEngine.UI;
using System;

namespace UIWidgetsSamples {
	/// <summary>
	/// ChatView test script.
	/// </summary>
	public class ChatViewTest : MonoBehaviour {
		/// <summary>
		/// ChatView.
		/// </summary>
		[SerializeField]
		public ChatView Chat;

		/// <summary>
		/// The message.
		/// </summary>
		[SerializeField]
		public InputField Message;

		/// <summary>
		/// The name of the user.
		/// </summary>
		[SerializeField]
		public InputField UserName;

		/// <summary>
		/// Sends the message.
		/// </summary>
		public void SendMessage()
		{
			// check username and message
			if ((UserName.text.Trim()=="") || Message.text.Trim()=="")
			{
				return ;
			}

			// add new message to chat
			Chat.DataSource.Add(new ChatLine(){
				UserName = UserName.text,
				Message = Message.text,
				Time = DateTime.Now,
				Type = (UserName.text=="System") ? ChatLineType.System : ChatLineType.User,
			});

			Message.text = "";

			// scroll to end
			Chat.ScrollRect.verticalNormalizedPosition = 0f;
		}
	}
}