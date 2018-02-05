using System.Collections.Generic;
using System.Linq;
using UIWidgets;

namespace UIWidgetsSamples.Shops {
	/// <summary>
	/// Harbor order.
	/// </summary>
	public class HarborOrder : IOrder {
		/// <summary>
		/// The order lines.
		/// </summary>
		List<HarborOrderLine> OrderLines;

		/// <summary>
		/// Initializes a new instance of the <see cref="UIWidgetsSamples.Shops.HarborOrder"/> class.
		/// </summary>
		/// <param name="orderLines">Order lines.</param>
		public HarborOrder(ObservableList<HarborOrderLine> orderLines)
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
			return OrderLines.Sum(x => x.Count * ((x.Count > 0) ? x.BuyPrice : x.SellPrice));
		}
	}
}