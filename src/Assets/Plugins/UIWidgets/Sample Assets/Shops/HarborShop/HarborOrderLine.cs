using UnityEngine;

namespace UIWidgetsSamples.Shops {

	/// <summary>
	/// Harbor order line.
	/// </summary>
	[System.Serializable]
	public class HarborOrderLine : IOrderLine {
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
		/// The buy price.
		/// </summary>
		[SerializeField]
		public int BuyPrice;

		/// <summary>
		/// The sell price.
		/// </summary>
		[SerializeField]
		public int SellPrice;

		/// <summary>
		/// The buy count.
		/// </summary>
		[SerializeField]
		public int BuyCount;

		/// <summary>
		/// The sell count.
		/// </summary>
		[SerializeField]
		public int SellCount;

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
		/// Initializes a new instance of the <see cref="UIWidgetsSamples.Shops.HarborOrderLine"/> class.
		/// </summary>
		/// <param name="newItem">New item.</param>
		/// <param name="buyPrice">Buy price.</param>
		/// <param name="sellPrice">Sell price.</param>
		/// <param name="buyCount">Buy count.</param>
		/// <param name="sellCount">Sell count.</param>
		public HarborOrderLine(Item newItem, int buyPrice, int sellPrice, int buyCount, int sellCount)
		{
			item = newItem;
			BuyPrice = buyPrice;
			SellPrice = sellPrice;
			BuyCount = buyCount;
			SellCount = sellCount;
		}
	}
}