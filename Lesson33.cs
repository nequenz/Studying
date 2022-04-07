using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, float> itemPrices = new Dictionary<string, float>();

            AddStandartItems(itemPrices);
            Console.WriteLine("Введите название предмета, чтобы вывести его цену");
            WritePriceByName(itemPrices,Console.ReadLine());
        }

        private static void AddStandartItems(Dictionary<string,float> dictionary)
        {
            dictionary.Add("Book",1200.50f);
            dictionary.Add("Cookie", 75.0f);
            dictionary.Add("Cake", 378.25f);
            dictionary.Add("Car",135000.0f);
            dictionary.Add("Beer",95.66f);
            dictionary.Add("Girl",999999.0f);
            dictionary.Add("Bottle",95.95f);
        }

        private static void WritePriceByName(Dictionary<string,float> dictionary, string itemName)
        {
            bool IsContained = dictionary.TryGetValue(itemName, out float price);

            if (IsContained == true)
            {
                Console.WriteLine("Цена предмета "+itemName+" равна "+price+" Р");
            }
            else
            {
                Console.WriteLine("Такого предмета нет в наборе..");
            }
        }
    }
}
