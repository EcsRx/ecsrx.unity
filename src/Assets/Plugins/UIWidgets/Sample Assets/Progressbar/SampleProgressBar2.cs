using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(Button))]
	public class SampleProgressBar2 : MonoBehaviour {
		public Progressbar bar;

		Button button;
		// Use this for initialization
		void Start()
		{
			button = GetComponent<Button>();
			if (button!=null)
			{
				button.onClick.AddListener(Click);
			}
		}
		
		void Click()
		{
			if (bar.IsAnimationRun)
			{
				bar.Stop();
			}
			else
			{
				if (bar.Value==0)
				{
					bar.Animate(bar.Max);
				}
				else
				{
					bar.Animate(0);
				}
			}
		}

		void OnDestroy()
		{
			if (button!=null)
			{
				button.onClick.RemoveListener(Click);
			}
		}
	}
}