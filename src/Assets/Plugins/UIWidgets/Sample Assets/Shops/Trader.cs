using UIWidgets;

namespace UIWidgetsSamples.Shops {
	/// <summary>
	/// On items change.
	/// </summary>
	public delegate void OnItemsChange();

	/// <summary>
	/// On money change.
	/// </summary>
	public delegate void OnMoneyChange();

    /// <summary>
    /// Trader.
    /// </summary>
    public class Trader {
        int money;

        /// <summary>
        /// Gets or sets the trader money. -1 to infinity money
        /// </summary>
        /// <value>The money.</value>
        public int Money {
            get {
                return money;
            }
            set {
                if (money == -1)
                {
                    MoneyChanged();
                    return;
                }
                money = value;
                MoneyChanged();
            }
        }

        ObservableList<Item> inventory = new ObservableList<Item>();

        /// <summary>
        /// Gets or sets the inventory.
        /// </summary>
        /// <value>The inventory.</value>
        public ObservableList<Item> Inventory {
            get {
                return inventory;
            }
            set {
                if (inventory != null)
                {
                    inventory.OnChange -= ItemsChanged;
                }
                inventory = value;
                if (inventory != null)
                {
                    inventory.OnChange += ItemsChanged;
                }
                ItemsChanged();
            }
        }

        /// <summary>
        /// The price factor.
        /// </summary>
        public float PriceFactor = 1;

        /// <summary>
        /// The delete items if Item.count = 0.
        /// </summary>
        public bool DeleteIfEmpty = true;

        /// <summary>
        /// Occurs when data changed.
        /// </summary>
        public event OnItemsChange OnItemsChange = () => { };

        /// <summary>
        /// Occurs when money changed.
        /// </summary>
        public event OnMoneyChange OnMoneyChange = () => { };

		/// <summary>
		/// Initializes a new instance of the <see cref="UIWidgetsSamples.Shops.Trader"/> class.
		/// </summary>
		/// <param name="deleteIfEmpty">If set to <c>true</c> delete if empty.</param>
		public Trader(bool deleteIfEmpty = true)
		{
			DeleteIfEmpty = deleteIfEmpty;
			inventory.OnChange += ItemsChanged;
		}

		void ItemsChanged()
		{
			OnItemsChange();
		}

		void MoneyChanged()
		{
			OnMoneyChange();
		}

		/// <summary>
		/// Sell the specified order.
		/// </summary>
		/// <param name="order">Order.</param>
		public void Sell(IOrder order)
		{
			if (order.OrderLinesCount()==0)
			{
				return ;
			}

			Inventory.BeginUpdate();
			order.GetOrderLines().ForEach(SellItem);
			Inventory.EndUpdate();

			Money += order.Total();
		}

		/// <summary>
		/// Sells the item.
		/// </summary>
		/// <param name="orderLine">Order line.</param>
		void SellItem(IOrderLine orderLine)
		{
			var count = orderLine.Count;

			// decrease items count
			orderLine.Item.Count -= count;

			// remove item from inventory if zero count
			if (DeleteIfEmpty && (orderLine.Item.Count==0))
			{
				Inventory.Remove(orderLine.Item);
			}
		}

		/// <summary>
		/// Buy the specified order.
		/// </summary>
		/// <param name="order">Order.</param>
		public void Buy(IOrder order)
		{
			if (order.OrderLinesCount()==0)
			{
				return ;
			}

			Inventory.BeginUpdate();
			order.GetOrderLines().ForEach(BuyItem);
			Inventory.EndUpdate();

			Money -= order.Total();
		}

		/// <summary>
		/// Buy the item.
		/// </summary>
		/// <param name="orderLine">Order line.</param>
		void BuyItem(IOrderLine orderLine)
		{
			// find item in inventory
			var item = Inventory.Find(x => x.Name==orderLine.Item.Name);

			var count = orderLine.Count;
			// if not found add new item to inventory
			if (item==null)
			{
				Inventory.Add(new Item(orderLine.Item.Name, count));
			}
			// if found increase count
			else
			{
				item.Count += count;
			}
		}

		/// <summary>
		/// Determines whether this instance can buy the specified order.
		/// </summary>
		/// <returns><c>true</c> if this instance can buy the specified order; otherwise, <c>false</c>.</returns>
		/// <param name="order">Order.</param>
		public bool CanBuy(IOrder order)
		{
			return Money==-1 || Money>=order.Total();
		}
	}
}