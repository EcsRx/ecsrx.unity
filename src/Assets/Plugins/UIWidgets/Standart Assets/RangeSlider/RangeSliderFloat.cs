using UnityEngine;
using System.Collections;

namespace UIWidgets {

	/// <summary>
	/// Range slider with float values.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/RangeSliderFloat")]
	public class RangeSliderFloat : RangeSliderBase<float>
	{
		/// <summary>
		/// Value to position.
		/// </summary>
		/// <returns>Position.</returns>
		/// <param name="value">Value.</param>
		protected override float ValueToPosition(float value)
		{
			var points_per_unit = FillSize() / (limitMax - limitMin);
			
			return points_per_unit * (InBounds(value) - limitMin) + GetStartPoint();
		}

		/// <summary>
		/// Position to value.
		/// </summary>
		/// <returns>Value.</returns>
		/// <param name="position">Position.</param>
		protected override float PositionToValue(float position)
		{
			var points_per_unit = FillSize() / (limitMax - limitMin);
			
			var value = position / points_per_unit + LimitMin;

			if (WholeNumberOfSteps)
			{
				return InBounds(Mathf.Round(value / step) * step);
			}
			else
			{
				return InBounds(value);
			}
		}

		/// <summary>
		/// Position range for minimum handle.
		/// </summary>
		/// <returns>The position limits.</returns>
		protected override Vector2 MinPositionLimits()
		{
			return new Vector2(ValueToPosition(LimitMin), ValueToPosition(valueMax - step));
		}

		/// <summary>
		/// Position range for maximum handle.
		/// </summary>
		/// <returns>The position limits.</returns>
		protected override Vector2 MaxPositionLimits()
		{
			return new Vector2(ValueToPosition(valueMin + step), ValueToPosition(limitMax));
		}

		/// <summary>
		/// Fit value to bounds.
		/// </summary>
		/// <returns>Value.</returns>
		/// <param name="value">Value.</param>
		protected override float InBounds(float value)
		{
			if (value < limitMin)
			{
				return limitMin;
			}
			if (value > limitMax)
			{
				return limitMax;
			}
			return value;
		}

		/// <summary>
		/// Fit minumum value to bounds.
		/// </summary>
		/// <returns>Value.</returns>
		/// <param name="value">Value.</param>
		protected override float InBoundsMin(float value)
		{
			if (value < limitMin)
			{
				return limitMin;
			}
			if (value > valueMax)
			{
				return valueMax;
			}
			return value;
		}

		/// <summary>
		/// Fit maximum value to bounds.
		/// </summary>
		/// <returns>Value.</returns>
		/// <param name="value">Value.</param>
		protected override float InBoundsMax(float value)
		{
			if (value < valueMin)
			{
				return valueMin;
			}
			if (value > limitMax)
			{
				return limitMax;
			}
			return value;
		}

		/// <summary>
		/// Increases the minimum value.
		/// </summary>
		protected override void IncreaseMin()
		{
			ValueMin += step;
		}

		/// <summary>
		/// Decreases the minimum value.
		/// </summary>
		protected override void DecreaseMin()
		{
			ValueMin -= step;
		}

		/// <summary>
		/// Increases the maximum value.
		/// </summary>
		protected override void IncreaseMax()
		{
			ValueMax += step;
		}

		/// <summary>
		/// Decreases the maximum value.
		/// </summary>
		protected override void DecreaseMax()
		{
			ValueMax -= step;
		}
	}
}