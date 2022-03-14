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
            int[] matrix = new int[RowSize];

            Console.WriteLine("Исходная матрица");

            for (int i = 0; i < RowSize; i++)
            {
                matrix[i] = new Random().Next(MinRandomValue,MaxRandomValue);

                if(i % OutputRowDivNumber == 0)
                {
                    Console.WriteLine();
                }

                Console.Write(matrix[i]+",");
            }

            Console.WriteLine("\n");

            for (int i = 0; i < RowSize; i++)
            {
                int nextElement = i + 1;
                int prevElement = i - 1;
                int localMax = 0;

                if ( i == 0 )
                {
                    if (matrix[i] > matrix[nextElement])
                    {
                        Console.WriteLine("Локальный максимум крайнего элемента:"+ matrix[i]);
                    }
                }
                else if( i == RowSize - 1)
                {
                    if (matrix[i] > matrix[prevElement])
                    {
                        Console.WriteLine("Локальный максимум крайнего элемента:" + matrix[i]);
                    }
                }
                else
                {
                    if(matrix[prevElement] > matrix[i])
                    {
                        localMax = matrix[prevElement];
                    }
                    else
                    {
                        localMax = matrix[i];
                    }

                    if(localMax < matrix[nextElement])
                    {
                        localMax = matrix[nextElement];
                    }

                    Console.WriteLine("Локальный максимум:" + localMax);
                }
            }
        }
    }
}
