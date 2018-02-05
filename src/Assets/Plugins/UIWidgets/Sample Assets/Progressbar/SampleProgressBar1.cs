using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(Button))]
	public class SampleProgressBar1 : MonoBehaviour {
		public Progressbar bar;

		Button button;

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
				bar.Animate();
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