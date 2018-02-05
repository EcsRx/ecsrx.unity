using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TreeViewSampleComponent : TreeViewComponentBase<ITreeViewSampleItem> {
		ITreeViewSampleItem item;
		
		public ITreeViewSampleItem Item {
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
		
		public override void SetData(TreeNode<ITreeViewSampleItem> node, int depth)
		{
			base.SetData(node, depth);
			
			Item = (node==null) ? null : node.Item;
		}
		
		protected virtual void UpdateView()
		{
			if (Item==null)
			{
				Icon.sprite = null;
				Text.text = string.Empty;
			}
			else
			{
				Item.Display(this);
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (item!=null)
			{
				item.OnChange -= UpdateView;
			}
		}

		protected bool IsColorSetted;
		
		public virtual void Coloring(Color primary, Color background, float fadeDuration)
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
	}
}