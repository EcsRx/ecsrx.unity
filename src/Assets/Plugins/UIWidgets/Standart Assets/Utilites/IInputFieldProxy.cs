using UnityEngine.UI;
using System;

namespace UIWidgets {
	/// <summary>
	/// IInputFieldProxy.
	/// </summary>
	public interface IInputFieldProxy {

		/// <summary>
		/// The current value of the input field.
		/// </summary>
		/// <value>The text.</value>
		string text {
			get;
			set;
		}

		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		#else
		[Obsolete("Use onValueChanged()")]
		#endif
		/// <summary>
		/// Accessor to the OnChangeEvent.
		/// </summary>
		/// <value>The OnValueChange.</value>
		InputField.OnChangeEvent onValueChange {
			get;
			set;
		}

		/// <summary>
		/// Accessor to the OnChangeEvent.
		/// </summary>
		/// <value>The OnValueChange.</value>
		InputField.OnChangeEvent onValueChanged {
			get;
			set;
		}

		/// <summary>
		/// The Unity Event to call when editing has ended.
		/// </summary>
		/// <value>The OnEndEdit.</value>
		InputField.SubmitEvent onEndEdit {
			get;
			set;
		}

		/// <summary>
		/// Current InputField caret position (also selection tail).
		/// </summary>
		/// <value>The caret position.</value>
		int caretPosition {
			get;
			set;
		}

		/// <summary>
		/// Is the InputField eligable for interaction (excludes canvas groups).
		/// </summary>
		/// <value><c>true</c> if interactable; otherwise, <c>false</c>.</value>
		bool interactable {
			get;
			set;
		}
	}
}