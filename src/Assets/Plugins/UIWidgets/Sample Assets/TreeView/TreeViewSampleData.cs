using UnityEngine;
using System.Collections.Generic;
using UIWidgets;

namespace UIWidgetsSamples {
	[System.Serializable]
	public class TreeViewSampleDataCountry {
		[SerializeField]
		public Sprite Flag;

		[SerializeField]
		public string Name;
	}

	public class TreeViewSampleData : MonoBehaviour {
		[SerializeField]
		TreeViewSample tree;

		[SerializeField]
		List<TreeViewSampleDataCountry> dataEurope = new List<TreeViewSampleDataCountry>();

		[SerializeField]
		List<TreeViewSampleDataCountry> dataAsia = new List<TreeViewSampleDataCountry>();

		void Start()
		{
			tree.Start();

			tree.Nodes = GetData();
		}

		ObservableList<TreeNode<ITreeViewSampleItem>> GetData()
		{
			var countries = new ObservableList<TreeNode<ITreeViewSampleItem>>();
			countries.Add(Node(new TreeViewSampleItemContinent("Africa", 54)));
			countries.Add(Node(new TreeViewSampleItemContinent("Antarctica", 12)));
			countries.Add(Node(new TreeViewSampleItemContinent("Asia", 48), Data2Country(dataAsia)));
			countries.Add(Node(new TreeViewSampleItemContinent("Australia", 4)));
			countries.Add(Node(new TreeViewSampleItemContinent("Europe", 50), Data2Country(dataEurope)));
			countries.Add(Node(new TreeViewSampleItemContinent("North America", 23)));
			countries.Add(Node(new TreeViewSampleItemContinent("South America", 12)));

			return countries;
		}

		ObservableList<TreeNode<ITreeViewSampleItem>> Data2Country(List<TreeViewSampleDataCountry> data)
		{
			var countries = new ObservableList<TreeNode<ITreeViewSampleItem>>();
			data.ForEach(x => countries.Add(Node(new TreeViewSampleItemCountry(x.Name, x.Flag))));

			return countries;
		}

		TreeNode<ITreeViewSampleItem> Node(ITreeViewSampleItem item, ObservableList<TreeNode<ITreeViewSampleItem>> nodes = null)
		{
			return new TreeNode<ITreeViewSampleItem>(item, nodes, false, true);
		}
	}
}