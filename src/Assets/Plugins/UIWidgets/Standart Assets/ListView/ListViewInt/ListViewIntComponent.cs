using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets {
	/// <summary>
	/// ListViewInt component.
	/// </summary>
	public class ListViewIntComponent : ListViewIntComponentBase {

		/// <summary>
		/// The number.
		/// </summary>
		[SerializeField]
		public Text Number;

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="item">Item.</param>
		public override void SetData(int item)
		{
			Number.text = item.ToString();
		}

		/// <summary>
		/// Is color setted.
		/// </summary>
		protected bool IsColorSetted;

		/// <summary>
		/// Sets specified colors.
		/// </summary>
		/// <param name="primary">Primary color.</param>
		/// <param name="background">Background color.</param>
		/// <param name="fadeDuration">Fade duration.</param>
		public override void Coloring(Color primary, Color background, float fadeDuration)
		{
			// reset default color to white, otherwise it will look darker than specified color,
			// because actual color = Text.color * Text.CrossFadeColor
			if (!IsColorSetted)
			{
				Number.color = Color.white;
				Background.color = Color.white;
			}

			// change color instantly for first time
			Number.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Background.CrossFadeColor(background, IsColorSetted ? fadeDuration : 0f, true, true);

			IsColorSetted = true;
		}
	}
}