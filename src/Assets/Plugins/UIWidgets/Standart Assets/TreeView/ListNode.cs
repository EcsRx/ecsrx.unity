using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// List node.
	/// </summary>
	public class ListNode<TItem>
	{
		/// <summary>
		/// The depth.
		/// </summary>
		public int Depth;

		/// <summary>
		/// The node.
		/// </summary>
		public TreeNode<TItem> Node;

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="depth">Depth.</param>
		public ListNode(TreeNode<TItem> node, int depth)
		{
			Node = node;
			Depth = depth;
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
		public override bool Equals(System.Object obj)
		{
			var nodeObj = obj as ListNode<TItem>; 
			if (nodeObj==null)
			{
				return this==null;
			}
			if (this==null)
			{
				return false;
			}
			return Node.Equals(nodeObj.Node);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns true if the nodes items are equal, false otherwise.
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		public static bool operator ==(ListNode<TItem> a, ListNode<TItem> b)
		{
			var a_null = object.ReferenceEquals(null, a);
			var b_null = object.ReferenceEquals(null, b);
			if (a_null && b_null)
			{
				return true;
			}
			if (a_null!=b_null)
			{
				return false;
			}

			return a.Node.Equals(b.Node);
		}

		/// <summary>
		/// Returns true if the nodes items are not equal, false otherwise.
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		public static bool operator !=(ListNode<TItem> a, ListNode<TItem> b)
		{
			var a_null = object.ReferenceEquals(null, a);
			var b_null = object.ReferenceEquals(null, b);

			if (a_null && b_null)
			{
				return false;
			}
			if (a_null!=b_null)
			{
				return true;
			}

			return !a.Node.Equals(b.Node);
		}
	}
}