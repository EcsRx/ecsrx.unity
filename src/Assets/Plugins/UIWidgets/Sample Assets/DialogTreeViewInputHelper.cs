using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System.Collections.Generic;
using System.Linq;

namespace UIWidgetsSamples {
	public class DialogTreeViewInputHelper : MonoBehaviour {
		[SerializeField]
		public TreeView Folders;
		
		ObservableList<TreeNode<TreeViewItem>> nodes;
		bool _InitDone;

		public void Refresh()
		{
			if (!_InitDone)
			{
				var config = new List<int>() { 5, 5, 2 };
				nodes = TestTreeView.GenerateTreeNodes(config, isExpanded: true);
				
				// Set nodes
				Folders.Start();
				Folders.Nodes = nodes;
				
				_InitDone = true;
			}
		}
	}
}