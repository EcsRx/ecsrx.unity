using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UIWidgets;

namespace UIWidgetsSamples {
	
	public class TestListViewSlider : MonoBehaviour {
		[SerializeField]
		ListViewSlider ListView;

		public void SetList()
		{
			var data = new ObservableList<ListViewSliderItem>();

			foreach (var i in Enumerable.Range(0, 100))
			{
				data.Add(new ListViewSliderItem(){Value = i});
				data[i].Value = Random.Range(0, 100);
			}

			ListView.DataSource = data;
		}
	}
}