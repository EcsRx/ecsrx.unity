using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// DialogInfoBase.
	/// </summary>
	abstract public class DialogInfoBase : MonoBehaviour {

		/// <summary>
		/// Sets the info.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="icon">Icon.</param>
		public abstract void SetInfo(string title, string message, Sprite icon);
	}
}