using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIWidgets {
	/// <summary>
	/// Color picker input mode.
	/// None - Don't display sliders
	/// RGB - Display RGB sliders
	/// HSV - Display HSV sliders
	/// </summary>
	public enum ColorPickerInputMode {
		None = -1,
		RGB = 0,
		HSV = 1,
	}

	/// <summary>
	/// Color picker palette mode.
	/// Specified value used in vertical slider, others used in palette.
	/// None - None.
	/// Red - Red.
	/// Green - Green.
	/// Blue - Blue.
	/// Hue - Hue.
	/// Saturation - Saturation.
	/// Value - Value.
	/// </summary>
	public enum ColorPickerPaletteMode {
		None = -1,
		Red = 0,
		Green = 1,
		Blue = 2,
		Hue = 3,
		Saturation = 4,
		Value = 5,
	}

	/// <summary>
	/// Color RGB changed event.
	/// </summary>
	[System.Serializable]
	public class ColorRGBChangedEvent : UnityEvent<Color32> {
	}

	/// <summary>
	/// Color HSV changed event.
	/// </summary>
	[System.Serializable]
	public class ColorHSVChangedEvent : UnityEvent<ColorHSV> {
	}

	/// <summary>
	/// Color alpha changed event.
	/// </summary>
	[System.Serializable]
	public class ColorAlphaChangedEvent : UnityEvent<byte> {
	}

	/// <summary>
	/// ColorPicker.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ColorPicker")]
	public class ColorPicker : MonoBehaviour {
		/// <summary>
		/// Value limit in HSV gradients.
		/// </summary>
		public const int ValueLimit = 80;

		[SerializeField]
		ColorPickerRGBPalette rgbPalette;

		/// <summary>
		/// Gets or sets the RGB palette.
		/// </summary>
		/// <value>The RGB palette.</value>
		public ColorPickerRGBPalette RGBPalette {
			get {
				return rgbPalette;
			}
			set {
				if (rgbPalette!=null)
				{
					rgbPalette.OnChangeRGB.RemoveListener(ColorRGBChanged);
				}
				rgbPalette = value;
				if (rgbPalette!=null)
				{
					rgbPalette.OnChangeRGB.AddListener(ColorRGBChanged);
				}
			}
		}
		
		[SerializeField]
		ColorPickerRGBBlock rgbBlock;

		/// <summary>
		/// Gets or sets the RGB sliders block.
		/// </summary>
		/// <value>The RGB sliders block.</value>
		public ColorPickerRGBBlock RGBBlock {
			get {
				return rgbBlock;
			}
			set {
				if (rgbBlock!=null)
				{
					rgbBlock.OnChangeRGB.RemoveListener(ColorRGBChanged);
				}
				rgbBlock = value;
				if (rgbBlock!=null)
				{
					rgbBlock.OnChangeRGB.AddListener(ColorRGBChanged);
				}
			}
		}

		[SerializeField]
		ColorPickerHSVPalette hsvPalette;

		/// <summary>
		/// Gets or sets the HSV palette.
		/// </summary>
		/// <value>The HSV palette.</value>
		public ColorPickerHSVPalette HSVPalette {
			get {
				return hsvPalette;
			}
			set {
				if (hsvPalette!=null)
				{
					hsvPalette.OnChangeHSV.RemoveListener(ColorHSVChanged);
				}
				hsvPalette = value;
				if (hsvPalette!=null)
				{
					hsvPalette.OnChangeHSV.AddListener(ColorHSVChanged);
				}
			}
		}

		[SerializeField]
		ColorPickerHSVBlock hsvBlock;

		/// <summary>
		/// Gets or sets the HSV sliders block.
		/// </summary>
		/// <value>The HSV sliders block.</value>
		public ColorPickerHSVBlock HSVBlock {
			get {
				return hsvBlock;
			}
			set {
				if (hsvBlock!=null)
				{
					hsvBlock.OnChangeHSV.RemoveListener(ColorHSVChanged);
				}
				hsvBlock = value;
				if (hsvBlock!=null)
				{
					hsvBlock.OnChangeHSV.AddListener(ColorHSVChanged);
				}
			}
		}

		[SerializeField]
		ColorPickerABlock aBlock;

		/// <summary>
		/// Gets or sets Alpha slider block.
		/// </summary>
		/// <value>Alpha slider block.</value>
		public ColorPickerABlock ABlock {
			get {
				return aBlock;
			}
			set {
				if (aBlock!=null)
				{
					aBlock.OnChangeAlpha.RemoveListener(ColorAlphaChanged);
				}
				aBlock = value;
				if (aBlock!=null)
				{
					aBlock.OnChangeAlpha.AddListener(ColorAlphaChanged);
				}
			}
		}
		
		[SerializeField]
		ColorPickerColorBlock colorView;

		/// <summary>
		/// Gets or sets the color view.
		/// </summary>
		/// <value>The color view.</value>
		public ColorPickerColorBlock ColorView {
			get {
				return colorView;
			}
			set {
				colorView = value;
				if (colorView!=null)
				{
					colorView.SetColor(color);
				}
			}
		}

		[SerializeField]
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
				if (rgbPalette!=null)
				{
					rgbPalette.InputMode = inputMode;
				}
				if (hsvPalette!=null)
				{
					hsvPalette.InputMode = inputMode;
				}
				
				if (rgbBlock!=null)
				{
					rgbBlock.InputMode = inputMode;
				}
				if (hsvBlock!=null)
				{
					hsvBlock.InputMode = inputMode;
				}
				if (aBlock!=null)
				{
					aBlock.InputMode = inputMode;
				}
			}
		}

		[SerializeField]
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
				if (rgbPalette!=null)
				{
					rgbPalette.PaletteMode = paletteMode;
				}
				if (hsvPalette!=null)
				{
					hsvPalette.PaletteMode = paletteMode;
				}
				
				if (rgbBlock!=null)
				{
					rgbBlock.PaletteMode = paletteMode;
				}
				if (hsvBlock!=null)
				{
					hsvBlock.PaletteMode = paletteMode;
				}
				if (aBlock!=null)
				{
					aBlock.PaletteMode = paletteMode;
				}
			}
		}
		
		[SerializeField]
		Color32 color = new Color32(255, 255, 255, 255);

		/// <summary>
		/// Gets or sets the color32.
		/// </summary>
		/// <value>The color32.</value>
		public Color32 Color32 {
			get {
				return color;
			}
			set {
				color = value;
				UpdateBlocks(color);
				OnChange.Invoke(color);
			}
		}

		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		/// <value>The color.</value>
		public Color Color {
			get {
				return Color32;
			}
			set {
				Color32 = value;
			}
		}

		/// <summary>
		/// OnChange color event.
		/// </summary>
		public ColorRGBChangedEvent OnChange = new ColorRGBChangedEvent();

		/// <summary>
		/// Start this instance.
		/// </summary>
		public void Start()
		{
			if (rgbPalette!=null)
			{
				RGBPalette = rgbPalette;
				rgbPalette.Start();
			}

			if (hsvPalette!=null)
			{
				HSVPalette = hsvPalette;
				hsvPalette.Start();
			}
			
			if (rgbBlock!=null)
			{
				RGBBlock = rgbBlock;
				rgbBlock.Start();
			}

			if (hsvBlock!=null)
			{
				HSVBlock = hsvBlock;
				hsvBlock.Start();
			}

			if (aBlock!=null)
			{
				ABlock = aBlock;
				aBlock.Start();
			}
			InputMode = inputMode;
			PaletteMode = paletteMode;

			UpdateBlocks(color);
		}

		void OnEnable()
		{
			UpdateBlocks(color);
		}

		void ColorRGBChanged(Color32 newColor)
		{
			color = newColor;
			UpdateBlocks(color);

			OnChange.Invoke(color);
		}

		void ColorHSVChanged(ColorHSV newColor)
		{
			color = newColor;
			UpdateBlocks(newColor);

			OnChange.Invoke(color);
		}

		void ColorAlphaChanged(byte alpha)
		{
			color.a = alpha;
			if (aBlock!=null)
			{
				aBlock.SetColor(color);
			}
			if (colorView!=null)
			{
				colorView.SetColor(color);
			}

			OnChange.Invoke(color);
		}

		void UpdateBlocks(Color32 newColor)
		{
			if (colorView!=null)
			{
				colorView.SetColor(newColor);
			}

			if (rgbPalette!=null)
			{
				rgbPalette.SetColor(newColor);
			}
			if (hsvPalette!=null)
			{
				hsvPalette.SetColor(newColor);
			}

			if (rgbBlock!=null)
			{
				rgbBlock.SetColor(newColor);
			}
			if (hsvBlock!=null)
			{
				hsvBlock.SetColor(newColor);  
			}
			if (aBlock!=null)
			{
				aBlock.SetColor(newColor);
			}
		}

		void UpdateBlocks(ColorHSV newColor)
		{
			if (colorView!=null)
			{
				colorView.SetColor(newColor);
			}

			if (rgbPalette!=null)
			{
				rgbPalette.SetColor(newColor);
			}
			if (hsvPalette!=null)
			{
				hsvPalette.SetColor(newColor);
			}
			if (hsvBlock!=null)
			{
				hsvBlock.SetColor(newColor);  
			}
			if (rgbBlock!=null)
			{
				rgbBlock.SetColor(newColor);
			}
			if (aBlock!=null)
			{
				aBlock.SetColor(newColor);
			}
		}

		/// <summary>
		/// Toggles the input mode.
		/// </summary>
		public void ToggleInputMode()
		{
			InputMode = (InputMode==ColorPickerInputMode.RGB)
				? ColorPickerInputMode.HSV
				: ColorPickerInputMode.RGB;
		}

		static List<ColorPickerPaletteMode> rgbPaletteModes = new List<ColorPickerPaletteMode>() {
			ColorPickerPaletteMode.Red,
			ColorPickerPaletteMode.Green,
			ColorPickerPaletteMode.Blue,
		};
		static List<ColorPickerPaletteMode> hsvPaletteModes = new List<ColorPickerPaletteMode>() {
			ColorPickerPaletteMode.Hue,
			ColorPickerPaletteMode.Saturation,
			ColorPickerPaletteMode.Value,
		};

		/// <summary>
		/// Toggles the palette mode.
		/// </summary>
		public void TogglePaletteMode()
		{
			var paletteModes = new List<ColorPickerPaletteMode>();
			if (rgbPalette!=null)
			{
				paletteModes.AddRange(rgbPaletteModes);
			}
			if (hsvPalette!=null)
			{
				paletteModes.AddRange(hsvPaletteModes);
			}
			if (paletteModes.Count==0)
			{
				return ;
			}

			var next_index = paletteModes.IndexOf(PaletteMode) + 1;
			if (next_index==paletteModes.Count)
			{
				next_index = 0;
			}

			PaletteMode = paletteModes[next_index];
		}

		void OnDestroy()
		{
			RGBPalette = null;
			HSVPalette = null;

			RGBBlock = null;
			HSVBlock = null;

			ABlock = null;

			ColorView = null;
		}
	}
}