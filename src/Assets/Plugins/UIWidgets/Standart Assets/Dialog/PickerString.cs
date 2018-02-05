using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// PickerString.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/PickerString")]
	public class PickerString : Picker<string,PickerString> {
		/// <summary>
		/// ListView.
		/// </summary>
		[SerializeField]
		public ListView ListView;

		/// <summary>
		/// Prepare picker to open.
		/// </summary>
		/// <param name="defaultValue">Default value.</param>
		public override void BeforeOpen(string defaultValue)
		{
			ListView.SelectedIndex = ListView.DataSource.IndexOf(defaultValue);

			ListView.OnSelectString.AddListener(ListViewCallback);
		}

		void ListViewCallback(int index, string value)
		{
			Selected(value);
		}

		/// <summary>
		/// Prepare picker to close.
		/// </summary>
		public override void BeforeClose()
		{
			ListView.OnSelectString.RemoveListener(ListViewCallback);
		}
	}
}