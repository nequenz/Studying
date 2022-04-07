using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int NumberArraySize = 25;
            int[] numberArray = new int[NumberArraySize];

            InitializateNumberArray(numberArray);

            Console.WriteLine("Исходный массив:");
            WriteNumberArray(numberArray);

            Console.WriteLine("Результативный массив:");
            ShuffleNumberArray(numberArray);
            WriteNumberArray(numberArray);
        }

        private static void InitializateNumberArray(int[] array)
        {
            int maxRandomBound = 100;

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new Random().Next(0, maxRandomBound);
            }
        }

        private static void WriteNumberArray(int[] array)
        {
            foreach(int number in array)
            {
                Console.Write(number+"  ");
            }

            Console.WriteLine();
        }

        private static void ShuffleNumberArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int randomIndex = new Random().Next(0, array.Length);
                int savedValue = array[randomIndex];

                array[randomIndex] = array[i];
                array[i] = savedValue;
            }
        }
    }
}
