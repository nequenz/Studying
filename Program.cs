using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MaxAccumulatedValue = 100;
            const int AccumulatorValue = 7;
            int accumulatedValue = 0;

            for(int i=1;i<=(MaxAccumulatedValue/AccumulatorValue); i++)
            {
                accumulatedValue = AccumulatorValue * i;

                Console.WriteLine("Вывод:" + accumulatedValue);
            }

            Console.WriteLine("Работа окончена");
        }
    }
}
