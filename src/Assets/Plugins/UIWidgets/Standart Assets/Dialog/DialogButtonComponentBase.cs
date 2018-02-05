using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// DialogInfoBase.
	/// </summary>
	abstract public class DialogButtonComponentBase : MonoBehaviour {

		/// <summary>
		/// Sets button label.
		/// </summary>
		/// <param name="name">Name.</param>
		public abstract void SetButtonName(string name);
	}
}