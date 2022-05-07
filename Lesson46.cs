using System;
using System.Collections.Generic;

namespace Tired
{
	public delegate void ItemHandler(Item item);

	class Program
	{
		static void Main(string[] args)
		{
			
		}
	}

	public class Client
    {
		private ShoppingCart _shoppingCart = new ShoppingCart();
		public int CurrentMoney { get; private set; } = 0;

		public void RemoveRandomItemFromCart()
        {
			CurrentMoney += _shoppingCart.RemoveRandomItem().Price;
		}
		
    }

	public class ShoppingCart
    {
		private List<Item> _itemList = new List<Item>();
		
		public Item RemoveRandomItem()
        {
			Item randomItem = _itemList[ new Random().Next(0,_itemList.Count) ];

			_itemList.Remove(randomItem);

			return randomItem;
        }
    }

	public struct Item
    {
		public string Name { get; private set; }
		public int Price { get; private set; }

		public Item(string name,int price)
        {
			Name = name;
			Price = price;
        }
    }
}
