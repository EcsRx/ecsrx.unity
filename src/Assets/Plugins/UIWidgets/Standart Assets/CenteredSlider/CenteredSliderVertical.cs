using UnityEngine;

namespace UIWidgets {
	/// <summary>
	/// Centered vertical slider (zero at center, positive and negative parts have different scales).
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/CenteredSliderVertical")]
	public class CenteredSliderVertical : CenteredSlider
	{
		/// <summary>
		/// Determines whether this instance is horizontal.
		/// </summary>
		/// <returns><c>true</c> if this instance is horizontal; otherwise, <c>false</c>.</returns>
		protected override bool IsHorizontal()
		{
			return false;
		}
	}
}