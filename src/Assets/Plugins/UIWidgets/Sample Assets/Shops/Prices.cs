using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIWidgetsSamples.Shops {
	/// <summary>
	/// Prices.
	/// </summary>
	public class Prices {
		static Dictionary<string,int> prices = new Dictionary<string, int>();

		static Prices()
		{
			prices.Add("Stick", 5);
			prices.Add("Sword", 100);
			prices.Add("Short Sword", 75);
			prices.Add("Long Sword", 120);
			prices.Add("Knife", 50);
			prices.Add("Dagger", 80);
			prices.Add("Hammer", 150);
			prices.Add("Shield", 70);
			prices.Add("Leather Armor", 200);
			prices.Add("Ring", 20);
			prices.Add("Bow", 100);
			prices.Add("Crossbow", 120);
			
			prices.Add("HP Potion", 10);
			prices.Add("Mana Potion", 10);
			prices.Add("HP UP", 1000);
			prices.Add("Mana UP", 1000);

			prices.Add("Wood", 10);
			prices.Add("Wheat", 10);
			prices.Add("Fruits", 20);
			prices.Add("Sugar", 70);
			prices.Add("Metal", 10);
			prices.Add("Cotton", 20);
			prices.Add("Silver", 300);
			prices.Add("Gold", 500);
			prices.Add("Cocoa", 160);
			prices.Add("Coffee", 140);
			prices.Add("Tobacco", 120);
		}

		/// <summary>
		/// Gets the price.
		/// </summary>
		/// <returns>The price.</returns>
		/// <param name="item">Item.</param>
		/// <param name="priceFactor">Price factor.</param>
		public static int GetPrice(Item item, float priceFactor)
		{
			if (!prices.ContainsKey(item.Name))
			{
				return 1;
			}

			return Mathf.Max(1, Mathf.RoundToInt(prices[item.Name] * priceFactor));
		}
	}
}