using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int ColumnSize = 3;
            const int RowSize = 3;
            const int MinRandomValue = 100;
            const int MaxRandomValue = 1000;
            int maxValue = 0;
            int[,] matrix = new int[ColumnSize, RowSize];

            Console.WriteLine("Исходная матрица");
            //Инициализация + вывод исходной
            for (int i = 0; i < ColumnSize; i++)
            {
                for (int k = 0; k < RowSize; k++)
                {
                    matrix[i, k] = new Random().Next(MinRandomValue, MaxRandomValue);

                    Console.Write(matrix[i, k] + " ");
                }
                Console.WriteLine();
            }
            //Поиск наибольшего
            for (int i = 0; i < ColumnSize; i++)
            {
                for (int k = 0; k < RowSize; k++)
                {
                    if (maxValue < matrix[i, k])
                    {
                        maxValue = matrix[i, k];
                    }
                }
            }
            //Замена нулями всех ячеек с maxValue
            for (int i = 0; i < ColumnSize; i++)
            {
                for (int k = 0; k < RowSize; k++)
                {
                    if (maxValue == matrix[i, k])
                    {
                        matrix[i, k] = 0;
                    }
                }
            }

            Console.WriteLine("\nРезультативная матрица");
            //Отдельно вывод
            for (int i = 0; i < ColumnSize; i++)
            {
                for (int k = 0; k < RowSize; k++)
                {
                    Console.Write(matrix[i,k]+" ");
                }

                Console.WriteLine();
            }
        }
    }
}
