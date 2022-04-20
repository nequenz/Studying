using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
      
        }
    }

    class Trader
    {

    }

    class Invetory
    {
        List<InventoryCell> cells = new List<InventoryCell>();
    }

    class InventoryCell : ItemCell
    {
        
    }

    class ItemCell 
    {
        public const int DefaultMaxAmount = 16;

        private readonly int _maxAmount;
        private int _amount;
        
        public Item SavedItem { get; private set; }
  
        public int Amount 
        {
            get { return _amount; }

            private set
            {
                if(SavedItem == null)
                {
                    _amount = 0;

                    return;
                }

                if (value > _maxAmount)
                {
                    _amount = _maxAmount;
                }
                else if(value <= 0)
                {
                    _amount = 0;
                    SavedItem = null;
                }
                else
                {
                    _amount = value;
                }
            }
        }

        public ItemCell(int maxAmount = DefaultMaxAmount)
        {
            _maxAmount = maxAmount;
        }

        public void SetItem(Item item,int amount)
        {
            SavedItem = item;
            Amount = amount;
        }

        public void Add() => Amount++;

        public void Remove(bool isFull = false)
        {
            if(isFull == true)
            {
                Amount = 0;
            }
            else
            {
                Amount--;
            }
        }

        public void MoveItemTo(ItemCell otherCell, int amount)
        {
            otherCell?.SetItem(SavedItem,amount);
            Amount = 0;
        }
    }

    class Item 
    {
        public string Name { get; private set; } = "Безымянный";

    }

}
