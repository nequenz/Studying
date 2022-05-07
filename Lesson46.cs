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

	public class UserInputMenu
    {

    }

	public class Kassa
    {
		private Queue<Client> _clients = new Queue<Client>();


    }

	public class Client
	{
		//public event ItemHandler RemoveEvent;
		private ShoppingCart _shoppingCart = new ShoppingCart();
		public int CurrentMoney { get; private set; } = 0;

		public Client()
        {
			float priceFactor = 0.7f;

			_shoppingCart.AddItem(ItemGiver.GiveRandomItems());

			CurrentMoney = (int)(_shoppingCart.GetCommonPrice() * priceFactor);
        }

		public void RemoveRandomItemFromCart()
		{
			Item item = _shoppingCart.RemoveRandomItem();
			CurrentMoney += item.Price;

			//RemoveEvent?.Invoke(item);
		}
	}

	public class ShoppingCart
	{
		private List<Item> _itemList = new List<Item>();

		public void AddItem(Item item) => _itemList.Add(item);

		public void AddItem(Item[] items) => _itemList.AddRange(items);

		public Item RemoveRandomItem()
		{
			Item randomItem = _itemList[new Random().Next(0, _itemList.Count)];

			_itemList.Remove(randomItem);

			return randomItem;
		}

		public int GetCommonPrice()
        {
			int price = 0;

			foreach(Item item in _itemList)
            {
				price += item.Price;
            }

			return price;
        }
	}

	public struct Item
	{
		public string Name { get; private set; }
		public int Price { get; private set; }

		public Item(string name, int price)
		{
			Name = name;
			Price = price;
		}
	}

	public static class ItemGiver
    {
		private static Item[] _items = {

			new Item("Хлеб",35),
			new Item("Молоко",84),
			new Item("Мясо",321),
			new Item("Бутылка воды",39),
			new Item("Шоколад",110),
			new Item("Чай",79),
			new Item("Фисташки",240),
			new Item("Кола",95),
			new Item("Пицца",278),
			new Item("Рис",67),
		};

		public static Item GiveRandomItem() => _items[new Random().Next(0, _items.Length)];

		public static Item[] GiveRandomItems()
        {
			int randomMin = 5;
			int randomMax = 15;

			int randomLength = new Random().Next(randomMin, randomMax);
			Item[] items = new Item[randomLength];

			for (int i = 0; i < randomLength; i++)
			{
				items[i] = GiveRandomItem();
            }

			return items;
        }
	}

}

