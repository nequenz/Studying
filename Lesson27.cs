using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int rowSize = 30;
            const int rowMiddleSize = rowSize / 3;
            const int MinRandomBound = 10;
            const int MaxRandomBound = 99;
            const int EmptyPlace = 0;
            int inputShift;

            int[] row = new int[rowSize];

            Console.WriteLine("Исходный массив:");

            for (int i = 0; i < rowSize; i++)
            {
                bool isInMiddle = (i > rowMiddleSize && i < (rowMiddleSize * 2));

                if (isInMiddle)
                {
                    row[i] = new Random().Next(MinRandomBound, MaxRandomBound);
                }
                else
                {
                    row[i] = EmptyPlace;
                }

                Console.Write(row[i] + " ");
            }

            Console.Write("\nВведите число сдвига массива влево:");

            inputShift = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < rowSize; i++)
            {
                bool isInRow = ((i - inputShift) >= 0);

                if (isInRow)
                {
                    row[i - inputShift] = row[i];

                }
                    row[i] = EmptyPlace;
            }

            Console.WriteLine("Сдвинутый массив:");

            for (int i = 0; i < rowSize; i++)
            {
                Console.Write(row[i] + " ");
            }
        }
    }
}
