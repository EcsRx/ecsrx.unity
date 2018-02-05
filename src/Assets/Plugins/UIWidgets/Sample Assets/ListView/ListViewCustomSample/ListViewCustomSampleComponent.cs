using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

namespace UIWidgetsSamples {
	public class ListViewCustomSampleComponent : ListViewItem {
		// specify components for displaying item data
		[SerializeField]
		public Image Icon;

		[SerializeField]
		public Text Text;

		[SerializeField]
		public Progressbar Progressbar;

		// Displaying item data
		public void SetData(ListViewCustomSampleItemDescription item)
		{
			if (item==null)
			{
				Icon.sprite = null;
				Text.text = string.Empty;
				Progressbar.Value = 0;
			}
			else
			{
				Icon.sprite = item.Icon;
				Text.text = item.Name;
				Progressbar.Value = item.Progress;
			}

			Icon.SetNativeSize();
			//set transparent color if no icon
			Icon.color = (Icon.sprite==null) ? Color.clear : Color.white;
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