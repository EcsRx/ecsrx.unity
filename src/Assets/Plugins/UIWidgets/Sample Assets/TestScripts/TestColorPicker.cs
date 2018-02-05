using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TestColorPicker : MonoBehaviour {
		[SerializeField]
		public ColorPicker ColorPicker;

		[SerializeField]
		public Image Image;

		public void UpdateColor()
		{
			if (Image==null)
			{
				return ;
			}
			if (ColorPicker==null)
			{
				return ;
			}
			Image.color = ColorPicker.Color;
		}

		public void SetColor()
		{
			ColorPicker.Color = Color.cyan;
		}
	}
}