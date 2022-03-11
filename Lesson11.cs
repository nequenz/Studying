using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MaxRandomBound = 100;
            const int MaxLoopRepeats = 50;
            const int MultiplesOf3 = 3;
            const int MultiplesOf5 = 5;
            
            int accumulatedNumber = 0;

            for(int i=0; i < MaxLoopRepeats; i++)
            {
                int currentRandomNumber = new Random().Next(0, MaxRandomBound);

                bool isMultiples = ((currentRandomNumber % MultiplesOf3) == 0 || (currentRandomNumber % MultiplesOf5) == 0);

                if (isMultiples == true)
                {
                    accumulatedNumber += currentRandomNumber;
                }
            }

            Console.WriteLine("Сумма равна:" + accumulatedNumber);
        }
    }
}
