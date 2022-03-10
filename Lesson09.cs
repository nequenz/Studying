using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MaxAccumulatedValue = 100;
            const int AccumulatorValue = 7;

            for (int i = AccumulatorValue; i <= MaxAccumulatedValue; i+= AccumulatorValue)
            {
                Console.WriteLine("Вывод:" + i);
            }
               
            Console.WriteLine("Работа окончена");
        }
    }
}
