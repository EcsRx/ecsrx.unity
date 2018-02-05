using UnityEngine;
using UIWidgets;
using System.Linq;

namespace UIWidgetsSamples {
	public class PopupTest : MonoBehaviour
	{
		[SerializeField]
		Popup popup;

		Popup currentPopup;
		ListViewInt currentListView;

		public void ShowPicker()
		{
			currentPopup = popup.Template();
			currentPopup.Show();
			currentListView = currentPopup.GetComponentInChildren<ListViewInt>();

			// fill list with values
			currentListView.DataSource = Enumerable.Range(1, 100).ToObservableList();

			// deselect
			currentListView.SelectedIndex = -1;
			currentListView.OnSelectObject.AddListener(Callback);
		}

		void Callback(int index)
		{
			// do something with value
			Debug.Log(currentListView.DataSource[index]);

			currentListView.OnSelectObject.RemoveListener(Callback);
			currentPopup.Close();
		}
	}
}