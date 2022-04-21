using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            ItemBase currentItemBase = new ItemBase();



        }
    }

    class Trader : Inventoriable
    {
        


    }

    abstract class Inventoriable
    {
        public Inventory Inventory { get; private set; } = new Inventory();
    }

    class Inventory
    {
        private const int DefaultMaxCells = 64;

        private List<ItemCell> _cells = new List<ItemCell>();

        public int MaxCells { get; private set; }
        
        public Inventory(int maxCells = DefaultMaxCells)
        {
            MaxCells = maxCells;
        }

        public bool TryAddItem(int itemID,int amount)
        {
            ItemCell neededCell = null;

            if(itemID == Item.EmptyID)
            {
                return false;
            }

            foreach(ItemCell cell in _cells)
            {
                if( (cell.SavedItemID == itemID) || (neededCell == null && cell.SavedItemID == Item.EmptyID) )
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

    }

    class ItemCell
    {
        private const int DefaultAmount = 16;

        private int _amount;

        public int SavedItemID { get; private set; } = Item.EmptyID;
        public int MaxAmount { get; private set; }
        public int Amount
        {
            get { return _amount; }

            private set
            {
                if(SavedItemID == Item.EmptyID)
                {
                    _amount = 0;
                }
                else if (value > MaxAmount)
                {
                    _amount = value;
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

        public void SetItem(int itemID) => SavedItemID = itemID;

        public void SetAmount(int amount) => Amount = amount;

        public void DeleteItem() => SetAmount(0);

        public bool TryMoveItemTo(ItemCell itemCell)
        {
            if(itemCell != null)
            {
                itemCell.SetItem(SavedItemID);
                itemCell.SetAmount(Amount);
                DeleteItem();

                return true;
            }

            return false;
        }

        public bool TryAddItem(int itemID, int amount, out int overAmount)
        {
            if(SavedItemID != itemID || SavedItemID != Item.EmptyID)
            {
                overAmount = 0;

                return false;
            }

            int resultAmount = Amount + amount;

            overAmount = resultAmount > MaxAmount ? (resultAmount - MaxAmount) : 0;

            SetItem(itemID);
            SetAmount(resultAmount);

            return true;
        }

        public void IncrementAmount() => Amount++;

        public void DecrementAmount() => Amount--;
    }

    class ItemBase
    {
        private Dictionary<int,Item> _itemsDictionary = new Dictionary<int,Item>();

        public int CreateNewItem(Item item)
        {
            if(item == null)
            {
                return Item.EmptyID;
            }

            _itemsDictionary.Add(item.ID,item);
                
            return item.ID;
        }

        public Item GetItemByID(int itemID)
        {
            if( _itemsDictionary.TryGetValue(itemID, out Item item) == true )
            {
                return item;
            }

            return null;
        }
    }

    class Item
    {
        public const int EmptyID = -1;

        private static int NextID = 0;
        public string Name { get; private set; } = "Безымянный";
        public int ID { get; private set; }

        public Item()
        {
            ID = NextID;
            NextID++;
        }
    }

}
