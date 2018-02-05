using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TestColorPickerRange : MonoBehaviour
	{
		[SerializeField]
		ColorPickerRange ColorRange;

		[SerializeField]
		Image Image;

		void Start()
		{
			ColorRange.OnChange.AddListener(ColorChanged);

			ColorChanged(ColorRange.Color);
		}

		void ColorChanged(Color32 color)
		{
			Image.color = color;
		}

		public void TestGreen()
		{
			ColorRange.Color = Color.green;
		}

		public void TestGray()
		{
			ColorRange.Color = Color.gray;
		}

		public void TestColor()
		{
			ColorRange.Color = ColorRange.Color;
		}

		public void TestRG()
		{
			ColorRange.Color = new Color32(100, 50, 0, 255);
		}
	}
}

