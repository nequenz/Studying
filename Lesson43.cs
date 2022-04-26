using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            Person player = new Person("Игрок", 10);
            Person trader = new Person("Торгаш Ярик", 10);
            UserInputMenu menu = new UserInputMenu(trader, player);

            player.SetGold(5500);
            trader.SetGold(8900);

            trader.TryAddItem(ItemBase.Meet, 28);
            trader.TryAddItem(ItemBase.Bow, 3);
            trader.TryAddItem(ItemBase.Axe, 1);
            trader.TryAddItem(ItemBase.Sword, 5);
            trader.TryAddItem(ItemBase.Beer, 13);
            trader.TryAddItem(ItemBase.Bread, 45);
            trader.TryAddItem(ItemBase.Stick, 18);

            menu.Update();
        }
    }

    class UserInputMenu
    {
        private const string WordToExit = "exit";
        private const string WordToBuy = "buy";
        private const string WordToSell = "sell";
        private const string WordToShow = "show";

        private string _wordToRead;
        private Person _trader;
        private Person _player;
        private bool _isPlayerInventoryShown = false;

        public UserInputMenu(Person trader, Person player)
        {
            _player = player;
            _trader = trader;
        }

        public void Update()
        {
            while (_player != null && _trader != null && _wordToRead != WordToExit)
            {
                ShowMenu();

                Console.Write("\nВвод:");

                _wordToRead = Console.ReadLine();

                Console.WriteLine();

                switch (_wordToRead)
                {
                    case WordToExit:
                        break;

                    case WordToBuy:
                        BuyItemByUser();
                        break;

                    case WordToSell:
                        SellItemByUser();
                        break;

                    case WordToShow:
                        SetPlayerInventoryShowModeByUser();
                        break;
                }
            }
        }

        public void SetPlayerInventoryShowModeByUser()
        {
            Console.Write("\nПоказать инвентарь(да,нет):");

            if (Console.ReadLine() == "да")
            {
                _isPlayerInventoryShown = true;
            }
            else if (Console.ReadLine() == "нет")
            {
                _isPlayerInventoryShown = false;
            }
        }

        public void ReadItemByUser(out int itemID, out int amount)
        {
            Console.Write("\nВведите имя предмета:");

            itemID = ItemBase.GetItemIDByName(Console.ReadLine());

            if (itemID == Item.EmptyID)
            {
                Console.WriteLine("\nТакого предмета не существует в игре!");

                amount = 0;

                return;
            }

            Console.Write("\nВведите количество:");

            if (Int32.TryParse(Console.ReadLine(), out amount) == true)
            {
                Console.WriteLine("\nДанные верны");
            }
        }

        public void BuyItemByUser()
        {
            Console.WriteLine("Покупка предмета:");

            ReadItemByUser(out int itemID, out int amount);

            if (_player.TryBuyItem(itemID, amount, _trader) == true)
            {
                Console.WriteLine("Покупка совершена!");
            }
            else
            {
                Console.WriteLine("Произошла ошибка..");
            }
        }

        public void SellItemByUser()
        {
            Console.WriteLine("Продажа предмета:");

            ReadItemByUser(out int itemID, out int amount);

            if (_player.TrySellItemTo(itemID, amount, _trader))
            {
                Console.WriteLine("Произошла ошибка..");
            }
        }

        public void ShowMenu()
        {
            Console.WriteLine("Вы классическом магазине 15-го века и торгуете с продавцом!");
            Console.WriteLine(" *Введите " + WordToBuy + " для покупки предмета");
            Console.WriteLine(" *Введите " + WordToSell + " для продажи предмета");
            Console.WriteLine(" *Введите " + WordToShow + " чтобы посмотреть свой инвентарь");
            Console.WriteLine(" *Введите " + WordToExit + " чтобы выйти");
            _trader.ShowInventory();

            if (_isPlayerInventoryShown == true)
            {
                Console.WriteLine("Ваш инвентарь");
                _player.ShowInventory();
            }

            Console.WriteLine();
        }
    }

    class Person
    {
        private Inventory _currentInventory;

        public string Name { get; private set; }
        public int Gold { get; private set; } = 0;

        public Person(string name, int inventorySize)
        {
            Name = name;
            _currentInventory = new Inventory(inventorySize);
        }

        public void SetGold(int gold) => Gold = gold;

        public bool TrySellItemTo(int itemID, int amount, Person person)
        {
            int commonPrice;

            if (_currentInventory.HasItem(itemID, amount) == false)
            {
                return false;
            }

            commonPrice = ItemBase.GetItemByID(itemID).Price * amount;

            if (commonPrice <= person.Gold && _currentInventory.TryMoveItemTo(itemID, amount, person._currentInventory))
            {
                person.SetGold(person.Gold - commonPrice);
                SetGold(Gold + commonPrice);

                Console.WriteLine("Персонаж " + person.Name + " заработал " + commonPrice + " золота.");

                return true;
            }

            Console.WriteLine("Персонаж " + person.Name + " не обладает таким количеством золота...");

            return false;
        }

        public bool TryBuyItem(int itemID, int amount, Person person) => person.TrySellItemTo(itemID, amount, this);

        public bool TryAddItem(int itemID, int amount) => _currentInventory.TryAddItem(itemID, amount);

        public void ShowInventory()
        {
            Console.WriteLine("\nПерсонаж:" + Name);
            Console.WriteLine("Текущее золото:" + Gold);
            _currentInventory.ShowItems();
        }
    }

    class Inventory
    {
        private const int DefaultMaxCells = 16;

        private List<ItemCell> _cells = new List<ItemCell>();

        public int MaxCells { get; private set; }

        public Inventory(int maxCells = DefaultMaxCells)
        {
            MaxCells = maxCells;

            Clear();
        }

        public void Clear()
        {
            _cells.Clear();

            for (int i = 0; i < MaxCells; i++)
            {
                _cells.Add(new ItemCell(64));
            }
        }

        public bool TryAddItem(int itemID, int amount)
        {
            ItemCell neededCell = null;

            if (itemID == Item.EmptyID)
            {
                return false;
            }

            foreach (ItemCell cell in _cells)
            {
                if ((cell.SavedItemID == itemID) || (neededCell == null && cell.SavedItemID == Item.EmptyID))
                {
                    neededCell = cell;
                }
            }

            if (neededCell != null && neededCell.TryAddItem(itemID, amount, out int overAmount) == true)
            {
                if (overAmount > 0)
                {
                    TryAddItem(itemID, overAmount);
                }

                return true;
            }

            return false;
        }

        public bool TryRemoveItem(int itemID, int amount)
        {
            ItemCell cell = GetFirstItemCellByItemID(itemID);

            if (cell != null)
            {
                cell.SetAmount(cell.Amount - amount);

                return true;
            }

            return false;
        }

        public bool TryMoveItemTo(int itemID, int amount, Inventory otherInventory)
        {
            ItemCell cell = GetFirstItemCellByItemID(itemID);

            if (cell == null || otherInventory == null || cell.Amount < amount)
            {
                return false;
            }

            if (otherInventory.TryAddItem(cell.SavedItemID, amount) == true)
            {
                cell.SetAmount(cell.Amount - amount);

                return true;
            }

            return false;
        }

        public void ShowItems()
        {
            Console.WriteLine("Содержимое инвентаря:");

            foreach (ItemCell cell in _cells)
            {
                Console.WriteLine(cell.GetItemInfo());
            }
        }

        public bool HasItem(int itemID, int amount)
        {
            ItemCell itemCell = GetFirstItemCellByItemID(itemID);

            return itemCell != null && itemCell.Amount >= amount;
        }

        private ItemCell GetFirstItemCellByItemID(int itemID)
        {
            if (itemID == Item.EmptyID)
            {
                return null;
            }

            foreach (ItemCell cell in _cells)
            {
                if (cell.SavedItemID == itemID)
                {
                    return cell;
                }
            }

            return null;
        }
    }

    class ItemCell
    {
        private const int DefaultAmount = 16;

        private int _amount;

        public int SavedItemID { get; private set; } = Item.EmptyID;
        public int MaxAmount { get; private set; }
        public int Amount
        {
            get
            {
                return _amount;
            }

            private set
            {
                if (SavedItemID == Item.EmptyID)
                {
                    _amount = 0;
                }
                else if (value > MaxAmount)
                {
                    _amount = MaxAmount;
                }
                else if (value <= 0)
                {
                    SavedItemID = Item.EmptyID;
                    _amount = 0;
                }
                else
                {
                    _amount = value;
                }
            }
        }

        public ItemCell(int maxCount = DefaultAmount)
        {
            MaxAmount = maxCount;
        }

        public void SetItemID(int itemID) => SavedItemID = itemID;

        public void SetAmount(int amount) => Amount = amount;

        public void DeleteItem() => SetAmount(0);

        public bool TryMoveItemTo(ItemCell itemCell)
        {
            if (itemCell != null)
            {
                itemCell.SetItemID(SavedItemID);
                itemCell.SetAmount(Amount);
                DeleteItem();

                return true;
            }

            return false;
        }

        public bool TryAddItem(int itemID, int amount, out int overAmount)
        {
            if (SavedItemID != itemID && SavedItemID != Item.EmptyID)
            {
                overAmount = 0;

                return false;
            }

            int resultAmount = Amount + amount;

            overAmount = resultAmount > MaxAmount ? (resultAmount - MaxAmount) : 0;
            SetItemID(itemID);
            SetAmount(resultAmount);

            return true;
        }

        public void IncrementAmount() => Amount++;

        public void DecrementAmount() => Amount--;

        public Item GetItem() => ItemBase.GetItemByID(SavedItemID);

        public string GetItemInfo() => (SavedItemID == Item.EmptyID) ? "Пустой слот" : GetItem().Name + ":" + Amount;
    }

    static class ItemBase
    {
        public static readonly int Bread;
        public static readonly int Beer;
        public static readonly int Meet;
        public static readonly int Sword;
        public static readonly int Bow;
        public static readonly int Stick;
        public static readonly int Axe;

        private static Dictionary<int, Item> _itemsDictionary = new Dictionary<int, Item>();

        static ItemBase()
        {
            Bread = CreateNewItem(new Item("Хлеб", 44));
            Beer = CreateNewItem(new Item("Пиво", 98));
            Meet = CreateNewItem(new Item("Мясо", 235));
            Sword = CreateNewItem(new Item("Меч", 2300));
            Bow = CreateNewItem(new Item("Лук", 2560));
            Stick = CreateNewItem(new Item("Палка", 56));
            Axe = CreateNewItem(new Item("Топор", 2900));
        }

        public static int CreateNewItem(Item item)
        {
            if (item == null)
            {
                return Item.EmptyID;
            }

            _itemsDictionary.Add(item.ID, item);

            return item.ID;
        }

        public static Item GetItemByID(int itemID)
        {
            if (_itemsDictionary.TryGetValue(itemID, out Item item) == true)
            {
                return item;
            }

            return null;
        }

        public static int GetItemIDByName(string name)
        {
            foreach (KeyValuePair<int, Item> pair in _itemsDictionary)
            {
                if (pair.Value.Name == name)
                {
                    return pair.Value.ID;
                }
            }

            return Item.EmptyID;
        }
    }

    class Item
    {
        public const int EmptyID = -1;

        private static int _nextID = 0;

        public string Name { get; private set; }
        public int ID { get; private set; }
        public int Price { get; private set; }

        public Item(string name, int price)
        {
            Name = name;
            Price = price;
            ID = _nextID;
            _nextID++;
        }
    }
}
