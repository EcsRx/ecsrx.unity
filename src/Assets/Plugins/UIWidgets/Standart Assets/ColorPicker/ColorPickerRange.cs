using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace UIWidgets {

	/// <summary>
	/// ColorPickerRange.
	/// Allow to select colors in range between specified colors.
	/// How to change to vertical: change slider direction and replace default shader with UIGradientShaderVLineRGB.
	/// </summary>
	public class ColorPickerRange : MonoBehaviour
	{
		[SerializeField]
		Slider slider;

		/// <summary>
		/// Gets or sets the slider.
		/// </summary>
		/// <value>The slider.</value>
		public Slider Slider {
			get {
				return slider;
			}
			set {
				SetSlider(value);
			}
		}
		
		[SerializeField]
		Image sliderBackground;

		/// <summary>
		/// Gets or sets the Blue slider background.
		/// </summary>
		/// <value>The Blue slider background.</value>
		public Image SliderBackground {
			get {
				return sliderBackground;
			}
			set {
				sliderBackground = value;
				UpdateMaterial();
			}
		}
		
		[SerializeField]
		Shader defaultShader;

		/// <summary>
		/// Gets or sets the default shader to display gradients for sliders background.
		/// </summary>
		/// <value>The default shader.</value>
		public Shader DefaultShader {
			get {
				return defaultShader;
			}
			set {
				defaultShader = value;
				UpdateMaterial();
			}
		}

		[SerializeField]
		Color colorLeft = Color.black;

		/// <summary>
		/// Gets or sets the color on left side (or bottom side).
		/// </summary>
		/// <value>The color.</value>
		public Color ColorLeft {
			get {
				return colorLeft;
			}
			set {
				colorLeft = value;
				UpdateView();
				ValueChanged();
			}
		}

		[SerializeField]
		Color colorRight = Color.white;

		/// <summary>
		/// Gets or sets the color on right side (or top side).
		/// </summary>
		/// <value>The color right.</value>
		public Color ColorRight {
			get {
				return colorRight;
			}
			set {
				colorRight = value;
				UpdateView();
				ValueChanged();
			}
		}

		[SerializeField]
		Color color = Color.white;

		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		/// <value>The color.</value>
		public Color Color {
			get {
				return color;
			}
			set {
				inUpdateMode = true;

				SetColor(value);
				UpdateView();
				OnChange.Invoke(color);

				inUpdateMode = false;
			}
		}

		/// <summary>
		/// OnChangeRGB event.
		/// </summary>
		[SerializeField]
		public ColorRGBChangedEvent OnChange = new ColorRGBChangedEvent();

		bool isStarted;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public virtual void Start()
		{
			if (isStarted)
			{
				return ;
			}
			isStarted = true;

			Slider = slider;
			Slider.minValue = 0;
			Slider.maxValue = 255;
			Slider.wholeNumbers = true;
			UpdateMaterial();
		}

		/// <summary>
		/// This function is called when the object becomes enabled and active.
		/// </summary>
		protected virtual void OnEnable()
		{
			UpdateMaterial();
		}

		/// <summary>
		/// Sets the slider.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetSlider(Slider value)
		{
			if (slider!=null)
			{
				slider.onValueChanged.RemoveListener(SliderValueChanged);
			}
			slider = value;
			if (slider!=null)
			{
				slider.onValueChanged.AddListener(SliderValueChanged);
			}
		}

		void SliderValueChanged(float value)
		{
			ValueChanged();
		}

		/// <summary>
		/// If in update mode?
		/// </summary>
		protected bool inUpdateMode;

		/// <summary>
		/// Difference between color components.
		/// </summary>
		protected List<float> Diffs = new List<float>();

		/// <summary>
		/// Sets the color.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetColor(Color value)
		{
			Diffs.Clear();

			if ((ColorRight.r - ColorLeft.r)!=0f)
			{
				Diffs.Add(Mathf.Abs((value.r - ColorLeft.r) / (ColorRight.r - ColorLeft.r)));
			}
			if ((ColorRight.g - ColorLeft.g)!=0f)
			{
				Diffs.Add(Mathf.Abs((value.g - ColorLeft.g) / (ColorRight.g - ColorLeft.g)));
			}
			if ((ColorRight.b - ColorLeft.b)!=0f)
			{
				Diffs.Add(Mathf.Abs((value.b - ColorLeft.b) / (ColorRight.b - ColorLeft.b)));
			}
			if ((ColorRight.a - ColorLeft.a)!=0f)
			{
				Diffs.Add(Mathf.Abs((value.a - ColorLeft.a) / (ColorRight.a - ColorLeft.a)));
			}

			var t = Diffs.Count==0 ? 1 : Diffs.Sum() / Diffs.Count;
			color = Color.Lerp(ColorLeft, ColorRight, t);
			Slider.value = t * (Slider.maxValue - Slider.minValue);
		}

		/// <summary>
		/// Values the changed.
		/// </summary>
		protected virtual void ValueChanged()
		{
			if (inUpdateMode)
			{
				return ;
			}
			color = Color.Lerp(ColorLeft, ColorRight, Slider.value / (Slider.maxValue - Slider.minValue));
			OnChange.Invoke(color);
		}

		/// <summary>
		/// Updates the material.
		/// </summary>
		protected virtual void UpdateMaterial()
		{
			if (defaultShader==null)
			{
				return ;
			}

			sliderBackground.material = new Material(defaultShader);

			UpdateViewReal();
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		protected virtual void UpdateView()
		{
			UpdateViewReal();

			slider.gameObject.SetActive(false);
			slider.gameObject.SetActive(true);
		}

		/// <summary>
		/// Updates the view real.
		/// </summary>
		protected virtual void UpdateViewReal()
		{
			inUpdateMode = true;

			SetColor(color);

			if (slider.direction==Slider.Direction.LeftToRight || slider.direction==Slider.Direction.RightToLeft)
			{
				sliderBackground.material.SetColor("_ColorLeft", ColorLeft);
				sliderBackground.material.SetColor("_ColorRight", ColorRight);
			}
			else
			{
				sliderBackground.material.SetColor("_ColorBottom", ColorLeft);
				sliderBackground.material.SetColor("_ColorTop", ColorRight);
			}

			inUpdateMode = false;
		}
	}
}

