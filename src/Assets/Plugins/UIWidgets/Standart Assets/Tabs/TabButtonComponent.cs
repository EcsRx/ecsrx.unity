using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets {
	/// <summary>
	/// Tab component.
	/// </summary>
	public class TabButtonComponent : TabButtonComponentBase {
		/// <summary>
		/// The name.
		/// </summary>
		[SerializeField]
		public Text Name;

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="tab">Tab.</param>
		public override void SetButtonData(Tab tab)
		{
			Name.text = tab.Name;
		}
	}
}