using UnityEngine;
using System.Collections.Generic;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(KeyValueListView))]
	public class KeyValueListViewDataSource : MonoBehaviour {
		void Start()
		{
			var items = GetComponent<KeyValueListView>().DataSource;

			items.BeginUpdate();

			items.Add(new KeyValuePair<string, string>("AT", "Austria"));
			items.Add(new KeyValuePair<string, string>("CN", "China"));
			items.Add(new KeyValuePair<string, string>("KR", "Korea"));
			items.Add(new KeyValuePair<string, string>("JP", "Japan"));
			items.Add(new KeyValuePair<string, string>("DE", "Germany"));
			items.Add(new KeyValuePair<string, string>("FI", "Finland"));

			items.EndUpdate();
		}
	}
}