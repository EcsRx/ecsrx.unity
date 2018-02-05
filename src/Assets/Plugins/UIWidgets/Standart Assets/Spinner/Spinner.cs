using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// On change event.
	/// </summary>
	[Serializable]
	public class OnChangeEventInt : UnityEvent<int>
	{

	}

	/// <summary>
	/// Submit event.
	/// </summary>
	[Serializable]
	public class SubmitEventInt : UnityEvent<int>
	{

	}

	/// <summary>
	/// Spinner.
	/// http://ilih.ru/images/unity-assets/UIWidgets/Spinner.png
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/Spinner")]
	public class Spinner : SpinnerBase<int>
	{
		/// <summary>
		/// onValueChange event.
		/// </summary>
		public OnChangeEventInt onValueChangeInt = new OnChangeEventInt();

		/// <summary>
		/// onEndEdit event.
		/// </summary>
		public SubmitEventInt onEndEditInt = new SubmitEventInt();

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public Spinner()
		{
			_max = 100;
			_step = 1;
		}

		/// <summary>
		/// Increase value on step.
		/// </summary>
		public override void Plus()
		{
			if ((Value <= 0) || (int.MaxValue - Value) >= Step)
			{
				Value += Step;
			}
			else
			{
				Value = int.MaxValue;
			}
		}

		/// <summary>
		/// Decrease value on step.
		/// </summary>
		public override void Minus()
		{
			if ((Value >= 0) || (Mathf.Abs(int.MinValue - Value)) >= Step)
			{
				Value -= Step;
			}
			else
			{
				Value = int.MinValue;
			}
		}

		/// <summary>
		/// Sets the value.
		/// </summary>
		/// <param name="newValue">New value.</param>
		protected override void SetValue(int newValue)
		{
			if (_value==InBounds(newValue))
			{
				return ;
			}
			_value = InBounds(newValue);

			SetTextValue();
			onValueChangeInt.Invoke(_value);
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

		protected void ParseValue(string value)
		{
			int new_value;
			if (!int.TryParse(value, out new_value))
			{
				new_value = (value.Trim()[0]=='-') ? int.MinValue : int.MaxValue;
			}
			SetValue(new_value);
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
			onEndEditInt.Invoke(_value);
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
			if (charIndex != 0 || validateText.Length <= 0 || validateText [0] != '-')
			{
				if (addedChar >= '0' && addedChar <= '9')
				{
					return addedChar;
				}
				if (addedChar == '-' && charIndex == 0 && Min < 0)
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
			var number = validateText.Insert(charIndex, addedChar.ToString());

			if ((Min > 0) && (charIndex==0) && (addedChar=='-'))
			{
				return (char)0;
			}

			var min_parse_length = ((number.Length > 0) && (number[0]=='-')) ? 2 : 1;
			if (number.Length >= min_parse_length)
			{
				int new_value;
				if ((!int.TryParse(number, out new_value)))
				{
					return (char)0;
				}
				
				if (new_value!=InBounds(new_value))
				{
					return (char)0;
				}

				//SetValue(new_value);
			}

			return addedChar;
		}

		/// <summary>
		/// Clamps a value between a minimum and maximum value.
		/// </summary>
		/// <returns>The bounds.</returns>
		/// <param name="value">Value.</param>
		protected override int InBounds(int value)
		{
			return Mathf.Clamp(value, _min, _max);
		}
	}
}