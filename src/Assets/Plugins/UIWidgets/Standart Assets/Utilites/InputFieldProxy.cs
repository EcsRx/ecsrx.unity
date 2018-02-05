using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets
{
	/// <summary>
	/// InputFieldProxy.
	/// </summary>
	public class InputFieldProxy : IInputFieldProxy
	{
		/// <summary>
		/// The InputField.
		/// </summary>
		InputField inputField;

		/// <summary>
		/// Initializes a new instance of the <see cref="UIWidgets.InputFieldProxy"/> class.
		/// </summary>
		/// <param name="input">Input.</param>
		public InputFieldProxy(InputField input)
		{
			inputField = input;
		}

		#region IInputFieldProxy implementation
		/// <summary>
		/// The current value of the input field.
		/// </summary>
		/// <value>The text.</value>
		public string text {
			get {
				return inputField.text;
			}
			set {
				inputField.text = value;
			}
		}

		/// <summary>
		/// Accessor to the OnChangeEvent.
		/// </summary>
		/// <value>The OnValueChange.</value>
		public InputField.OnChangeEvent onValueChange {
			get {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				return inputField.onValueChange;
				#else
				return inputField.onValueChanged;
				#endif
			}
			set {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				inputField.onValueChange = value;
				#else
				inputField.onValueChanged = value;
				#endif
			}
		}

		/// <summary>
		/// Accessor to the OnChangeEvent.
		/// </summary>
		/// <value>The OnValueChange.</value>
		public InputField.OnChangeEvent onValueChanged {
			get {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				return inputField.onValueChange;
				#else
				return inputField.onValueChanged;
				#endif
			}
			set {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				inputField.onValueChange = value;
				#else
				inputField.onValueChanged = value;
				#endif
			}
		}

		/// <summary>
		/// The Unity Event to call when editing has ended.
		/// </summary>
		/// <value>The OnEndEdit.</value>
		public InputField.SubmitEvent onEndEdit {
			get {
				return inputField.onEndEdit;
			}
			set {
				inputField.onEndEdit = value;
			}
		}

		/// <summary>
		/// Current InputField caret position (also selection tail).
		/// </summary>
		/// <value>The caret position.</value>
		public int caretPosition {
			get {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
				return inputField.text.Length;
				#else
				return inputField.caretPosition;
				#endif
			}
			set {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
				//inputField.ActivateInputField();
				#else
				inputField.caretPosition = value;
				#endif
			}
		}

		public bool interactable {
			get {
				return inputField.interactable;
			}
			set {
				inputField.interactable = value;
			}
		}
		#endregion
	}
}

