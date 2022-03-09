using System;

namespace Tired
{
    class Program
    {

        private static int TryToConvert(string value)
        {

            int Result = 0;

            try
            {
                Result = Convert.ToInt32(value);
            }
            catch
            {

                Console.WriteLine("Вот зачем вы ввели неправильне значение?\nК сожалению вы без остались без золота и без кристалов!:(");

                //без throw

                return 0;

            }

            return Result;
        }

        static void Main(string[] args)
        {

            /*
             *  
             *  Unity Junior Developer - lesson 04
             * 
             */


            int crystalPrice = new Random().Next(15, 50);

            int crystalsToBuy = 0;


            int currentGoldCount = 0;

            int currentCrystalsCount = 0;



            Console.WriteLine("У данного торговца цена кристалла составляет:" + crystalPrice);

            Console.Write("Введите начальное количество золота:");

            currentGoldCount = TryToConvert( Console.ReadLine() );

            if (currentGoldCount == 0) return;

            Console.WriteLine("Принято\nВведите количество алмазов, которое вы хотите купить:");

            crystalsToBuy = TryToConvert( Console.ReadLine() );

            if (crystalsToBuy == 0) return;

            int dealPrice = crystalsToBuy * crystalPrice;

            Console.WriteLine("Вы отдатите " + dealPrice + " золота за " + crystalsToBuy + ", совершить сделку?\n(Нажмите что нибудь)");

            Console.ReadLine();

            currentGoldCount = currentGoldCount - dealPrice;

            currentCrystalsCount += crystalsToBuy;

            Console.WriteLine("Вы прибрели " + crystalsToBuy + " алмазов!\nТекущее золото:"+ currentGoldCount);

        }
    }
}
