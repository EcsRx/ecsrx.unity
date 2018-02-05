using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UIWidgets {
	/// <summary>
	/// TreeViewDataSourceItem.
	/// </summary>
	[Serializable]
	public class TreeViewDataSourceItem {
		/// <summary>
		/// The depth.
		/// </summary>
		[SerializeField]
		public int Depth;

		/// <summary>
		/// The is visible.
		/// </summary>
		[SerializeField]
		public bool IsVisible = true;

		/// <summary>
		/// The is expanded.
		/// </summary>
		[SerializeField]
		public bool IsExpanded;

		/// <summary>
		/// The icon.
		/// </summary>
		[SerializeField]
		public Sprite Icon;

		/// <summary>
		/// The name.
		/// </summary>
		[SerializeField]
		public string Name;

		/// <summary>
		/// The value.
		/// </summary>
		[SerializeField]
		public int Value;

		/// <summary>
		/// The tag.
		/// </summary>
		[SerializeField]
		public object Tag;
	}

	/// <summary>
	/// TreeViewDataSource.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/TreeViewDataSource")]
	[RequireComponent(typeof(TreeView))]
	public class TreeViewDataSource : MonoBehaviour
	{
		/// <summary>
		/// The data.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<TreeViewDataSourceItem> Data = new List<TreeViewDataSourceItem>();

		bool isStarted;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public virtual void Start()
		{
			if (isStarted)
			{
				return ;
			}
			isStarted = true;
			SetDataSource();
		}

		/// <summary>
		/// Sets the data source.
		/// </summary>
		public virtual void SetDataSource()
		{
			var tree = GetComponent<TreeView>();
			tree.Start();

			var nodes = new ObservableList<TreeNode<TreeViewItem>>();
			List2Tree(nodes);
			tree.Nodes = nodes;
		}

		/// <summary>
		/// Convert flat list to tree.
		/// </summary>
		/// <param name="nodes">Nodes.</param>
		public virtual void List2Tree(ObservableList<TreeNode<TreeViewItem>> nodes)
		{
			TreeNode<TreeViewItem> last_node = null;
			for (int i = 0; i < Data.Count; i++)
			{
				var item = Data[i];
				item.IsVisible = true;

				//Debug.Log(item.Depth + " -> " + item.Name + " -> " + item.IsVisible);
				if (item.Depth==0)
				{
					last_node = Item2Node(item);
					nodes.Add(last_node);
				}
				else if (item.Depth==(Data[i-1].Depth+1))
				{
					var current_node = Item2Node(item);
					last_node.Nodes.Add(current_node);

					last_node = current_node;
				}
				else if (item.Depth <= Data[i-1].Depth)
				{
					int n = item.Depth - Data[i-1].Depth + 1;
					for (int j = 0; j < n; j++)
					{
						last_node = last_node.Parent;
					}

					var current_node = Item2Node(item);
					last_node.Nodes.Add(current_node);

					last_node = current_node;
				}
				else
				{
					//Debug.LogWarning("Unknown case");
				}
			}
		}

		/// <summary>
		/// Convert item to node.
		/// </summary>
		/// <returns>The node.</returns>
		/// <param name="item">Item.</param>
		protected virtual TreeNode<TreeViewItem> Item2Node(TreeViewDataSourceItem item)
		{
			var nodeItem = new TreeViewItem(item.Name, item.Icon);
			nodeItem.Value = item.Value;
			nodeItem.Tag = item.Tag;
			return new TreeNode<TreeViewItem>(nodeItem, new ObservableList<TreeNode<TreeViewItem>>(), item.IsExpanded, item.IsVisible);
		}
	}
}