using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int OutputRowDivNumber = 10;
            const int ValueArraySize = 30;
            const int MinRandomBound = 0;
            const int MaxRandomBound = 6;

            int maxSameValue;
            int maxSameValueRepeats = 1;
            int savedValue;
            int savedValueRepeatCount = 1;

            int[] valueArray = new int[ValueArraySize];

            Console.WriteLine("Исходный массив:");

            for (int i = 0; i < ValueArraySize; i++)
            {
                valueArray[i] = new Random().Next(MinRandomBound, MaxRandomBound);

                if (i % OutputRowDivNumber == 0)
                {
                    Console.WriteLine();
                }

                Console.Write(valueArray[i] + " ");
            }

            maxSameValue = valueArray[0];

            for (int i = 1; i < ValueArraySize; i++)
            {
                int prevElement = valueArray[i - 1];
                int currentElement = valueArray[i];

                if (currentElement == prevElement)
                {
                    savedValue = currentElement;
                    savedValueRepeatCount++;

                    bool isSavedRepeatCountMoreMaxRepeats = (savedValueRepeatCount > maxSameValueRepeats);

                    if (isSavedRepeatCountMoreMaxRepeats)
                    {
                        maxSameValue = savedValue;
                        maxSameValueRepeats = savedValueRepeatCount;
                    }
                }
                else
                {
                    savedValueRepeatCount = 1;
                }
            }

            Console.Write("\n\nЧисло " + maxSameValue + " повторилось " + maxSameValueRepeats + " раз и является наибольшим локальный массивом\nРезультативный массив:");
        }
    }
}

