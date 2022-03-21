using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int RowSize = 20;
            const int MinRandomBound = 10;
            const int MaxRandomBound = 99;

            int[] row = new int[RowSize];

            Console.WriteLine("Исходный массив:");

            for (int i = 0; i < RowSize; i++)
            {
                row[i] = new Random().Next(MinRandomBound, MaxRandomBound);

                Console.Write(row[i] + " ");
            }

            Console.WriteLine();

            for (int i = 0; i < RowSize; i++)
            {
                int savedMinElement;
                int minElement = i;

                for (int k = i; k < RowSize; k++)
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

            for (int i = 0; i < RowSize; i++)
            {
                Console.Write(row[i] + " ");
            }
        }
    }
}
