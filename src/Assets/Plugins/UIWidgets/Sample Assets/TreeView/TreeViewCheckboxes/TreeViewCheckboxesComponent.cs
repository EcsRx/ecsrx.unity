using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TreeViewCheckboxesComponent : TreeViewComponentBase<TreeViewCheckboxesItem> {
		TreeViewCheckboxesItem item;

		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		public TreeViewCheckboxesItem Item {
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

		public NodeToggleEvent NodeCheckboxChanged = new NodeToggleEvent();

		public Toggle Checkbox;

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="newNode">Node.</param>
		/// <param name="depth">Depth.</param>
		public override void SetData(TreeNode<TreeViewCheckboxesItem> newNode, int depth)
		{
			Node = newNode;
			base.SetData(Node, depth);

			Item = (Node==null) ? null : Node.Item;
		}

		public void ToggleChanged()
		{
			Item.Selected = Checkbox.isOn;
			NodeCheckboxChanged.Invoke(Index);
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
				Checkbox.isOn = false;
			}
			else
			{
				Icon.sprite = Item.Icon;
				Text.text = Item.LocalizedName ?? Item.Name;
				Checkbox.isOn = Item.Selected;
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

		protected bool IsColorSetted;
		
		public virtual void Coloring(Color primary, Color background, float fadeDuration = 0f)
		{
			// reset default color to white, otherwise it will look darker than specified color,
			// because actual color = Text.color * Text.CrossFadeColor
			Text.color = Color.white;
			Background.color = Color.white;

			// change color instantly for first time
			Text.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Background.CrossFadeColor(background, IsColorSetted ? fadeDuration : 0f, true, true);

			IsColorSetted = true;
		}
	}
}