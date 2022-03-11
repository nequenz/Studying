using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MinLoopBound = 100;
            const int MaxLoopBound = 1000;
            const int MinRandomBound = 0;
            const int MaxRandomBound = 29;
            int randomN = new Random().Next(MinRandomBound, MaxRandomBound);
            int multipleOfRandomN = 0;
            int SumOfMultiplesRandomN = 0;

            Console.WriteLine("Случайное кратное число:" + randomN);

            for (int i = MinLoopBound; i < MaxLoopBound; i++)
            {
                if (multipleOfRandomN < i)
                {
                    multipleOfRandomN += randomN;
                    i--;
                    
                    continue;
                }

                if(i == multipleOfRandomN)
                {
                    SumOfMultiplesRandomN += i;
                    multipleOfRandomN += randomN;
                }
            }

            Console.WriteLine("Сумма равна:" + SumOfMultiplesRandomN);
        }
    }
}
