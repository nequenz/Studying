using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int OutputRowDivNumber = 10;
            const int RowSize = 30;
            const int MinRandomValue = 10;
            const int MaxRandomValue = 100;
            const int RowTopElementIndex = RowSize - 1;
            int[] rowArray = new int[RowSize];

            Console.WriteLine("Исходная матрица");

            for (int i = 0; i < RowSize; i++)
            {
                rowArray[i] = new Random().Next(MinRandomValue, MaxRandomValue);

                if (i % OutputRowDivNumber == 0)
                {
                    Console.WriteLine();
                }

                Console.Write(rowArray[i] + ",");
            }

            Console.WriteLine("\n");

            if (rowArray[0] > rowArray[1])
            {
                Console.WriteLine("Локальный максимум крайнего элемента:" + rowArray[0]);
            }

            for (int i = 1; i < RowTopElementIndex; i++)
            {
                int nextElement = i + 1;
                int prevElement = i - 1;
                int localMax = 0;

                if (rowArray[prevElement] > rowArray[i])
                {
                    localMax = rowArray[prevElement];
                }
                else
                {
                    localMax = rowArray[i];
                }

                if (localMax < rowArray[nextElement])
                {
                    localMax = rowArray[nextElement];
                }

                Console.WriteLine("Локальный максимум:" + localMax);
            }

            if (rowArray[RowTopElementIndex] > rowArray[RowTopElementIndex - 1])
            {
                Console.WriteLine("Локальный максимум крайнего элемента:" + rowArray[RowTopElementIndex]);
            }
        }
    }
}
