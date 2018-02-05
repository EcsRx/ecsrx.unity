using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	[System.Serializable]
	public class TreeViewSampleItemContinent : ITreeViewSampleItem {
		public event OnChange OnChange = () => { };
		
		[SerializeField]
		string name;
		
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
				Changed();
			}
		}

		[SerializeField]
		int countries;
		
		public int Countries {
			get {
				return countries;
			}
			set {
				countries = value;
				Changed();
			}
		}

		public TreeViewSampleItemContinent(string itemName, int itemCountries = 0)
		{
			name = itemName;
			countries = itemCountries;
		}
		
		void Changed()
		{
			OnChange();
		}

		public void Display(TreeViewSampleComponent component)
		{
			component.Icon.sprite = null;
			component.Icon.color = Color.clear;
			component.Text.text = Name + " (Countries: " + Countries + ") ";
		}
	}
}