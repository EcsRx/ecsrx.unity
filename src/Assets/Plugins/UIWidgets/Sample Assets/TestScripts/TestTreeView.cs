using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TestTreeView : MonoBehaviour {
		public TreeView Tree;

		ObservableList<TreeNode<TreeViewItem>> nodes;
		ObservableList<TreeNode<TreeViewItem>> UINodes;

		void Start()
		{
			Tree.Start();

			Tree.Nodes = nodes;

			//SetComparison();
		}

		public void UseDuplicates()
		{
			nodes.OnChange += UpdateUINodes;

			UpdateUINodes();
		}

		void UpdateUINodes()
		{
			UINodes = DuplicateNodes(nodes);

			// some code to set UINodes comparison

			Tree.Nodes = UINodes;
			// imporant: TreeView events will return index in UINodes, not nodes
		}

		ObservableList<TreeNode<T>> DuplicateNodes<T>(ObservableList<TreeNode<T>> source)
		{
			var result = new ObservableList<TreeNode<T>>();

			foreach (var node in source)
			{
				result.Add(new TreeNode<T>(
					node.Item,
					(node.Nodes==null) ? null : DuplicateNodes(node.Nodes as ObservableList<TreeNode<T>>),
					node.IsExpanded,
					node.IsVisible
				));
			}

			return result;
		}

		public void SetComparison()
		{
			(nodes[0].Nodes as ObservableList<TreeNode<TreeViewItem>>).Comparison = comparisonDesc;
			nodes.Comparison = comparisonDesc;
		}

		// add subnodes
		public void AddSubNodes()
		{
			if (nodes.Count==0)
			{
				return ;
			}
			// get parent node
			var node = nodes[0];
			// or find parent node by name
			// var node = nodes.Find(x => x.Item.Name = "Node 2");

			if (node.Nodes==null)
			{
				node.Nodes = new ObservableList<TreeNode<TreeViewItem>>();
			}
			var new_item1 = new TreeViewItem("Subnode 1");
			var new_node1 = new TreeNode<TreeViewItem>(new_item1);

			var new_item2 = new TreeViewItem("Subnode 2");
			var new_node2 = new TreeNode<TreeViewItem>(new_item2);

			var new_item3 = new TreeViewItem("Subnode 3");
			var new_node3 = new TreeNode<TreeViewItem>(new_item3);

			node.Nodes.BeginUpdate();

			node.Nodes.Add(new_node1);
			node.Nodes.Add(new_node2);
			node.Nodes.Add(new_node3);

			node.Nodes.EndUpdate();
		}

		public void SelectNode()
		{
			var node_to_select = nodes[0].Nodes[0];
			// find index of node, DataSource contains list of visible nodes
			var index = Tree.DataSource.FindIndex(x => x.Node==node_to_select);
			// select node
			Tree.Select(index);
		}

		public void ScrollToNewNode()
		{
			var new_item = new TreeViewItem("New node");
			var new_node = new TreeNode<TreeViewItem>(new_item);
			//nodes[0].Nodes.Add(new_node);
			nodes.Add(new_node);
			//nodes.Insert(0, new_node);

			//ScrollToNode(new_node);
			ScrollToNodeAnimated(new_node);
		}

		public void ScrollToNode(TreeNode<TreeViewItem> node)
		{
			// find index of node, DataSource contains list of visible nodes
			var index = Tree.DataSource.FindIndex(x => x.Node==node);
			// if node exists and visible
			if (index!=-1)
			{
				//scroll to node
				Tree.ScrollTo(index);
			}
		}

		[SerializeField]
		[Tooltip("Requirements: start value should be less than end value; Recommended start value = 0; end value = 1;")]
		public AnimationCurve Movement = AnimationCurve.EaseInOut(0, 0, 1, 1);

		public void ScrollToNodeAnimated(TreeNode<TreeViewItem> node)
		{
			// find index of node, DataSource contains list of visible nodes
			var index = Tree.DataSource.FindIndex(x => x.Node==node);
			// if node exists and visible
			if (index!=-1)
			{
				//scroll to node
				StartCoroutine(RunAnimation(index, false));
			}
		}

		IEnumerator RunAnimation(int index, bool unscaledTime)
		{
			var startPosition = Tree.IsHorizontal() ? Tree.ScrollRect.content.anchoredPosition.x : Tree.ScrollRect.content.anchoredPosition.y;
			var endPosition = Tree.GetItemPosition(index);
			if (endPosition > startPosition)
			{
				endPosition = Tree.GetItemPositionBottom(index);
			}

			float delta;

			var animationLength = Movement.keys[Movement.keys.Length - 1].time;
			var startTime = (unscaledTime ? Time.unscaledTime : Time.time);
			do
			{
				delta = ((unscaledTime ? Time.unscaledTime : Time.time) - startTime);
				var value =  Movement.Evaluate(delta);

				var position = startPosition + ((endPosition - startPosition) * value);
				if (Tree.IsHorizontal())
				{
					Tree.ScrollRect.content.anchoredPosition = new Vector2(position, Tree.ScrollRect.content.anchoredPosition.y);
				}
				else
				{
					Tree.ScrollRect.content.anchoredPosition = new Vector2(Tree.ScrollRect.content.anchoredPosition.x, position);
				}
				yield return null;
			}
			while (delta < animationLength);

			if (Tree.IsHorizontal())
			{
				Tree.ScrollRect.content.anchoredPosition = new Vector2(endPosition, Tree.ScrollRect.content.anchoredPosition.y);
			}
			else
			{
				Tree.ScrollRect.content.anchoredPosition = new Vector2(Tree.ScrollRect.content.anchoredPosition.x, endPosition);
			}
		}


		// get currently selected nodes
		public void GetSelectedNodes()
		{
			Debug.Log(Tree.SelectedIndex);
			Debug.Log(string.Join(",", Tree.SelectedIndicies.Select(x => x.ToString()).ToArray()));
			var selectedNodes = Tree.SelectedNodes;
			if (selectedNodes!=null)
			{
				selectedNodes.ForEach(node => Debug.Log(node.Item.Name));
			}

		}

		public void GetNodePath()
		{
			var path = nodes[0].Nodes[0].Nodes[0].Path;
			path.ForEach(node => Debug.Log(node.Item.Name));
		}

		public void SelectNodes()
		{
			if ((nodes.Count==0) || (nodes[0].Nodes.Count==0))
			{
				return ;
			}
			// replace on find node "Node 1 - 1"
			var parent_node = nodes[0].Nodes[0];
			var children = new List<TreeNode<TreeViewItem>>();
			GetChildrenNodes(parent_node, children);

			// add children to selected nodes
			Tree.SelectedNodes = Tree.SelectedNodes.Union(children).ToList();

			// select only children
			//Tree.SelectedNodes = children;
		}

		public void DeselectNodes()
		{
			if ((nodes.Count==0) || (nodes[0].Nodes.Count==0))
			{
				return ;
			}
			// replace on find node "Node 1 - 1"
			var parent_node = nodes[0].Nodes[0];
			var children = new List<TreeNode<TreeViewItem>>();
			GetChildrenNodes(parent_node, children);

			// remove children from selected nodes
			Tree.SelectedNodes = Tree.SelectedNodes.Except(children).ToList();

			// deselect all
			//Tree.SelectedNodes = new List<TreeNode<TreeViewItem>>();
		}

		void GetChildrenNodes(TreeNode<TreeViewItem> node, List<TreeNode<TreeViewItem>> children)
		{
			if (node.Nodes!=null)
			{
				children.AddRange(node.Nodes);
				node.Nodes.ForEach(x => GetChildrenNodes(x, children));
			}
		}

		// only one node can be selected
		public void SetOnlyOnSelectable()
		{
			Tree.Multiple = false;
		}

		// many nodes can be selected at once
		public void SetMultipleSelectable()
		{
			Tree.Multiple = true;
		}

		// Compare nodes by Name in ascending order
		Comparison<TreeNode<TreeViewItem>> comparisonAsc = (x, y) => {
			return (x.Item.LocalizedName ?? x.Item.Name).CompareTo(y.Item.LocalizedName ?? y.Item.Name);
		};

		// Compare nodes by Name in descending order
		Comparison<TreeNode<TreeViewItem>> comparisonDesc = (x, y) => {
			return -(x.Item.LocalizedName ?? x.Item.Name).CompareTo(y.Item.LocalizedName ?? y.Item.Name);
		};

		public void SortAsc()
		{
			nodes.BeginUpdate();
			ApplyNodesSort(nodes, comparisonAsc);
			nodes.EndUpdate();
		}

		public void SortDesc()
		{
			nodes.BeginUpdate();
			ApplyNodesSort(nodes, comparisonDesc);
			nodes.EndUpdate();
		}

		void ApplyNodesSort<T>(ObservableList<TreeNode<T>> nodes, Comparison<TreeNode<T>> comparison)
		{
			nodes.Sort(comparison);
			nodes.ForEach(node => {
				if (node.Nodes!=null)
				{
					ApplyNodesSort(node.Nodes as ObservableList<TreeNode<T>>, comparison);
				}
			});
		}

		public void TestRemove(string name)
		{
			RemoveByName(nodes, name);
		}

		public void RemoveByName(ObservableList<TreeNode<TreeViewItem>> nodes, string name)
		{
			Remove(nodes, x => x.Item.Name==name);
		}

		public bool Remove<T>(ObservableList<TreeNode<T>> nodes, Predicate<TreeNode<T>> match)
		{
			var findedNode = nodes.Find(match);
			if (findedNode!=null)
			{
				findedNode.Parent = null;
				//this.nodes.Add(findedNode as TreeNode<TreeViewItem>);
				return true;
			}
			foreach (var node in nodes)
			{
				if (node.Nodes==null)
				{
					continue ;
				}
				if (Remove(node.Nodes as ObservableList<TreeNode<T>>, match))
				{
					return true;
				}
			}
			return false;
		}

		public void TestReorder(string name)
		{
			ChangePositionByName(nodes, name, 0);
		}
		
		public void ChangePositionByName(ObservableList<TreeNode<TreeViewItem>> nodes, string name, int position)
		{
			ChangePosition(nodes, x => x.Item.Name==name, position);
		}

		public bool ChangePosition<T>(ObservableList<TreeNode<T>> nodes, Predicate<TreeNode<T>> match, int position)
		{
			var findedNode = nodes.Find(match);
			if (findedNode!=null)
			{
				nodes.BeginUpdate();
				nodes.Remove(findedNode);
				nodes.Insert(position, findedNode);
				nodes.EndUpdate();
				return true;
			}
			foreach (var node in nodes)
			{
				if (node.Nodes==null)
				{
					continue ;
				}
				if (ChangePosition(node.Nodes as ObservableList<TreeNode<T>>, match, position))
				{
					return true;
				}
			}
			return false;
		}

		public void Test10K()
		{
			var config = new List<int>() {10, 10, 10, 10};
			nodes = GenerateTreeNodes(config, isExpanded: true);

			Tree.Nodes = nodes;
		}

		public void LongNames()
		{
			var config = new List<int>() {2, 2, 2, 2, 2, 2, 2, 2, 2};
			nodes = GenerateTreeNodes(config, isExpanded: true);
			
			Tree.Nodes = nodes;
		}

		Dictionary<string,ObservableList<TreeNode<TreeViewItem>>> dNodes;
		ObservableList<TreeNode<TreeViewItem>> nodes5k;
		ObservableList<TreeNode<TreeViewItem>> nodes10k;
		ObservableList<TreeNode<TreeViewItem>> nodes50k;
		public TestTreeView()
		{
			//var config = new List<int>() {20, 10, 10, 10, 25};
			//var config = new List<int>() {5, 5, 5, 5, 5};
			//var config = new List<int>() {5, 4, 3};
			//var config = new List<int>() {2, 2, 2, 2, 2, 2, 2, 2, 2};
			//var config = new List<int>() {5, 10, 10, 10, };
			//var config = new List<int>() {3, 3 };
			var config = new List<int>() {5, 5, 2 };
			nodes = GenerateTreeNodes(config, isExpanded: true);
		}

		public void PerformanceCheck(string k)
		{
			if (dNodes==null)
			{
				dNodes = new Dictionary<string, ObservableList<TreeNode<TreeViewItem>>>();

				var config1k = new List<int>() {10, 10, 10 };
				dNodes.Add("1k", GenerateTreeNodes(config1k, isExpanded: true));
				
				var config5k = new List<int>() {5, 10, 10, 10 };
				dNodes.Add("5k", GenerateTreeNodes(config5k, isExpanded: true));
				
				var config10k = new List<int>() {10, 10, 10, 10 };
				dNodes.Add("10k", GenerateTreeNodes(config10k, isExpanded: true));
				
				var config50k = new List<int>() {5, 10, 10, 10, 10 };
				dNodes.Add("50k", GenerateTreeNodes(config50k, isExpanded: true));
			}
			nodes = dNodes[k];
			Tree.Nodes = dNodes[k];
		}

		public void SetTreeNodes()
		{
			Tree.Nodes = nodes;

			nodes.BeginUpdate();

			var test_item = new TreeViewItem("added");
			var test_node = new TreeNode<TreeViewItem>(test_item);
			nodes.Add(test_node);
			nodes[1].IsVisible = false;
			nodes[2].Nodes[1].IsVisible = false;

			nodes.EndUpdate();
		}

		public void AddNode()
		{
			var test_item = new TreeViewItem("New node");
			var test_node = new TreeNode<TreeViewItem>(test_item);
			nodes.Add(test_node);
		}

		public void ToggleNode()
		{
			nodes[0].Nodes[0].IsExpanded = !nodes[0].Nodes[0].IsExpanded;
		}

		public void ChangeNodesName()
		{
			nodes[0].Item.Name = "Node renamed from code";
			nodes[0].Nodes[1].Item.Name = "Another node renamed from code";
		}

		public void ResetFilter()
		{
			nodes.BeginUpdate();

			nodes.ForEach(SetVisible);

			nodes.EndUpdate();
		}

		void SetVisible(TreeNode<TreeViewItem> node)
		{
			if (node.Nodes!=null)
			{
				node.Nodes.ForEach(SetVisible);
			}
			node.IsVisible = true;
		}

		public void Filter(string nameContains)
		{
			nodes.BeginUpdate();

			SampleFilter(nodes, x => x.Name.Contains(nameContains));

			nodes.EndUpdate();
		}

		public void Clear()
		{
			//nodes.Clear();
			nodes = new ObservableList<TreeNode<TreeViewItem>>();
			Tree.Nodes = nodes;
		}

		bool SampleFilter(IObservableList<TreeNode<TreeViewItem>> nodes, Func<TreeViewItem,bool> filterFunc)
		{
			return nodes.Count(x => {
				var have_visible_children = (x.Nodes==null) ? false : SampleFilter(x.Nodes, filterFunc);
				x.IsVisible = have_visible_children || filterFunc(x.Item) ;
				return x.IsVisible;
			}) > 0;
		}

		static public ObservableList<TreeNode<TreeViewItem>> GenerateTreeNodes(List<int> items, string nameStartsWith = "Node ", bool isExpanded = true)
		{
			return Enumerable.Range(1, items[0]).Select(x => {
				var item_name = nameStartsWith + x;
				var item = new TreeViewItem(item_name, null);
				var nodes = items.Count > 1
					? GenerateTreeNodes(items.GetRange(1, items.Count - 1), item_name + " - ", isExpanded)
					: null;

				return new TreeNode<TreeViewItem>(item, nodes, isExpanded);
			}).ToObservableList();
		}

		public void ReloadScene()
		{
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			Application.LoadLevel(Application.loadedLevel);
			#else
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
			#endif
		}
	}
}