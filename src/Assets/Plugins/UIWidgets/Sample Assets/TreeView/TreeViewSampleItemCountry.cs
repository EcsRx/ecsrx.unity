using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	[System.Serializable]
	public class TreeViewSampleItemCountry : ITreeViewSampleItem {
		public event OnChange OnChange = () => { };
		
		[SerializeField]
		Sprite icon;
		
		public Sprite Icon {
			get {
				return icon;
			}
			set {
				icon = value;
				Changed();
			}
		}

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

		public TreeViewSampleItemCountry(string itemName, Sprite itemIcon = null)
		{
			name = itemName;
			icon = itemIcon;
		}
		
		void Changed()
		{
			OnChange();
		}

		public void Display(TreeViewSampleComponent component)
		{
			component.Icon.sprite = Icon;
			component.Text.text = Name;
			
			if (component.SetNativeSize)
			{
				component.Icon.SetNativeSize();
			}
			
			component.Icon.color = (component.Icon.sprite==null) ? Color.clear : Color.white;
		}
	}
}