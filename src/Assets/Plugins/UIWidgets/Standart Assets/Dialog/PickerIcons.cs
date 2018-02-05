using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// PickerIcons.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/PickerIcons")]
	public class PickerIcons : Picker<ListViewIconsItemDescription,PickerIcons> {
		/// <summary>
		/// ListView.
		/// </summary>
		[SerializeField]
		public ListViewIcons ListView;

		/// <summary>
		/// Prepare picker to open.
		/// </summary>
		/// <param name="defaultValue">Default value.</param>
		public override void BeforeOpen(ListViewIconsItemDescription defaultValue)
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