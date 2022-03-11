using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MinRandomBound = 2;
            const int MaxRandomBound = 100;
            const int MaxLoopValue = 100;

            int currentValue = new Random().Next(MinRandomBound, MaxRandomBound);
            int minValueExp = 2;

            for (int i=1; i< MaxLoopValue; i++)
            {
                bool isCurrentValueLessMinExpOf2 = (currentValue <= minValueExp);

                if (isCurrentValueLessMinExpOf2 == true)
                {
                    Console.WriteLine("Случайное число "+ currentValue + "\nМинимальная степень составляет: "+i+"\nРезультативное значение:"+ minValueExp);
                    
                    break;
                }

                minExpOf2 *= 2;
            }
        }
    }
}
