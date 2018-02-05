using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	
	[System.Serializable]
	public class ListViewVariableHeightItemDescription : IItemHeight {
		[SerializeField]
		public string Name;

		[SerializeField]
		public string Text;

		[SerializeField]
		float height;

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