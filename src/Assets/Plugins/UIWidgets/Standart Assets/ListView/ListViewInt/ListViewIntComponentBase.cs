using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets {
	/// <summary>
	/// ListViewInt base component.
	/// </summary>
	abstract public class ListViewIntComponentBase : ListViewItem {

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="item">Item.</param>
		abstract public void SetData(int item);

		/// <summary>
		/// Sets specified colors.
		/// </summary>
		/// <param name="primary">Primary color.</param>
		/// <param name="background">Background color.</param>
		/// <param name="fadeDuration">Fade duration.</param>
		abstract public void Coloring(Color primary, Color background, float fadeDuration);
	}
}