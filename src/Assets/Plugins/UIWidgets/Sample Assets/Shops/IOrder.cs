using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIWidgetsSamples.Shops {

	/// <summary>
	/// IOrder.
	/// </summary>
	public interface IOrder {
		/// <summary>
		/// Gets the order lines.
		/// </summary>
		/// <returns>The order lines.</returns>
		List<IOrderLine> GetOrderLines();

		/// <summary>
		/// Order lines count.
		/// </summary>
		/// <returns>The lines count.</returns>
		int OrderLinesCount();

		/// <summary>
		/// Total.
		/// </summary>
		int Total();
	}
}