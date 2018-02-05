using UIWidgets;
using System.Collections.Generic;

namespace UIWidgetsSamples {

	public class KeyValueListView : ListViewCustom<KeyValueListViewItem,KeyValuePair<string,string>> {
		protected override void SetData(KeyValueListViewItem component, KeyValuePair<string,string> item)
		{
			component.SetData(item);
		}
	}
}
