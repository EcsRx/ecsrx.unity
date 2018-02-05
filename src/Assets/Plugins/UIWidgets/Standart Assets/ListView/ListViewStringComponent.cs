using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UIWidgets {

	/// <summary>
	/// List view item component.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ListViewStringComponent")]
	public class ListViewStringComponent : ListViewItem
	{
		/// <summary>
		/// The Text component.
		/// </summary>
		public Text Text;

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="text">Text.</param>
		public virtual void SetData(string text)
		{
			Text.text = text.Replace("\\n", "\n");
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
	}
}