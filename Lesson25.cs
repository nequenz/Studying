using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int rowSize = 20;
            const int MinRandomBound = 10;
            const int MaxRandomBound = 99;

            int[] row = new int[rowSize];

            Console.WriteLine("Исходный массив:");

            for (int i = 0; i < rowSize; i++)
            {
                row[i] = new Random().Next(MinRandomBound, MaxRandomBound);

                Console.Write(row[i] + " ");
            }

            Console.WriteLine();

            for (int i = 0; i < rowSize; i++)
            {
                int savedMinElement;
                int minElement = i;

                for (int k = i; k < rowSize; k++)
                {
                    int currentValue = row[k];

                    if (currentValue < row[minElement])
                    {
                        minElement = k;
                    }

                }

                savedMinElement = row[i];
                row[i] = row[minElement];
                row[minElement] = savedMinElement;
            }

            Console.WriteLine("\nОтсортированный массив:");

            for (int i = 0; i < rowSize; i++)
            {
                Console.Write(row[i] + " ");
            }
        }
    }
}
