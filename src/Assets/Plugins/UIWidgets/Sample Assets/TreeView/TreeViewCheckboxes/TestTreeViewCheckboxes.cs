using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TestTreeViewCheckboxes : MonoBehaviour {
		public TreeViewCheckboxes Tree;

		void Start()
		{
			Tree.Start();

			SetTreeNodes();

			Tree.OnNodeCheckboxChanged.AddListener(CheckboxChanged);
		}

		void SetTreeNodes()
		{
			var nodes = new ObservableList<TreeNode<TreeViewCheckboxesItem>>();
			nodes.Add(new TreeNode<TreeViewCheckboxesItem>(new TreeViewCheckboxesItem("Item 1")));
			nodes.Add(new TreeNode<TreeViewCheckboxesItem>(new TreeViewCheckboxesItem("Item 2")));
			nodes.Add(new TreeNode<TreeViewCheckboxesItem>(new TreeViewCheckboxesItem("Item 3")));

			nodes[0].Nodes = new ObservableList<TreeNode<TreeViewCheckboxesItem>>();
			nodes[0].Nodes.Add(new TreeNode<TreeViewCheckboxesItem>(new TreeViewCheckboxesItem("Item 1-1")));
			nodes[0].Nodes.Add(new TreeNode<TreeViewCheckboxesItem>(new TreeViewCheckboxesItem("Item 1-2")));

			Tree.Nodes = nodes;
		}

		public void CheckboxChanged(TreeNode<TreeViewCheckboxesItem> node)
		{
			if (node.Item.Selected)
			{
				Debug.Log(node.Item.Name + " selected");
			}
			else
			{
				Debug.Log(node.Item.Name + " deselected");
			}
		}

		void OnDestroy()
		{
			if (Tree!=null)
			{
				Tree.OnNodeCheckboxChanged.RemoveListener(CheckboxChanged);
			}
		}
	}
}