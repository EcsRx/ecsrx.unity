using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(RangeSliderFloat))]
	public class RangeSliderFloatSample : MonoBehaviour {
		[SerializeField]
		Text Text;

		RangeSliderFloat slider;

		void Start()
		{
			slider = GetComponent<RangeSliderFloat>();
			if (slider!=null)
			{
				slider.OnValuesChange.AddListener(SliderChanged);
				SliderChanged(slider.ValueMin, slider.ValueMax);
			}
		}
		
		void SliderChanged(float min, float max)
		{
			if (Text!=null)
			{
				if (slider.WholeNumberOfSteps)
				{
					Text.text = string.Format("Range: {0:000.00} - {1:000.00};  Step: {2:0.00}", min, max, slider.Step);
				}
				else
				{
					Text.text = string.Format("Range: {0:000.00} - {1:000.00}", min, max);
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