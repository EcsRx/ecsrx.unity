using UnityEngine;

namespace UIWidgetsSamples.Shops {
	/// <summary>
	/// JRPG order line.
	/// </summary>
	[System.Serializable]
	public class JRPGOrderLine : IOrderLine {
		[SerializeField]
		Item item;

		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		public Item Item {
			get {
				return item;
			}
			set {
				item = value;
			}
		}

		/// <summary>
		/// The price.
		/// </summary>
		[SerializeField]
		public int Price;

		[SerializeField]
		int count;

		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count {
			get {
				return count;
			}
			set {
				count = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UIWidgetsSamples.Shops.JRPGOrderLine"/> class.
		/// </summary>
		/// <param name="newItem">New item.</param>
		/// <param name="newPrice">New price.</param>
		public JRPGOrderLine(Item newItem, int newPrice)
		{
			item = newItem;
			Price = newPrice;
		}
	}
}