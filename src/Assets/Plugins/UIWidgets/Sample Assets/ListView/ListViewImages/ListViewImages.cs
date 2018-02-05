using UIWidgets;

namespace UIWidgetsSamples {

	/// <summary>
	/// ListViewImages.
	/// </summary>
	public class ListViewImages : ListViewCustomHeight<ListViewImagesComponent,ListViewImagesItem> {
		/// <summary>
		/// Sets component data with specified item.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="item">Item.</param>
		protected override void SetData(ListViewImagesComponent component, ListViewImagesItem item)
		{
			component.SetData(item);
		}
	}
}