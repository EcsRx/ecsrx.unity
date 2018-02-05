using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(RangeSlider))]
	public class RangeSliderSample : MonoBehaviour {
		[SerializeField]
		Text Text;

		RangeSlider slider;
		void Start()
		{
			slider = GetComponent<RangeSlider>();
			if (slider!=null)
			{
				slider.OnValuesChange.AddListener(SliderChanged);
				SliderChanged(slider.ValueMin, slider.ValueMax);
			}
		}

		void SliderChanged(int min, int max)
		{
			if (Text!=null)
			{
				if (slider.WholeNumberOfSteps)
				{
					Text.text = string.Format("Range: {0:000} - {1:000}; Step: {2}", min, max, slider.Step);
				}
				else
				{
					Text.text = string.Format("Range: {0:000} - {1:000}", min, max);
				}
			}
		}
		
		void OnDestroy()
		{
			if (slider!=null)
			{
				slider.OnValuesChange.RemoveListener(SliderChanged);
			}
		}
	}
}