using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

namespace UIWidgetsSamples {
	public class ListViewUnderlineSampleComponent : ListViewItem {
		// specify components for displaying item data
		[SerializeField]
		public Image Icon;
		
		[SerializeField]
		public Text Text;
		
		[SerializeField]
		public Image Underline;
		
		// Displaying item data
		public void SetData(ListViewUnderlineSampleItemDescription item)
		{
			Icon.sprite = item.Icon;
			Text.text = item.Name;

			Icon.SetNativeSize();
			
			Icon.color = (Icon.sprite==null) ? Color.clear : Color.white;
		}

		protected bool IsColorSetted;
		
		public virtual void Coloring(Color primary, Color background, float fadeDuration)
		{
			// reset default color to white, otherwise it will look darker than specified color,
			// because actual color = Text.color * Text.CrossFadeColor
			if (!IsColorSetted)
			{
				Underline.color = Color.white;
				Text.color = Color.white;
			}

			// change color instantly for first time
			Underline.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Text.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);

			IsColorSetted = true;
		}
	}
}