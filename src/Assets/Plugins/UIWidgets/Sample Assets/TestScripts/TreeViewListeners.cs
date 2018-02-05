using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TreeViewListeners : MonoBehaviour {
		public TreeView Tree;

		void Start()
		{
			Tree.NodeSelected.AddListener(NodeSelected);
			Tree.NodeDeselected.AddListener(NodeDeselected);
		}

		// called when node selected
		public void NodeSelected(TreeNode<TreeViewItem> node)
		{
			Debug.Log(node.Item.Name + " selected");
		}

		// called when node deselected
		public void NodeDeselected(TreeNode<TreeViewItem> node)
		{
			Debug.Log(node.Item.Name + " deselected");
		}

		void OnDestroy()
		{
			if (Tree!=null)
			{
				Tree.NodeSelected.RemoveListener(NodeSelected);
				Tree.NodeDeselected.RemoveListener(NodeDeselected);
			}
		}
	}
}