using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// Color picker RGB palette.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ColorPicker RGB Palette")]
	public class ColorPickerRGBPalette : MonoBehaviour {
		[SerializeField]
		Image palette;

		RectTransform paletteRect;
		OnDragListener dragListener;

		/// <summary>
		/// Gets or sets the palette.
		/// </summary>
		/// <value>The palette.</value>
		public Image Palette {
			get {
				return palette;
			}
			set {
				SetPalette(value);
			}
		}

		[SerializeField]
		Shader paletteShader;

		/// <summary>
		/// Gets or sets the shader to display gradient in palette.
		/// </summary>
		/// <value>The palette shader.</value>
		public Shader PaletteShader {
			get {
				return paletteShader;
			}
			set {
				paletteShader = value;
				UpdateMaterial();
			}
		}

		[SerializeField]
		RectTransform paletteCursor;

		/// <summary>
		/// Gets or sets the palette cursor.
		/// </summary>
		/// <value>The palette cursor.</value>
		public RectTransform PaletteCursor {
			get {
				return paletteCursor;
			}
			set {
				paletteCursor = value;
				if (paletteCursor!=null)
				{
					UpdateView();
				}
			}
		}

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
		/// Gets or sets the slider background.
		/// </summary>
		/// <value>The slider background.</value>
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
		Shader sliderShader;

		/// <summary>
		/// Gets or sets the shader to display gradient for slider background.
		/// </summary>
		/// <value>The slider shader.</value>
		public Shader SliderShader {
			get {
				return sliderShader;
			}
			set {
				sliderShader = value;
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
				SetPaletteMode(value);
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

			Palette = palette;
			Slider = slider;
			SliderBackground = sliderBackground;
		}

		/// <summary>
		/// This function is called when the object becomes enabled and active.
		/// </summary>
		protected virtual void OnEnable()
		{
			UpdateMaterial();
		}

		/// <summary>
		/// Sets the palette.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetPalette(Image value)
		{
			if (dragListener!=null)
			{
				dragListener.OnDragEvent.RemoveListener(OnDrag);
			}
			palette = value;
			if (palette!=null)
			{
				paletteRect = palette.transform as RectTransform;
				dragListener = palette.GetComponent<OnDragListener>();
				if (dragListener==null)
				{
					dragListener = palette.gameObject.AddComponent<OnDragListener>();
				}
				dragListener.OnDragEvent.AddListener(OnDrag);
				UpdateMaterial();
			}
			else
			{
				paletteRect = null;
			}
		}

		/// <summary>
		/// Sets the palette mode.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetPaletteMode(ColorPickerPaletteMode value)
		{
			paletteMode = value;
			var is_active = paletteMode==ColorPickerPaletteMode.Red
				|| paletteMode==ColorPickerPaletteMode.Green
					|| paletteMode==ColorPickerPaletteMode.Blue;
			gameObject.SetActive(is_active);
			if (is_active)
			{
				UpdateView();
			}
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
		/// Values the changed.
		/// </summary>
		protected virtual void ValueChanged()
		{
			if (inUpdateMode)
			{
				return ;
			}
			currentColor = GetColor();
			OnChangeRGB.Invoke(currentColor);
		}

		/// <summary>
		/// Current color.
		/// </summary>
		protected Color32 currentColor;

		/// <summary>
		/// Sets the color.
		/// </summary>
		/// <param name="color">Color.</param>
		public void SetColor(Color32 color)
		{
			currentColor = color;
			UpdateView();
		}

		/// <summary>
		/// Sets the color.
		/// </summary>
		/// <param name="color">Color.</param>
		public void SetColor(ColorHSV color)
		{
			currentColor = color;
			UpdateView();
		}

		/// <summary>
		/// When draging is occuring this will be called every time the cursor is moved.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void OnDrag(PointerEventData eventData)
		{
			Vector2 size = paletteRect.rect.size;
			Vector2 cur_pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(paletteRect, eventData.position, eventData.pressEventCamera, out cur_pos);

			cur_pos.x = Mathf.Clamp(cur_pos.x, 0, size.x);
			cur_pos.y = Mathf.Clamp(cur_pos.y, -size.y, 0);
			paletteCursor.localPosition = cur_pos;

			ValueChanged();
		}

		/// <summary>
		/// Gets the color.
		/// </summary>
		/// <returns>The color.</returns>
		protected Color32 GetColor()
		{
			var coords = GetCursorCoords();

			var s = (byte)slider.value;
			var x = (byte)Mathf.RoundToInt(coords.x);
			var y = (byte)Mathf.RoundToInt(coords.y);

			switch (paletteMode)
			{
				case ColorPickerPaletteMode.Red:
					return new Color32(s, y, x, currentColor.a);
				case ColorPickerPaletteMode.Green:
					return new Color32(y, s, x, currentColor.a);
				case ColorPickerPaletteMode.Blue:
					return new Color32(x, y, s, currentColor.a);
				default:
					return currentColor;
			}
		}

		/// <summary>
		/// Gets the cursor coords.
		/// </summary>
		/// <returns>The cursor coords.</returns>
		protected Vector2 GetCursorCoords()
		{
			var coords = paletteCursor.localPosition;
			var size = paletteRect.rect.size;
			
			var x = (coords.x / size.x) * 255;
			var y = ((coords.y / size.y) + 1) * 255;
			
			return new Vector2(x, y);
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		protected virtual void UpdateView()
		{
			UpdateViewReal();

			#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6
			Palette.gameObject.SetActive(false);
			Palette.gameObject.SetActive(true);
			#endif
		}

		/// <summary>
		/// Updates the view real.
		/// </summary>
		protected virtual void UpdateViewReal()
		{
			inUpdateMode = true;

			//set slider value
			if (slider!=null)
			{
				slider.value = GetSliderValue();
			}

			//set slider colors
			if (sliderBackground!=null)
			{
				var colors = GetSliderColors();

				sliderBackground.material.SetColor("_ColorBottom", colors[0]);
				sliderBackground.material.SetColor("_ColorTop", colors[1]);
			}

			//set palette drag position
			if ((paletteCursor!=null) && (palette!=null) && (paletteRect!=null))
			{
				var coords = GetPaletteCoords();
				var size = paletteRect.rect.size;

				paletteCursor.localPosition = new Vector3((coords.x /  255) * size.x, - (1 - coords.y /  255) * size.y, 0);
			}

			//set palette colors
			if (palette!=null)
			{
				var colors = GetPaletteColors();

				palette.material.SetColor("_ColorLeft", colors[0]);
				palette.material.SetColor("_ColorRight", colors[1]);
				palette.material.SetColor("_ColorBottom", colors[2]);
				palette.material.SetColor("_ColorTop", colors[3]);
			}

			inUpdateMode = false;
		}

		/// <summary>
		/// Gets the slider value.
		/// </summary>
		/// <returns>The slider value.</returns>
		protected int GetSliderValue()
		{
			switch (paletteMode)
			{
				case ColorPickerPaletteMode.Red:
					return currentColor.r;
				case ColorPickerPaletteMode.Green:
					return currentColor.g;
				case ColorPickerPaletteMode.Blue:
					return currentColor.b;
				default:
					return 0;
			}
		}

		/// <summary>
		/// Gets the slider colors.
		/// </summary>
		/// <returns>The slider colors.</returns>
		protected Color32[] GetSliderColors()
		{
			switch (paletteMode)
			{
				case ColorPickerPaletteMode.Red:
					return new Color32[] {
						new Color32(0, currentColor.g, currentColor.b, 255),
						new Color32(255, currentColor.g, currentColor.b, 255),
					};
				case ColorPickerPaletteMode.Green:
					return new Color32[] {
						new Color32(currentColor.r, 0, currentColor.b, 255),
						new Color32(currentColor.r, 255, currentColor.b, 255),
					};
				case ColorPickerPaletteMode.Blue:
					return new Color32[] {
						new Color32(currentColor.r, currentColor.g, 0, 255),
						new Color32(currentColor.r, currentColor.g, 255, 255),
					};
				default:
					return new Color32[] {
						new Color32(0, 0, 0, 255),
						new Color32(255, 255, 255, 255)
					};
			}
		}

		/// <summary>
		/// Gets the palette coords.
		/// </summary>
		/// <returns>The palette coords.</returns>
		protected Vector2 GetPaletteCoords()
		{
			switch (paletteMode)
			{
				case ColorPickerPaletteMode.Red:
					return new Vector2(currentColor.b, currentColor.g);
				case ColorPickerPaletteMode.Green:
					return new Vector2(currentColor.b, currentColor.r);
				case ColorPickerPaletteMode.Blue:
					return new Vector2(currentColor.r, currentColor.g);
				default:
					return new Vector2(0, 0);
			}
		}

		/// <summary>
		/// Gets the palette colors.
		/// </summary>
		/// <returns>The palette colors.</returns>
		protected Color[] GetPaletteColors()
		{
			switch (paletteMode)
			{
				case ColorPickerPaletteMode.Red:
					return new Color[] {
						new Color(currentColor.r / 255f / 2f, 0f, 0f, 1f),
						new Color(currentColor.r / 255f / 2f, 0f, 1f, 1f),
						new Color(currentColor.r / 255f / 2f, 0f, 0f, 1f),
						new Color(currentColor.r / 255f / 2f, 1f, 0f, 1f),
					};
				case ColorPickerPaletteMode.Green:
					return new Color[] {
						new Color(0f, currentColor.g / 255f / 2f, 0f, 1f),
						new Color(0f, currentColor.g / 255f / 2f, 1f, 1f),
						new Color(0f, currentColor.g / 255f / 2f, 0f, 1f),
						new Color(1f, currentColor.g / 255f / 2f, 0f, 1f),
					};
				case ColorPickerPaletteMode.Blue:
					return new Color[] {
						new Color(0f, 0f, currentColor.b / 255f / 2f, 1f),
						new Color(1f, 0f, currentColor.b / 255f / 2f, 1f),
						new Color(0f, 0f, currentColor.b / 255f / 2f, 1f),
						new Color(0f, 1f, currentColor.b / 255f / 2f, 1f),
					};
				default:
					return new Color[] {
						new Color(0f, 0f, 0f, 1f),
						new Color(1f, 1f, 1f, 1f),
						new Color(0f, 0f, 0f, 1f),
						new Color(1f, 1f, 1f, 1f),
					};
			}
		}

		/// <summary>
		/// Updates the material.
		/// </summary>
		protected virtual void UpdateMaterial()
		{
			if ((paletteShader!=null) && (palette!=null))
			{
				palette.material = new Material(paletteShader);
			}
			
			if ((sliderShader!=null) && (sliderBackground!=null))
			{
				sliderBackground.material = new Material(sliderShader);
			}

			UpdateViewReal();
		}

		/// <summary>
		/// This function is called when the object becomes enabled and active.
		/// </summary>
		protected virtual void OnDestroy()
		{
			dragListener = null;
			slider = null;
		}
	}
}