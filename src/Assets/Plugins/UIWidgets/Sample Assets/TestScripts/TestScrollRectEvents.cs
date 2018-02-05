using UnityEngine;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(ScrollRectEvents))]
	public class TestScrollRectEvents : MonoBehaviour {
		[SerializeField]
		public ListViewIcons ListView;

		IObservableList<ListViewIconsItemDescription> data;

		bool isStarted = false;
		void Start()
		{
			if (isStarted)
			{
				return ;
			}
			isStarted = true;

			ListView.Sort = false;
			data = ListView.DataSource;
			(data as ObservableList<ListViewIconsItemDescription>).Comparison = null;
			ListView.Start();

			var scrollRectEvents = GetComponent<ScrollRectEvents>();
			if (scrollRectEvents!=null)
			{
				scrollRectEvents.OnPullUp.AddListener(Refresh);
				scrollRectEvents.OnPullDown.AddListener(LoadMore);
			}
		}

		void OnEnable()
		{
			Start();
			StartCoroutine(LoadData(0));
		}
		
		IEnumerator LoadData(int start)
		{
			if (start==0)
			{
				data.Clear();
			}

			WWW www = new WWW("https://ilih.ru/steamspy/?start=" + start.ToString());
			yield return www;
			
			var lines = www.text.Split('\n');
			
			data.BeginUpdate();

			lines.ForEach(ParseLine);
			
			data.EndUpdate();
		}

		void ParseLine(string line)
		{
			if (line=="")
			{
				return ;
			}
			var info = line.Split('\t');
			
			var item = new ListViewIconsItemDescription(){
				Name = string.Format("{0}. {1}", data.Count + 1, info[0]),
			};
			data.Add(item);
		}

		public void Refresh()
		{
			StartCoroutine(LoadData(0));
		}

		public void LoadMore()
		{
			StartCoroutine(LoadData(data.Count));
		}

		void OnDestroy()
		{
			var scrollRectEvents = GetComponent<ScrollRectEvents>();
			if (scrollRectEvents!=null)
			{
				scrollRectEvents.OnPullUp.RemoveListener(Refresh);
				scrollRectEvents.OnPullDown.RemoveListener(LoadMore);
			}
		}
	}
}