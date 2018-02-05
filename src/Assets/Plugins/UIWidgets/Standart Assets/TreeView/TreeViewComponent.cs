using UnityEngine;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// TreeView component.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/TreeViewComponent")]
	public class TreeViewComponent : TreeViewComponentBase<TreeViewItem> {
		TreeViewItem item;

		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		public TreeViewItem Item {
			get {
				return item;
			}
			set {
				if (item!=null)
				{
					item.OnChange -= UpdateView;
				}
				item = value;
				if (item!=null)
				{
					item.OnChange += UpdateView;
				}
				UpdateView();
			}
		}

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="newNode">Node.</param>
		/// <param name="depth">Depth.</param>
		public override void SetData(TreeNode<TreeViewItem> newNode, int depth)
		{
			Node = newNode;
			base.SetData(Node, depth);

			Item = (Node==null) ? null : Node.Item;
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		protected virtual void UpdateView()
		{
			if ((Icon==null) || (Text==null))
			{
				return ;
			}
				
			if (Item==null)
			{
				Icon.sprite = null;
				Text.text = string.Empty;
			}
			else
			{
				Icon.sprite = Item.Icon;
				Text.text = Item.LocalizedName ?? Item.Name;
			}
			
			if (SetNativeSize)
			{
				Icon.SetNativeSize();
			}
			
			//set transparent color if no icon
			Icon.color = (Icon.sprite==null) ? Color.clear : Color.white;
		}

		/// <summary>
		/// Called when item moved to cache, you can use it free used resources.
		/// </summary>
		public override void MovedToCache()
		{
			if (Icon!=null)
			{
				Icon.sprite = null;
			}
		}

		/// <summary>
		/// Set default colors.
		/// </summary>
		/// <param name="primary">Primary color.</param>
		/// <param name="background">Background color.</param>
		/// <param name="fadeDuration">Fade duration.</param>
		public virtual void DefaultColoring(Color primary, Color background, float fadeDuration = 0f)
		{
			Coloring(primary, background, fadeDuration);
		}

		/// <summary>
		/// Set highlights colors.
		/// </summary>
		/// <param name="primary">Primary color.</param>
		/// <param name="background">Background color.</param>
		/// <param name="fadeDuration">Fade duration.</param>
		public virtual void HighlightColoring(Color primary, Color background, float fadeDuration = 0f)
		{
			Coloring(primary, background, fadeDuration);
		}

		/// <summary>
		/// Set select colors.
		/// </summary>
		/// <param name="primary">Primary color.</param>
		/// <param name="background">Background color.</param>
		/// <param name="fadeDuration">Fade duration.</param>
		public virtual void SelectColoring(Color primary, Color background, float fadeDuration = 0f)
		{
			Coloring(primary, background, fadeDuration);
		}

		/// <summary>
		/// Is color setted at least once.
		/// </summary>
		protected bool IsColorSetted;

		/// <summary>
		/// Set colors.
		/// </summary>
		/// <param name="primary">Primary color.</param>
		/// <param name="background">Background color.</param>
		/// <param name="fadeDuration">Fade duration.</param>
		protected virtual void Coloring(Color primary, Color background, float fadeDuration = 0f)
		{
			// reset default color to white, otherwise it will look darker than specified color,
			// because actual color = Text.color * Text.CrossFadeColor
			if (!IsColorSetted)
			{
				Text.color = Color.white;
				Background.color = Color.white;
			}

			// change color instantly for first time
			Text.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Background.CrossFadeColor(background, IsColorSetted ? fadeDuration : 0f, true, true);

			IsColorSetted = true;
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected override void OnDestroy()
		{
			if (item!=null)
			{
				item.OnChange -= UpdateView;
			}
			base.OnDestroy();
		}
	}
}