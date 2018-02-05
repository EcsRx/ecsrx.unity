using UnityEngine;
using System.Collections;

namespace UIWidgetsSamples
{
	public class StandaloneExit : MonoBehaviour {

		void Start()
		{
			#if !UNITY_STANDALONE
			gameObject.SetActive(false);
			#endif
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}