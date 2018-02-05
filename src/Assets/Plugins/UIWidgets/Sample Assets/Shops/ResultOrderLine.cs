using UnityEngine;
using System.Collections;

namespace UIWidgetsSamples.Shops {
	/// <summary>
	/// Result order line.
	/// </summary>
	public class ResultOrderLine : IOrderLine {
		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		public Item Item { get; set; }
		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count { get; set; }
	}
}