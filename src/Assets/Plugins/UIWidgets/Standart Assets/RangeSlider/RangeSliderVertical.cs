using UnityEngine;

namespace UIWidgets
{
	/// <summary>
	/// Vertical range slider.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/RangeSliderVertical")]
	public class RangeSliderVertical : RangeSlider
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