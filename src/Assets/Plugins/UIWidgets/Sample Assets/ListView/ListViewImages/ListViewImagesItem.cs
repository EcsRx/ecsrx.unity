using UIWidgets;
using UnityEngine;

namespace UIWidgetsSamples {
	[System.Serializable]
	public class ListViewImagesItem : IItemHeight {
		/// <summary>
		/// URL.
		/// </summary>
		[SerializeField]
		public string Url;

		[SerializeField]
		float height;

		/// <summary>
		/// Gets or sets the height of item.
		/// </summary>
		/// <value>The height.</value>
		public float Height {
			get {
				return height;
			}
			set {
				height = value;
			}
		}
	}
}