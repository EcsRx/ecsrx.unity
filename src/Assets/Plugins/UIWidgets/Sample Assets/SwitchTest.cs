using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	public class SwitchTest : MonoBehaviour
	{
		[SerializeField]
		Switch Switch;

		void Start()
		{
			if (Switch!=null)
			{
				Switch.OnValueChanged.AddListener(OnSwitchChanged);
			}

		}

		void OnSwitchChanged(bool status)
		{
			Debug.Log("switch status: " + status);
		}

		void OnDestroy()
		{
			if (Switch!=null)
			{
				Switch.OnValueChanged.RemoveListener(OnSwitchChanged);
			}
		}
	}
}

