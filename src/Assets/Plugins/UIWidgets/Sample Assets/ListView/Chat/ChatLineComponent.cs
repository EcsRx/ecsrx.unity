using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

namespace UIWidgetsSamples {
	/// <summary>
	/// ChatLine component.
	/// </summary>
	public class ChatLineComponent : ListViewItem {
		// specify components for displaying item data
		[SerializeField]
		public Text UserName;
		
		[SerializeField]
		public Text Message;

		/// <summary>
		/// Display ChatLine.
		/// </summary>
		/// <param name="item">Item.</param>
		public void SetData(ChatLine item)
		{
			UserName.text = item.UserName;
			Message.text = item.Message;
			Utilites.UpdateLayout(GetComponent<LayoutGroup>());
		}
	}
}