using UnityEngine;
using System.Collections;
using System;
using UIWidgets;

namespace UIWidgetsSamples {

	/// <summary>
	/// Origin of chat line.
	/// </summary>
	public enum ChatLineType {
		System,
		User,
	}

	/// <summary>
	/// Chat line.
	/// </summary>
	[Serializable]
	public class ChatLine : IItemHeight {
		[SerializeField]
		public string UserName;
		[SerializeField]
		public string Message;
		[SerializeField]
		public DateTime Time;

		[SerializeField]
		public ChatLineType Type;

		public float Height {
			get;
			set;
		}
	}
}