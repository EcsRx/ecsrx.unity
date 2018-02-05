using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// PickerInt.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/PickerInt")]
	public class PickerInt : Picker<int,PickerInt> {
		/// <summary>
		/// ListView.
		/// </summary>
		[SerializeField]
		public ListViewInt ListView;

		/// <summary>
		/// Prepare picker to open.
		/// </summary>
		/// <param name="defaultValue">Default value.</param>
		public override void BeforeOpen(int defaultValue)
		{
			ListView.SelectedIndex = ListView.DataSource.IndexOf(defaultValue);

			ListView.OnSelectObject.AddListener(ListViewCallback);
		}

		void ListViewCallback(int index)
		{
			Selected(ListView.DataSource[index]);
		}

		/// <summary>
		/// Prepare picker to close.
		/// </summary>
		public override void BeforeClose()
		{
			ListView.OnSelectObject.RemoveListener(ListViewCallback);
		}
	}
}