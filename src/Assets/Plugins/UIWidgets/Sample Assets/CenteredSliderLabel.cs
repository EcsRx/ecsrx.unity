using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples
{
	[RequireComponent(typeof(CenteredSlider))]
	public class CenteredSliderLabel : MonoBehaviour
	{
		[SerializeField]
		Text label;

		CenteredSlider slider;

		void Start()
		{
			slider = GetComponent<CenteredSlider>();
			if (slider!=null)
			{
				slider.OnValuesChange.AddListener(ValueChanged);
				ValueChanged(slider.Value);
			}
		}
		
		void ValueChanged(int value)
		{
			label.text = value.ToString();
		}

		void OnDestroy()
		{
			if (slider!=null)
			{
				slider.OnValuesChange.RemoveListener(ValueChanged);
			}
		}
	}
}