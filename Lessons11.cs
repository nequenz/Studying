using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MaxRandomBound = 100;
            const int MaxLoopRepeats = 50;
            int accumulatedNumber = 0;

            for(int i=0; i < MaxLoopRepeats; i++)
            {
                int currentRandomNumber = new Random().Next(0, MaxRandomBound);

                bool isMultiples = ((currentRandomNumber % 3) == 0 || (currentRandomNumber % 5) == 0);

                if (isMultiples == true) accumulatedNumber += currentRandomNumber;
            }

            Console.WriteLine("Сумма кратных 3 или 5 рандомов равна:" + accumulatedNumber);
        }
    }
}
