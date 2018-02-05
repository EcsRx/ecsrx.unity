using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Globalization;

namespace UIWidgets {
	/// <summary>
	/// On change event.
	/// </summary>
	[Serializable]
	public class OnChangeEventFloat : UnityEvent<float> {
	}

	/// <summary>
	/// Submit event.
	/// </summary>
	[Serializable]
	public class SubmitEventFloat : UnityEvent<float> {
	}
	
	/// <summary>
	/// Spinner with float value.
	/// Warning: incompatible types with different Unity versions - Unity 4.x use string[] and Unity 5.x use char[]
	/// http://ilih.ru/images/unity-assets/UIWidgets/Spinner.png
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/SpinnerFloat")]
	public class SpinnerFloat : SpinnerBase<float> {
		[SerializeField]
		string format = "0.00";

		/// <summary>
		/// Allowed decimal separators.
		/// Warning: incompatible types with different Unity versions - Unity 4.x use string[] and Unity 5.x use char[]
		/// </summary>
		[SerializeField]
		[Tooltip("Allowed decimal separators.")]
		#if UNITY_4_6 || UNITY_4_7
		public string[] DecimalSeparators = new string[] {".", ","};
		#else
		public char[] DecimalSeparators = new char[] {'.', ','};
		#endif

		/// <summary>
		/// Gets or sets the format.
		/// </summary>
		/// <value>The format.</value>
		public string Format {
			get {
				return format;
			}
			set {
				format = value;
				SetTextValue();
			}
		}

		/// <summary>
		/// onValueChange event.
		/// </summary>
		public OnChangeEventFloat onValueChangeFloat = new OnChangeEventFloat();
		
		/// <summary>
		/// onEndEdit event.
		/// </summary>
		public SubmitEventFloat onEndEditFloat = new SubmitEventFloat();

		NumberStyles numberStyle = NumberStyles.AllowDecimalPoint
			| NumberStyles.AllowThousands
			| NumberStyles.AllowLeadingSign;

		public NumberStyles NumberStyle {
			get {
				return numberStyle;
			}
			set {
				numberStyle = value;
			}
		}

		CultureInfo culture = CultureInfo.InvariantCulture;

		public CultureInfo Culture {
			get {
				return culture;
			}
			set {
				culture = value;
				SetTextValue();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UIWidgets.SpinnerFloat"/> class.
		/// </summary>
		public SpinnerFloat()
		{
			_max = 100f;
			_step = 1f;
		}

		/// <summary>
		/// Increase value on step.
		/// </summary>
		public override void Plus()
		{
			if ((Value <= 0) || (float.MaxValue - Value) >= Step)
			{
				Value += Step;
			}
			else
			{
				Value = float.MaxValue;
			}
		}
		
		/// <summary>
		/// Decrease value on step.
		/// </summary>
		public override void Minus()
		{
			if ((Value >= 0) || (Mathf.Abs(float.MinValue - Value)) >= Step)
			{
				Value -= Step;
			}
			else
			{
				Value = float.MinValue;
			}
		}

		/// <summary>
		/// Sets the value.
		/// </summary>
		/// <param name="newValue">New value.</param>
		protected override void SetValue(float newValue)
		{
			if (_value==InBounds(newValue))
			{
				return ;
			}
			_value = InBounds(newValue);

			SetTextValue();
			onValueChangeFloat.Invoke(_value);
		}

		protected override void SetTextValue()
		{
			#if UNITY_5_3_4 || UNITY_5_3_5 || UNITY_5_3_6 || UNITY_5_3_7 || UNITY_5_4 || UNITY_5_5 || UNITY_5_5 || UNITY_5_6
			if (m_Text!=_value.ToString(format, culture))
			{
				m_Text = _value.ToString(format, culture);
				UpdateLabel();
			}
			#else
			text = _value.ToString(format, Culture);
			#endif
		}

		/// <summary>
		/// Called when value changed.
		/// </summary>
		/// <param name="value">Value.</param>
		protected override void ValueChange(string value)
		{
			if (SpinnerValidation.OnEndInput==Validation)
			{
				return ;
			}
			if (value.Length==0)
			{
				value = "0";
			}
			ParseValue(value);
		}

		/// <summary>
		/// Called when end edit.
		/// </summary>
		/// <param name="value">Value.</param>
		protected override void ValueEndEdit(string value)
		{
			if (value.Length==0)
			{
				value = "0";
			}
			ParseValue(value);
			onEndEditFloat.Invoke(_value);
		}

		protected void ParseValue(string value)
		{
			float new_value;
			if (!float.TryParse(value, NumberStyle, culture, out new_value))
			{
				new_value = (value.Trim()[0]=='-') ? float.MinValue : float.MaxValue;
			}
			SetValue(new_value);
		}

		/// <summary>
		/// Validate when key down for Validation=OnEndInput.
		/// </summary>
		/// <returns>The char.</returns>
		/// <param name="validateText">Validate text.</param>
		/// <param name="charIndex">Char index.</param>
		/// <param name="addedChar">Added char.</param>
		protected override char ValidateShort(string validateText, int charIndex, char addedChar)
		{
			#if UNITY_4_6 || UNITY_4_7
			var replace_decimal_separator = Array.IndexOf(DecimalSeparators, addedChar.ToString())!=-1;
			#else
			var replace_decimal_separator = Array.IndexOf(DecimalSeparators, addedChar)!=-1;
			#endif
			if (replace_decimal_separator)
			{
				addedChar = culture.NumberFormat.NumberDecimalSeparator[0];
			}

			if (charIndex != 0 || validateText.Length <= 0 || validateText [0] != culture.NumberFormat.NegativeSign[0])
			{
				if ((addedChar >= '0' && addedChar <= '9'))// || (Array.IndexOf(culture.NumberFormat.NativeDigits, addedChar.ToString())!=-1))
				{
					return addedChar;
				}
				if (addedChar == culture.NumberFormat.NegativeSign[0] && charIndex == 0 && Min < 0)
				{
					return addedChar;
				}
				char decimal_separator = culture.NumberFormat.NumberDecimalSeparator[0];
				if (addedChar == decimal_separator && !validateText.Contains(decimal_separator.ToString()))
				{
					return addedChar;
				}
			}
			return '\0';
		}

		/// <summary>
		/// Validates when key down for Validation=OnKeyDown.
		/// </summary>
		/// <returns>The char.</returns>
		/// <param name="validateText">Validate text.</param>
		/// <param name="charIndex">Char index.</param>
		/// <param name="addedChar">Added char.</param>
		protected override char ValidateFull(string validateText, int charIndex, char addedChar)
		{
			#if UNITY_4_6 || UNITY_4_7
			var replace_decimal_separator = Array.IndexOf(DecimalSeparators, addedChar.ToString())!=-1;
			#else
			var replace_decimal_separator = Array.IndexOf(DecimalSeparators, addedChar)!=-1;
			#endif
			if (replace_decimal_separator)
			{
				addedChar = culture.NumberFormat.NumberDecimalSeparator[0];
			}

			var number = validateText.Insert(charIndex, addedChar.ToString());
			
			if ((Min > 0) && (charIndex==0) && (addedChar==culture.NumberFormat.NegativeSign[0]))
			{
				return (char)0;
			}
			
			var min_parse_length = ((number.Length > 0) && (number[0]==culture.NumberFormat.NegativeSign[0])) ? 2 : 1;
			if (number.Length >= min_parse_length)
			{
				float new_value;
				if (!float.TryParse(number, NumberStyle, culture, out new_value))
				{
					return (char)0;
				}

				if (new_value!=InBounds(new_value))
				{
					return (char)0;
				}
				
				_value = new_value;
			}
			
			return addedChar;
		}

		/// <summary>
		/// Clamps a value between a minimum and maximum value.
		/// </summary>
		/// <returns>The bounds.</returns>
		/// <param name="value">Value.</param>
		protected override float InBounds(float value)
		{
			return Mathf.Clamp(value, _min, _max);
		}
	}
}