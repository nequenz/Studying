using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            int crystalPrice = new Random().Next(15, 50);

            int crystalsToBuy = 0;

            int currentGoldCount = 0;

            int currentCrystalsCount = 0;

            Console.WriteLine("У данного торговца цена кристалла составляет:" + crystalPrice);

            Console.Write("Введите начальное количество золота:");

            currentGoldCount = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Принято\nВведите количество алмазов, которое вы хотите купить:");

            crystalsToBuy = Convert.ToInt32(Console.ReadLine());

            int dealPrice = crystalsToBuy * crystalPrice;

            Console.WriteLine("Вы отдатите " + dealPrice + " золота за " + crystalsToBuy + ", совершить сделку?\n(Нажмите что нибудь)");

            Console.ReadLine();

            currentGoldCount = currentGoldCount - dealPrice;

            currentCrystalsCount += crystalsToBuy;

            Console.WriteLine("Вы прибрели " + crystalsToBuy + " алмазов!\nТекущее золото:" + currentGoldCount);
        }
    }
}
