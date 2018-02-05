using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// Node toggle.
	/// </summary>
	public enum NodeToggle {
		Rotate = 0,
		ChangeSprite = 1,
	}
	
	/// <summary>
	/// Node toggle event.
	/// </summary>
	[Serializable]
	public class NodeToggleEvent : UnityEvent<int> {
	}

	/// <summary>
	/// Tree view component base.
	/// </summary>
	public class TreeViewComponentBase<T> : ListViewItem {

		/// <summary>
		/// The icon.
		/// </summary>
		public Image Icon;
		
		/// <summary>
		/// The text.
		/// </summary>
		public Text Text;

		/// <summary>
		/// The toggle.
		/// </summary>
		public TreeNodeToggle Toggle;

		Image toggleImage;

		/// <summary>
		/// Gets the toggle image.
		/// </summary>
		/// <value>The toggle image.</value>
		protected Image ToggleImage {
			get {
				if (toggleImage==null)
				{
					toggleImage = Toggle.GetComponent<Image>();
				}
				return toggleImage;
			}
		}

		/// <summary>
		/// The toggle event.
		/// </summary>
		public NodeToggleEvent ToggleEvent = new NodeToggleEvent();

		/// <summary>
		/// The filler.
		/// </summary>
		public LayoutElement Filler;

		/// <summary>
		/// The on node expand.
		/// </summary>
		public NodeToggle OnNodeExpand = NodeToggle.Rotate;

		/// <summary>
		/// Is need animate arrow?
		/// </summary>
		public bool AnimateArrow;

		/// <summary>
		/// Sprite when node opened.
		/// </summary>
		public Sprite NodeOpened;

		/// <summary>
		/// Sprite when node closed.
		/// </summary>
		public Sprite NodeClosed;

		/// <summary>
		/// The node.
		/// </summary>
		public TreeNode<T> Node {
			get;
			protected set;
		}

		/// <summary>
		/// The padding per level.
		/// </summary>
		public float PaddingPerLevel = 30;

		/// <summary>
		/// Set icon native size.
		/// </summary>
		public bool SetNativeSize = true;

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected override void Start()
		{
			base.Start();
			Toggle.OnClick.AddListener(ToggleNode);
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected override void OnDestroy()
		{
			if (Toggle!=null)
			{
				Toggle.OnClick.RemoveListener(ToggleNode);
			}

			base.OnDestroy();
		}

		/// <summary>
		/// Toggles the node.
		/// </summary>
		protected virtual void ToggleNode()
		{
			if (AnimationCorutine!=null)
			{
				StopCoroutine(AnimationCorutine);
			}
			SetToggle(Node.IsExpanded);

			ToggleEvent.Invoke(Index);

			if (OnNodeExpand==NodeToggle.Rotate)
			{
				if (AnimateArrow)
				{
					AnimationCorutine = Node.IsExpanded ? CloseCorutine() : OpenCorutine();
					StartCoroutine(AnimationCorutine);
				}
			}
			else
			{
				SetToggle(Node.IsExpanded);
			}
		}

		/// <summary>
		/// Ses the toggle sprite.
		/// </summary>
		/// <param name="isExpanded">If set to <c>true</c> is expanded.</param>
		protected virtual void SeToggleSprite(bool isExpanded)
		{
			ToggleImage.sprite = isExpanded ? NodeOpened : NodeClosed;
		}

		/// <summary>
		/// The animation corutine.
		/// </summary>
		protected IEnumerator AnimationCorutine;

		/// <summary>
		/// Animate arrow on open.
		/// </summary>
		/// <returns>The corutine.</returns>
		protected virtual IEnumerator OpenCorutine()
		{
			var rect = Toggle.transform as RectTransform;
			yield return StartCoroutine(Animations.RotateZ(rect, 0.2f, -90, 0));
		}

		/// <summary>
		/// Animate arrow on close.
		/// </summary>
		/// <returns>The corutine.</returns>
		protected virtual IEnumerator CloseCorutine()
		{
			var rect = Toggle.transform as RectTransform;
			yield return StartCoroutine(Animations.RotateZ(rect, 0.2f, 0, -90));
		}

		/// <summary>
		/// Sets the toggle rotation.
		/// </summary>
		/// <param name="isExpanded">If set to <c>true</c> is expanded.</param>
		protected virtual void SetToggleRotation(bool isExpanded)
		{
			if (Toggle==null)
			{
				return ;
			}
			Toggle.transform.localRotation = Quaternion.Euler(0, 0, (isExpanded) ? -90 : 0);
		}

		/// <summary>
		/// Sets the toggle.
		/// </summary>
		/// <param name="isExpanded">If set to <c>true</c> is expanded.</param>
		protected virtual void SetToggle(bool isExpanded)
		{
			if (OnNodeExpand==NodeToggle.Rotate)
			{
				SetToggleRotation(isExpanded);
			}
			else
			{
				SeToggleSprite(isExpanded);
			}
		}

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="depth">Depth.</param>
		public virtual void SetData(TreeNode<T> node, int depth)
		{
			if (node!=null)
			{
				Node = node;
				SetToggle(Node.IsExpanded);
			}
			if (Filler!=null)
			{
				Filler.preferredWidth = depth * PaddingPerLevel;
			}

			if ((Toggle!=null) && (Toggle.gameObject!=null))
			{
				var toggle_active = (node.Nodes!=null) && (node.Nodes.Count>0);
				if (Toggle.gameObject.activeSelf!=toggle_active)
				{
					Toggle.gameObject.SetActive(toggle_active);
				}
			}
		}
	}
}