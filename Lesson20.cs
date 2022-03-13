using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int ColumnSize = 3;
            const int RowSize = 3;
            int currentRowToSum = 1;
            int currentColumnToMul = 0;
            int rowSum = 0;
            int columnMul = 1;

            int[,] matrix = new int[ColumnSize,RowSize] { 
                {5, 9, 2}, 
                {1, 5, 4}, 
                {5, 2, 3} };

            for(int i = 0; i < RowSize; i++)
            {
                rowSum += matrix[currentRowToSum, i];
                columnMul *= matrix[i,currentColumnToMul];
            }

            for (int i = 0; i < ColumnSize; i++)
            {
                for (int k = 0; k < RowSize; k++)
                {
                    Console.Write(matrix[i,k]+" ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Сумма строки:" + rowSum + "\nПроизведение столбца:" + columnMul);
        }
    }
}
