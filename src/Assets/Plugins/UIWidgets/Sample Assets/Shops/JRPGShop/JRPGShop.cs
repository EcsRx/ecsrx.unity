using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UIWidgets;

namespace UIWidgetsSamples.Shops {
	/// <summary>
	/// JRPG shop.
	/// </summary>
	public class JRPGShop : MonoBehaviour {
		Trader Shop;

		[SerializeField]
		TraderListView ShopItems;

		[SerializeField]
		Button BuyButton;

		[SerializeField]
		Text ShopTotal;

		Trader Player;

		[SerializeField]
		TraderListView PlayerItems;

		[SerializeField]
		Button SellButton;

		[SerializeField]
		Text PlayerMoney;

		[SerializeField]
		Text PlayerTotal;

		[SerializeField]
		Notify notify;

		void Start()
		{
			Shop = new Trader();
			Player = new Trader();

			Init();

			BuyButton.onClick.AddListener(Buy);
			SellButton.onClick.AddListener(Sell);

			Shop.OnItemsChange += UpdateShopItems;

			Player.OnItemsChange += UpdatePlayerItems;
			Player.OnMoneyChange += UpdatePlayerMoneyInfo;

			UpdateShopItems();

			UpdatePlayerItems();
			UpdatePlayerMoneyInfo();
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init()
		{
			Shop.Money = -1;
			Shop.PriceFactor = 1;
			Shop.Inventory.Clear();
			var shop_items = new List<Item>(){
				new Item("Sword", 10),
				new Item("Short Sword", 5),
				new Item("Long Sword", 5),
				new Item("Knife", -1),
				new Item("Dagger", -1),
				new Item("Hammer", -1),
				new Item("Shield", -1),
				new Item("Leather Armor", 3),
				new Item("Ring", 2),
				new Item("Bow", -1),
				new Item("Crossbow", -1),

				new Item("HP Potion", -1),
				new Item("Mana Potion", -1),
				new Item("HP UP", 10),
				new Item("Mana UP", 10),
			};
			Shop.Inventory.AddRange(shop_items);

			Player.Money = 2000;
			Player.PriceFactor = 0.5f;
			Player.Inventory.Clear();
			var player_items = new List<Item>(){
				new Item("Stick", 1),
				new Item("Sword", 1),
				new Item("HP Potion", 5),
				new Item("Mana Potion", 5),
			};
			Player.Inventory.AddRange(player_items);
		}

		/// <summary>
		/// Updates the shop total.
		/// </summary>
		public void UpdateShopTotal()
		{
			var order = new JRPGOrder(ShopItems.DataSource);
			ShopTotal.text = order.Total().ToString();
		}

		/// <summary>
		/// Updates the player total.
		/// </summary>
		public void UpdatePlayerTotal()
		{
			var order = new JRPGOrder(PlayerItems.DataSource);
			PlayerTotal.text = order.Total().ToString();
		}

		ObservableList<JRPGOrderLine> CreateOrderLines(Trader trader)
		{
			return trader.Inventory.Convert(item => {
				return new JRPGOrderLine(item, Prices.GetPrice(item, trader.PriceFactor));
			});
		}

		void UpdateShopItems()
		{
			ShopItems.DataSource = CreateOrderLines(Shop);
		}

		void UpdatePlayerItems()
		{
			PlayerItems.DataSource = CreateOrderLines(Player);
		}

		void UpdatePlayerMoneyInfo()
		{
			PlayerMoney.text = Player.Money.ToString();
		}

		void Buy()
		{
			var order = new JRPGOrder(ShopItems.DataSource);

			if (Player.CanBuy(order))
			{
				Shop.Sell(order);
				Player.Buy(order);
			}
			else
			{
				var message = string.Format("Not enough money to buy items. Available: {0}; Required: {1}", Player.Money, order.Total());
				notify.Template().Show(message, customHideDelay: 3f, sequenceType: NotifySequence.First, clearSequence: true);
			}
		}

		void Sell()
		{
			var order = new JRPGOrder(PlayerItems.DataSource);

			if (Shop.CanBuy(order))
			{
				Shop.Buy(order);
				Player.Sell(order);
			}
			else
			{
				var message = string.Format("Not enough money in shop to sell items. Available: {0}; Required: {1}", Shop.Money, order.Total());
				notify.Template().Show(message, customHideDelay: 3f, sequenceType: NotifySequence.First, clearSequence: true);
			}
		}

		void OnDestroy()
		{
			if (BuyButton!=null)
			{
				BuyButton.onClick.RemoveListener(Buy);
			}
			if (SellButton!=null)
			{
				SellButton.onClick.RemoveListener(Sell);
			}

			if (Shop!=null)
			{
				Shop.OnItemsChange -= UpdateShopItems;
			}

			if (Player!=null)
			{
				Player.OnItemsChange -= UpdatePlayerItems;
				Player.OnMoneyChange -= UpdatePlayerMoneyInfo;
			}
		}

	}
}