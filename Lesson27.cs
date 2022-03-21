using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int rowSize = 30;
            const int MinRandomBound = 10;
            const int MaxRandomBound = 99;
            int inputShift;

            int[] row = new int[rowSize];

            Console.WriteLine("Исходный массив:");

            for (int i = 0; i < rowSize; i++)
            {
                row[i] = new Random().Next(MinRandomBound, MaxRandomBound);

                Console.Write(row[i] + " ");
            }

            Console.Write("\nВведите число сдвига массива влево:");

            inputShift = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < inputShift; i++)
            {
                int savedValue = row[0];

                for (int k = 1; k < rowSize; k++)
                {
                    int currentElement = row[k];

                    row[k - 1] = currentElement;
                }

                row[rowSize-1] = savedValue;
            }

            Console.WriteLine("Сдвинутый массив:");

            for (int i = 0; i < rowSize; i++)
            {
                Console.Write(row[i] + " ");
            }
        }
    }
}
