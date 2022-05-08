using System;
using System.Collections.Generic;

namespace Tired
{
	class Program
	{
		static void Main(string[] args)
		{
			Kassa kassa = new Kassa();
			UserInputMenu menu = new UserInputMenu(kassa);

			menu.Update();
			
		}
	}

	public class UserInputMenu
	{
		private const int QueueSleepTime = 2000;
		private const string WordToExit = "выход";
		private const string WordToStart = "начать обслуживание";

		private string _wordToRead;
		private Kassa _kassa;

		public UserInputMenu(Kassa kassa)
        {
			_kassa = kassa;
        }

		public void Update()
        {
			while (_kassa != null && _wordToRead != WordToExit)
			{
				ShowMenu();

				_wordToRead = Console.ReadLine();

				Console.WriteLine();

				switch (_wordToRead)
				{
					case WordToExit:
						break;

					case WordToStart:
						AcceptClientCart();
						break;
				}
			}

		}

		private void AcceptClientCart()
        {
            while (_kassa.ClientCount != 0)
            {
				Console.WriteLine("\n----Обслуживание клиента----");
				Console.WriteLine("Итоговая сумма товаров клинта:"+_kassa.GetFirstClient().GetShoppigCart().GetCommonPrice()+" Р");
				Console.WriteLine("Количество товаров в корзине клиента:" + _kassa.GetFirstClient().GetShoppigCart().ItemAmount);
				Console.WriteLine("Счет клиента:" + _kassa.GetFirstClient().CurrentMoney + " Р");
				Console.WriteLine("----Обслуживание клиента----");

				if (_kassa.TryPayCart() == true)
                {
					Console.WriteLine("Клиент оплатил корзину товаров!");
                }
                else
                {
					Console.WriteLine("У клиента недостаточно средств на покупку товаров!\nСлучайный предмет был возвращен в магазин.");
					_kassa.RemoveRandomItem();
				}

				Console.WriteLine();

				System.Threading.Thread.Sleep(QueueSleepTime);
			}

			Console.WriteLine("Клиенты обслужены!");
		}

		private void ShowMenu()
        {
			Console.WriteLine("Система администрирования супермаркетом");
			Console.WriteLine("Введите <" + WordToExit + ">, чтобы выйти из программы.");
			Console.WriteLine("Введите <" + WordToStart + ">, чтобы начать обслуживание клиентов.");
			Console.WriteLine("Клиентов в очереди:"+_kassa.ClientCount);
		}
	}


	public class Kassa
	{
		private Queue<Client> _clients = new Queue<Client>();

		public int ClientCount
        {
			get { return _clients.Count; }
        }

		public Kassa()
        {
			for(int i = 0; i < 8; i++)
            {
				_clients.Enqueue(new Client());
			}
        }

		public bool TryPayCart()
        {
			Client client = _clients.Peek();

			if( client.TryPay() == true)
            {
				_clients.Dequeue();

				return true;
            }

			return false;
        }

		public void RemoveRandomItem() => _clients.Peek().RemoveRandomItemFromCart();

		public Client GetFirstClient() => _clients.Peek();

	}

	public class Client
	{
		
		private ShoppingCart _shoppingCart = new ShoppingCart();
		public int CurrentMoney { get; private set; } = 0;

		public Client()
		{
			float priceFactor = 0.7f;

			_shoppingCart.AddItem(ItemGiver.GiveRandomItems());

			CurrentMoney = (int)(_shoppingCart.GetCommonPrice() * priceFactor);
		}

		public bool TryPay()
        {
			if ( CurrentMoney >= _shoppingCart.GetCommonPrice() )
            {
				CurrentMoney -= _shoppingCart.GetCommonPrice();

				_shoppingCart.Clear();

				return true;
            }

			return false;
		}

		public void RemoveRandomItemFromCart()
		{
			Item item = _shoppingCart.RemoveRandomItem();
			CurrentMoney += item.Price;
		}

		public ShoppingCart GetShoppigCart() => _shoppingCart;
	}

	public class ShoppingCart
	{
		private List<Item> _itemList = new List<Item>();

		public int ItemAmount
        {
			get
			{
				return _itemList.Count;
			}
        }

		public void AddItem(Item item) => _itemList.Add(item);

		public void AddItem(Item[] items) => _itemList.AddRange(items);

		public void Clear() => _itemList.Clear();

		public Item RemoveRandomItem()
		{
			if(_itemList.Count == 0)
            {
				return default;
            }

			Item randomItem = _itemList[new Random().Next(0, _itemList.Count)];

			_itemList.Remove(randomItem);

			return randomItem;
		}

		public int GetCommonPrice()
		{
			int price = 0;

			foreach (Item item in _itemList)
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
