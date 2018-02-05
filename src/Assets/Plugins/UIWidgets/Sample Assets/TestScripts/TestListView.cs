using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(Button))]
	public class TestListView : MonoBehaviour {
		public ListView listView;

		Button button;

		void Start()
		{
			button = GetComponent<Button>();
			if (button!=null)
			{
				button.onClick.AddListener(Click);
			}
		}

		ObservableList<string> items;

		int click = 0;
		void Click()
		{
			if (click==0)
			{
				items = listView.DataSource;

				items.Add("Added from script 0");
				items.Add("Added from script 1");
				items.Add("Added from script 2");

				items.Remove("Caster");

				click += 1;
				return ;
			}
			if (click==1)
			{
				items.Clear();
				
				click += 1;
				return ;
			}
			if (click==2)
			{
				items.Add("test");
				
				click += 1;
				return ;
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