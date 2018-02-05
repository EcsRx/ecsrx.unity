using UnityEngine;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(Combobox))]
	public class SampleFilter : MonoBehaviour
	{
		public GameObject Container;

		Combobox combobox;
		void Start()
		{
			combobox = GetComponent<Combobox>();
			if (combobox!=null)
			{
				combobox.OnSelect.AddListener(Filter);
			}
		}

		void Filter(int index, string item)
		{
			foreach (Transform child in Container.transform)
			{
				var child_active = (item=="All") ? true : child.gameObject.name.StartsWith(item);
				child.gameObject.SetActive(child_active);
			}
		}

		void OnDestroy()
		{
			if (combobox!=null)
			{
				combobox.OnSelect.RemoveListener(Filter);
			}
		}
	}
}