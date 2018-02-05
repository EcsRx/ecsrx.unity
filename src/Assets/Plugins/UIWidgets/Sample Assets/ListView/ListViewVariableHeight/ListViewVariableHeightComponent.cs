using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

namespace UIWidgetsSamples {
	public class ListViewVariableHeightComponent : ListViewItem/*, IListViewItemHeight*/ {
		[SerializeField]
		public Text Name;

		[SerializeField]
		public Text Text;

		public float Height {
			get {
				return CalculateHeight();
			}
		}

		// Displaying item data
		public void SetData(ListViewVariableHeightItemDescription item)
		{
			Name.text = item.Name;
			Text.text = item.Text.Replace("\\n", "\n");
		}

		float CalculateHeight()
		{
			float default_total_height = 58;
			float default_name_height = 21;
			float default_text_height = 17;

			var base_height = default_total_height - default_name_height - default_text_height;

			var h = base_height + Name.preferredHeight + Text.preferredHeight;

			return h;
		}

		protected bool IsColorSetted;
		
		public virtual void Coloring(Color primary, Color background, float fadeDuration)
		{
			// reset default color to white, otherwise it will look darker than specified color,
			// because actual color = Text.color * Text.CrossFadeColor
			if (!IsColorSetted)
			{
				Name.color = Color.white;
				Text.color = Color.white;
				Background.color = Color.white;
			}

			// change color instantly for first time
			Name.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Text.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Background.CrossFadeColor(background, IsColorSetted ? fadeDuration : 0f, true, true);

			IsColorSetted = true;
		}
	}
}