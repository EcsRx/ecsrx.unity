using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// Tab component.
	/// </summary>
	public abstract class TabButtonComponentBase : MonoBehaviour {

		/// <summary>
		/// Sets the button data.
		/// </summary>
		/// <param name="tab">Tab.</param>
		public abstract void SetButtonData(Tab tab);
	}
}