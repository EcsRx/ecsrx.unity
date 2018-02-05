using System.Collections.Generic;
using System.Linq;
using UIWidgets;

namespace UIWidgetsSamples.Shops {
	/// <summary>
	/// JRPG order.
	/// </summary>
	public class JRPGOrder : IOrder {
		List<JRPGOrderLine> OrderLines;

		/// <summary>
		/// Initializes a new instance of the <see cref="UIWidgetsSamples.Shops.JRPGOrder"/> class.
		/// </summary>
		/// <param name="orderLines">Order lines.</param>
		public JRPGOrder(ObservableList<JRPGOrderLine> orderLines)
		{
			OrderLines = orderLines.Where(x => x.Count != 0).ToList();
		}

		/// <summary>
		/// Gets the order lines.
		/// </summary>
		/// <returns>The order lines.</returns>
		public List<IOrderLine> GetOrderLines()
		{
			return OrderLines.Convert(x => x as IOrderLine);
		}

		/// <summary>
		/// Order lines count.
		/// </summary>
		/// <returns>The lines count.</returns>
		public int OrderLinesCount()
		{
			return OrderLines.Count;
		}

		/// <summary>
		/// Total.
		/// </summary>
		public int Total()
		{
			return OrderLines.Sum(x => x.Count * x.Price);
		}
	}
}