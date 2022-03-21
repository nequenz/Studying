using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int ValueArraySize = 20;
            const int MinRandomBound = 0;
            const int MaxRandomBound = 100;
            int maxValue;
            int minValue;
            int avrValue;

            int[] valueArray = new int[ValueArraySize];
            int[] leftArray = new int[ValueArraySize];
            int[] rightArray = new int[ValueArraySize];

            int leftArrayLastIndex = 0;
            int rightArrayLastIndex = 0;
            int theMaxLength;

            Console.WriteLine("Исходный массив:\n");

            for (int i = 0; i < ValueArraySize; i++)
            {
                valueArray[i] = new Random().Next(MinRandomBound, MaxRandomBound);

                Console.WriteLine("[" + i + "] " + valueArray[i] + " ");
            }

            minValue = valueArray[0];
            maxValue = valueArray[0];

            for (int i = 1; i < ValueArraySize; i++)
            {
                int currentValue = valueArray[i];
                int prevValue = valueArray[i - 1];

                if (currentValue > maxValue)
                {
                    maxValue = currentValue;
                }

                if (currentValue < minValue)
                {
                    minValue = currentValue;
                }

                if (prevValue > currentValue)
                {
                    valueArray[i] = prevValue;
                    valueArray[i - 1] = currentValue;
                }
            }

            avrValue = (minValue + maxValue) / 2;

            for (int i = 0; i < ValueArraySize; i++)
            {
                int currentValue = valueArray[i];

                if(currentValue < avrValue)
                {
                    if (leftArrayLastIndex == 0)
                    {
                        leftArray[leftArrayLastIndex] = currentValue;
                    }
                    else
                    {
                        if (leftArray[leftArrayLastIndex - 1] > currentValue)
                        {
                            leftArray[leftArrayLastIndex] = leftArray[leftArrayLastIndex - 1];
                            leftArray[leftArrayLastIndex - 1] = currentValue;
                        }
                        else
                        {
                            leftArray[leftArrayLastIndex] = currentValue;
                        }
                    }

                    leftArrayLastIndex++;
                }
                else
                {
                    if(rightArrayLastIndex == 0)
                    {
                        rightArray[rightArrayLastIndex] = currentValue;
                    }
                    else
                    {
                        if (rightArray[rightArrayLastIndex - 1] > currentValue)
                        {
                            rightArray[rightArrayLastIndex] = rightArray[rightArrayLastIndex - 1];
                            rightArray[rightArrayLastIndex - 1] = currentValue;
                        }
                        else
                        {
                            rightArray[rightArrayLastIndex] = currentValue;
                        }
                    }
                    
                    rightArrayLastIndex++;
                }
            }

            theMaxLength = ((rightArrayLastIndex > leftArrayLastIndex) ? rightArrayLastIndex : leftArrayLastIndex) + 1;

            for (int i = 1; i < theMaxLength; i++)
            {
                for (int k = 1; k < theMaxLength; k++)
                {
                    int currentLeftValue = leftArray[k];
                    int currentRightValue = rightArray[k];

                    if (k<leftArrayLastIndex && currentLeftValue < leftArray[k - 1])
                    {
                        leftArray[k] = leftArray[k - 1];
                        leftArray[k - 1] = currentLeftValue;
                    }

                    if (k < rightArrayLastIndex && currentRightValue < rightArray[k - 1])
                    {
                        rightArray[k] = rightArray[k - 1];
                        rightArray[k - 1] = currentRightValue;
                    }
                }
            }

            Console.WriteLine("\nОтсортированный массив:\n");

            for (int i = 0; i < ValueArraySize; i++)
            {
                if (i < leftArrayLastIndex)
                {
                    valueArray[i] = leftArray[i];
                }
                else
                {
                    valueArray[i] = rightArray[i - leftArrayLastIndex];
                }

                Console.WriteLine("["+i+"] "+valueArray[i] + " ");
            }

            leftArray = null;
            rightArray = null;
        }
    }
}
