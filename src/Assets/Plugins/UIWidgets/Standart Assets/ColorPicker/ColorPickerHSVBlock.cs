using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// Color picker HSV sliders block.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ColorPicker HSVBlock")]
	public class ColorPickerHSVBlock : MonoBehaviour {
		[SerializeField]
		Slider hSlider;

		/// <summary>
		/// Gets or sets the Hue slider.
		/// </summary>
		/// <value>The Hue slider.</value>
		public Slider HSlider {
			get {
				return hSlider;
			}
			set {
				SetHSlider(value);
			}
		}


		[SerializeField]
		Spinner hInput;

		/// <summary>
		/// Gets or sets the Hue input.
		/// </summary>
		/// <value>The Hue input.</value>
		public Spinner HInput {
			get {
				return hInput;
			}
			set {
				SetHInput(value);
			}
		}


		[SerializeField]
		Image hSliderBackground;

		/// <summary>
		/// Gets or sets the Hue slider background.
		/// </summary>
		/// <value>The Hue slider background.</value>
		public Image HSliderBackground {
			get {
				return hSliderBackground;
			}
			set {
				hSliderBackground = value;
				UpdateMaterial();
			}
		}

		[SerializeField]
		Slider sSlider;

		/// <summary>
		/// Gets or sets the Saturation slider.
		/// </summary>
		/// <value>The Saturation slider.</value>
		public Slider SSlider {
			get {
				return sSlider;
			}
			set {
				SetSSlider(value);
			}
		}

		[SerializeField]
		Spinner sInput;

		/// <summary>
		/// Gets or sets the Saturation input.
		/// </summary>
		/// <value>The Saturation input.</value>
		public Spinner SInput {
			get {
				return sInput;
			}
			set {
				SetSInput(value);
			}
		}
		
		[SerializeField]
		Image sSliderBackground;

		/// <summary>
		/// Gets or sets the Saturation slider background.
		/// </summary>
		/// <value>The Saturation slider background.</value>
		public Image SSliderBackground {
			get {
				return sSliderBackground;
			}
			set {
				sSliderBackground = value;
				UpdateMaterial();
			}
		}

		[SerializeField]
		Slider vSlider;

		/// <summary>
		/// Gets or sets the Value slider.
		/// </summary>
		/// <value>The Value slider.</value>
		public Slider VSlider {
			get {
				return vSlider;
			}
			set {
				SetVSlider(value);
			}
		}
		
		[SerializeField]
		Spinner vInput;

		/// <summary>
		/// Gets or sets the Value input.
		/// </summary>
		/// <value>The Value input.</value>
		public Spinner VInput {
			get {
				return vInput;
			}
			set {
				SetVInput(value);
			}
		}


		[SerializeField]
		Image vSliderBackground;

		/// <summary>
		/// Gets or sets the Value slider background.
		/// </summary>
		/// <value>The Value slider background.</value>
		public Image VSliderBackground {
			get {
				return vSliderBackground;
			}
			set {
				vSliderBackground = value;
				UpdateMaterial();
			}
		}
		
		[SerializeField]
		Shader defaultShader;

		/// <summary>
		/// Gets or sets the default shader to display gradients in sliders backgrounds.
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
		ColorPickerInputMode inputMode;

		/// <summary>
		/// Gets or sets the input mode.
		/// </summary>
		/// <value>The input mode.</value>
		public ColorPickerInputMode InputMode {
			get {
				return inputMode;
			}
			set {
				inputMode = value;

				gameObject.SetActive(inputMode==ColorPickerInputMode.HSV);
				UpdateView();
			}
		}
		
		ColorPickerPaletteMode paletteMode;

		/// <summary>
		/// Gets or sets the palette mode.
		/// </summary>
		/// <value>The palette mode.</value>
		public ColorPickerPaletteMode PaletteMode {
			get {
				return paletteMode;
			}
			set {
				paletteMode = value;
			}
		}

		/// <summary>
		/// OnChangeRGB event.
		/// </summary>
		public ColorRGBChangedEvent OnChangeRGB = new ColorRGBChangedEvent();

		/// <summary>
		/// OnChangeHSV event.
		/// </summary>
		public ColorHSVChangedEvent OnChangeHSV = new ColorHSVChangedEvent();

		/// <summary>
		/// OnChangeAlpha event.
		/// </summary>
		public ColorAlphaChangedEvent OnChangeAlpha = new ColorAlphaChangedEvent();

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

			HSlider = hSlider;
			HInput = hInput;
			HSliderBackground = hSliderBackground;

			SSlider = sSlider;
			SInput = sInput;
			SSliderBackground = sSliderBackground;

			VSlider = vSlider;
			VInput = vInput;
			VSliderBackground = vSliderBackground;
		}

		/// <summary>
		/// Sets the Hue slider.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetHSlider(Slider value)
		{
			if (hSlider!=null)
			{
				hSlider.onValueChanged.RemoveListener(SliderValueChanged);
			}
			hSlider = value;
			if (hSlider!=null)
			{
				hSlider.onValueChanged.AddListener(SliderValueChanged);
			}
		}

		/// <summary>
		/// Sets the Hue input.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetHInput(Spinner value)
		{
			if (hInput!=null)
			{
				hInput.onValueChangeInt.RemoveListener(SpinnerValueChanged);
			}
			hInput = value;
			if (hInput!=null)
			{
				hInput.onValueChangeInt.AddListener(SpinnerValueChanged);
			}
		}

		/// <summary>
		/// Sets the Brightness slider.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetVSlider(Slider value)
		{
			if (vSlider!=null)
			{
				vSlider.onValueChanged.RemoveListener(SliderValueChanged);
			}
			vSlider = value;
			if (vSlider!=null)
			{
				vSlider.onValueChanged.AddListener(SliderValueChanged);
			}
		}

		/// <summary>
		/// Sets the Brightness input.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetVInput(Spinner value)
		{
			if (vInput!=null)
			{
				vInput.onValueChangeInt.RemoveListener(SpinnerValueChanged);
			}
			vInput = value;
			if (vInput!=null)
			{
				vInput.onValueChangeInt.AddListener(SpinnerValueChanged);
			}
		}

		/// <summary>
		/// Sets the Saturation slider.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetSSlider(Slider value)
		{
			if (sSlider!=null)
			{
				sSlider.onValueChanged.RemoveListener(SliderValueChanged);
			}
			sSlider = value;
			if (sSlider!=null)
			{
				sSlider.onValueChanged.AddListener(SliderValueChanged);
			}
		}

		/// <summary>
		/// Sets the Saturation input.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetSInput(Spinner value)
		{
			if (sInput!=null)
			{
				sInput.onValueChangeInt.RemoveListener(SpinnerValueChanged);
			}
			sInput = value;
			if (sInput!=null)
			{
				sInput.onValueChangeInt.AddListener(SpinnerValueChanged);
			}
		}

		/// <summary>
		/// This function is called when the object becomes enabled and active.
		/// </summary>
		protected virtual void OnEnable()
		{
			UpdateMaterial();
		}
		
		void SpinnerValueChanged(int value)
		{
			ValueChanged(isSlider: false);
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
		/// Values the changed.
		/// </summary>
		/// <param name="isSlider">Is slider value changed?</param>
		protected virtual void ValueChanged(bool isSlider=true)
		{
			if (inUpdateMode)
			{
				return ;
			}
			currentColorHSV = new ColorHSV(
				GetHue(isSlider),
				GetSaturation(isSlider),
				GetBrightness(isSlider),
				currentColorHSV.A
			);

			OnChangeHSV.Invoke(currentColorHSV);
		}

		/// <summary>
		/// Gets the hue.
		/// </summary>
		/// <param name="isSlider">Is slider value changed?</param>
		/// <returns>The hue.</returns>
		protected int GetHue(bool isSlider=true)
		{
			if ((hSlider!=null) && (isSlider))
			{
				return (int)hSlider.value;
			}
			if (hInput!=null)
			{
				return hInput.Value;
			}
			return currentColorHSV.H;
		}

		/// <summary>
		/// Gets the saturation.
		/// </summary>
		/// <param name="isSlider">Is slider value changed?</param>
		/// <returns>The saturation.</returns>
		protected int GetSaturation(bool isSlider=true)
		{
			if ((sSlider!=null) && (isSlider))
			{
				return (int)sSlider.value;
			}
			if (sInput!=null)
			{
				return sInput.Value;
			}
			return currentColorHSV.S;
		}

		/// <summary>
		/// Gets the brightness.
		/// </summary>
		/// <param name="isSlider">Is slider value changed?</param>
		/// <returns>The brightness.</returns>
		protected int GetBrightness(bool isSlider=true)
		{
			if ((vSlider!=null) && (isSlider))
			{
				return (int)vSlider.value;
			}
			if (vInput!=null)
			{
				return vInput.Value;
			}
			return currentColorHSV.V;
		}

		/// <summary>
		/// Current color.
		/// </summary>
		protected ColorHSV currentColorHSV;

		/// <summary>
		/// Sets the color.
		/// </summary>
		/// <param name="color">Color.</param>
		public void SetColor(Color32 color)
		{
			currentColorHSV = new ColorHSV(color);

			UpdateView();
		}

		/// <summary>
		/// Sets the color.
		/// </summary>
		/// <param name="color">Color.</param>
		public void SetColor(ColorHSV color)
		{
			currentColorHSV = color;

			UpdateView();
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		protected virtual void UpdateView()
		{
			UpdateViewReal();

			#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6
			hSlider.gameObject.SetActive(false);
			hSlider.gameObject.SetActive(true);

			sSlider.gameObject.SetActive(false);
			sSlider.gameObject.SetActive(true);

			vSlider.gameObject.SetActive(false);
			vSlider.gameObject.SetActive(true);
			#endif
		}

		/// <summary>
		/// Updates the view real.
		/// </summary>
		protected virtual void UpdateViewReal()
		{
			inUpdateMode = true;

			if (hSlider!=null)
			{
				hSlider.value = currentColorHSV.H;
			}
			if (hInput!=null)
			{
				hInput.Value = currentColorHSV.H;
			}
			
			if (sSlider!=null)
			{
				sSlider.value = currentColorHSV.S;
			}
			if (sInput!=null)
			{
				sInput.Value = currentColorHSV.S;
			}
			
			if (vSlider!=null)
			{
				vSlider.value = currentColorHSV.V;
			}
			if (vInput!=null)
			{
				vInput.Value = currentColorHSV.V;
			}

			var h = (byte)((currentColorHSV.H / 360f) * 255f);
			var s = (byte)currentColorHSV.S;
			var v = (byte)currentColorHSV.V;

			if (hSliderBackground!=null)
			{
				//hSliderBackground.material.SetColor("_ColorLeft", new Color32(0, s, v, 255));
				//hSliderBackground.material.SetColor("_ColorRight", new Color32(255, s, v, 255));
				hSliderBackground.material.SetColor("_ColorLeft", new Color32(0, 255, 255, 255));
				hSliderBackground.material.SetColor("_ColorRight", new Color32(255, 255, 255, 255));
			}

			if (sSliderBackground!=null)
			{
				sSliderBackground.material.SetColor("_ColorLeft", new Color32(h, 0, (byte)Mathf.Max(ColorPicker.ValueLimit, v), 255));
				sSliderBackground.material.SetColor("_ColorRight", new Color32(h, 255, (byte)Mathf.Max(ColorPicker.ValueLimit, v), 255));
			}
			
			if (vSliderBackground!=null)
			{
				vSliderBackground.material.SetColor("_ColorLeft", new Color32(h, s, 0, 255));
				vSliderBackground.material.SetColor("_ColorRight", new Color32(h, s, 255, 255));
			}
			
			inUpdateMode = false;
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

			if (hSliderBackground!=null)
			{
				hSliderBackground.material = new Material(defaultShader);
			}

			if (sSliderBackground!=null)
			{
				sSliderBackground.material = new Material(defaultShader);
			}

			if (vSliderBackground!=null)
			{
				vSliderBackground.material = new Material(defaultShader);
			}
			
			UpdateViewReal();
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			hSlider = null;
			hInput = null;
			
			sSlider = null;
			sInput = null;
			
			vSlider = null;
			vInput = null;
		}
	}
}