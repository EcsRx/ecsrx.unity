using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// NotifyInfoBase.
	/// </summary>
	abstract public class NotifyInfoBase : MonoBehaviour {

		/// <summary>
		/// Sets the info.
		/// </summary>
		/// <param name="message">Message.</param>
		public abstract void SetInfo(string message);
	}
}