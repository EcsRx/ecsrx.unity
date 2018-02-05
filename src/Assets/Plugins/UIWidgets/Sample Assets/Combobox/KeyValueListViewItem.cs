using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System.Collections.Generic;
using System.Collections;

namespace UIWidgetsSamples {
	public class KeyValueListViewItem : ListViewItem {
		[SerializeField]
		public Text Text;

		public void SetData(KeyValuePair<string,string> item)
		{
			Text.text = item.Value;
		}
	}
}