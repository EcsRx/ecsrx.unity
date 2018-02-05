using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// Spinner validation.
	/// </summary>
	public enum SpinnerValidation {
		OnKeyDown = 0,
		OnEndInput = 1,
	}

	/// <summary>
	/// Spinner base class.
	/// </summary>
	abstract public class SpinnerBase<T> : InputField, IPointerDownHandler
		where T : struct
	{
		/// <summary>
		/// The min.
		/// </summary>
		[SerializeField]
		protected T _min;
		
		/// <summary>
		/// The max.
		/// </summary>
		[SerializeField]
		protected T _max;
		
		/// <summary>
		/// The step.
		/// </summary>
		[SerializeField]
		protected T _step;
		
		/// <summary>
		/// The value.
		/// </summary>
		[SerializeField]
		protected T _value;
		
		/// <summary>
		/// The validation type.
		/// </summary>
		[SerializeField]
		public SpinnerValidation Validation = SpinnerValidation.OnKeyDown;
		
		/// <summary>
		/// Delay of hold in seconds for permanent increase/descrease value.
		/// </summary>
		[SerializeField]
		public float HoldStartDelay = 0.5f;
		
		/// <summary>
		/// Delay of hold in seconds between increase/descrease value.
		/// </summary>
		[SerializeField]
		public float HoldChangeDelay = 0.1f;
		
		/// <summary>
		/// Gets or sets the minimum.
		/// </summary>
		/// <value>The minimum.</value>
		public T Min {
			get {
				return _min;
			}
			set {
				_min = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the maximum.
		/// </summary>
		/// <value>The maximum.</value>
		public T Max {
			get {
				return _max;
			}
			set {
				_max = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the step.
		/// </summary>
		/// <value>The step.</value>
		public T Step {
			get {
				return _step;
			}
			set {
				_step = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public T Value {
			get {
				return _value;
			}
			set {
				SetValue(value);
			}
		}
		
		/// <summary>
		/// The plus button.
		/// </summary>
		[SerializeField]
		protected ButtonAdvanced _plusButton;
		
		/// <summary>
		/// The minus button.
		/// </summary>
		[SerializeField]
		protected ButtonAdvanced _minusButton;
		
		/// <summary>
		/// Gets or sets the plus button.
		/// </summary>
		/// <value>The plus button.</value>
		public ButtonAdvanced PlusButton {
			get {
				return _plusButton;
			}
			set {
				if (_plusButton!=null)
				{
					_plusButton.onClick.RemoveListener(OnPlusClick);
					_plusButton.onPointerDown.RemoveListener(OnPlusButtonDown);
					_plusButton.onPointerUp.RemoveListener(OnPlusButtonUp);
				}
				_plusButton = value;
				if (_plusButton!=null)
				{
					_plusButton.onClick.AddListener(OnPlusClick);
					_plusButton.onPointerDown.AddListener(OnPlusButtonDown);
					_plusButton.onPointerUp.AddListener(OnPlusButtonUp);
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the minus button.
		/// </summary>
		/// <value>The minus button.</value>
		public ButtonAdvanced MinusButton {
			get {
				return _minusButton;
			}
			set {
				if (_minusButton!=null)
				{
					_minusButton.onClick.RemoveListener(OnMinusClick);
					_minusButton.onPointerDown.RemoveListener(OnMinusButtonDown);
					_minusButton.onPointerUp.RemoveListener(OnMinusButtonUp);
				}
				_minusButton = value;
				if (_minusButton!=null)
				{
					_minusButton.onClick.AddListener(OnMinusClick);
					_minusButton.onPointerDown.AddListener(OnMinusButtonDown);
					_minusButton.onPointerUp.AddListener(OnMinusButtonUp);
				}
			}
		}

		/// <summary>
		/// onPlusClick event.
		/// </summary>
		public UnityEvent onPlusClick = new UnityEvent();
		
		/// <summary>
		/// onMinusClick event.
		/// </summary>
		public UnityEvent onMinusClick = new UnityEvent();
		
		/// <summary>
		/// Increase value on step.
		/// </summary>
		abstract public void Plus();
		
		/// <summary>
		/// Decrease value on step.
		/// </summary>
		abstract public void Minus();

		/// <summary>
		/// Sets the value.
		/// </summary>
		/// <param name="newValue">New value.</param>
		abstract protected void SetValue(T newValue);

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected override void Start()
		{
			base.Start();
			
			onValidateInput = SpinnerValidate;
			
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			onValueChange.AddListener(ValueChange);
			#else
			onValueChanged.AddListener(ValueChange);
			#endif
			onEndEdit.AddListener(ValueEndEdit);
			
			PlusButton = _plusButton;
			MinusButton = _minusButton;
			Value = _value;

			SetTextValue();
		}

		protected virtual void SetTextValue()
		{
			#if UNITY_5_3_4 || UNITY_5_3_5 || UNITY_5_3_6 || UNITY_5_3_7 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6
			if (m_Text!=_value.ToString())
			{
				m_Text = _value.ToString();
				UpdateLabel();
			}
			#else
			text = _value.ToString();
			#endif
		}

		IEnumerator HoldPlus()
		{
			yield return new WaitForSeconds(HoldStartDelay);
			while (true)
			{
				Plus();
				yield return new WaitForSeconds(HoldChangeDelay);
			}
		}
		
		IEnumerator HoldMinus()
		{
			yield return new WaitForSeconds(HoldStartDelay);
			while (true)
			{
				Minus();
				yield return new WaitForSeconds(HoldChangeDelay);
			}
		}
		
		/// <summary>
		/// Raises the minus click event.
		/// </summary>
		public void OnMinusClick()
		{
			Minus();
			onMinusClick.Invoke();
		}
		
		/// <summary>
		/// Raises the plus click event.
		/// </summary>
		public void OnPlusClick()
		{
			Plus();
			onPlusClick.Invoke();
		}
		
		IEnumerator corutine;
		
		/// <summary>
		/// Raises the plus button down event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnPlusButtonDown(PointerEventData eventData)
		{
			if (corutine!=null)
			{
				StopCoroutine(corutine);
			}
			corutine = HoldPlus();
			StartCoroutine(corutine);
		}
		
		/// <summary>
		/// Raises the plus button up event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnPlusButtonUp(PointerEventData eventData)
		{
			StopCoroutine(corutine);
		}
		
		/// <summary>
		/// Raises the minus button down event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnMinusButtonDown(PointerEventData eventData)
		{
			if (corutine!=null)
			{
				StopCoroutine(corutine);
			}
			
			corutine = HoldMinus();
			StartCoroutine(corutine);
		}
		
		/// <summary>
		/// Raises the minus button up event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnMinusButtonUp(PointerEventData eventData)
		{
			StopCoroutine(corutine);
		}
		
		/// <summary>
		/// Ons the destroy.
		/// </summary>
		protected virtual void onDestroy()
		{
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			onValueChange.RemoveListener(ValueChange);
			#else
			onValueChanged.RemoveListener(ValueChange);
			#endif
			onEndEdit.RemoveListener(ValueEndEdit);
			
			PlusButton = null;
			MinusButton = null;
		}

		/// <summary>
		/// Called when value changed.
		/// </summary>
		/// <param name="value">Value.</param>
		abstract protected void ValueChange(string value);

		/// <summary>
		/// Called when end edit.
		/// </summary>
		/// <param name="value">Value.</param>
		abstract protected void ValueEndEdit(string value);
		
		char SpinnerValidate(string validateText, int charIndex, char addedChar)
		{
			if (SpinnerValidation.OnEndInput==Validation)
			{
				return ValidateShort(validateText, charIndex, addedChar);
			}
			else
			{
				return ValidateFull(validateText, charIndex, addedChar);
			}
		}

		/// <summary>
		/// Validate when key down for Validation=OnEndInput.
		/// </summary>
		/// <returns>The char.</returns>
		/// <param name="validateText">Validate text.</param>
		/// <param name="charIndex">Char index.</param>
		/// <param name="addedChar">Added char.</param>
		abstract protected char ValidateShort(string validateText, int charIndex, char addedChar);

		/// <summary>
		/// Validates when key down for Validation=OnKeyDown.
		/// </summary>
		/// <returns>The char.</returns>
		/// <param name="validateText">Validate text.</param>
		/// <param name="charIndex">Char index.</param>
		/// <param name="addedChar">Added char.</param>
		abstract protected char ValidateFull(string validateText, int charIndex, char addedChar);

		/// <summary>
		/// Clamps a value between a minimum and maximum value.
		/// </summary>
		/// <returns>The bounds.</returns>
		/// <param name="value">Value.</param>
		abstract protected T InBounds(T value);
	}
}