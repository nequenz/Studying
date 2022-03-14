using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ExitWord = "exit";
            const string SumWord = "sum";
            string wordToRead = "";
            int readValueIndex = 0;
            int[] resizableArray = new int[1];

            while (wordToRead != ExitWord)
            {
                int currentArrayLength = resizableArray.GetLength(0);

                Console.Write("Введите число, которое хотите добавить в массив:");
                wordToRead = Console.ReadLine();

                if (wordToRead == SumWord)
                {
                    int sumAllElements = 0;

                    for(int i = 0; i < currentArrayLength; i++)
                    {
                        sumAllElements += resizableArray[i];
                    }

                    Console.WriteLine("\nСумма элементов:" + sumAllElements);

                }else if (wordToRead == ExitWord)
                {
                    continue;
                }
                else
                {
                    int readValue = Convert.ToInt32(wordToRead);

                    if (readValueIndex < currentArrayLength)
                    {
                        resizableArray[readValueIndex] = readValue;
                    }
                    else
                    {
                        int newArrayLenth = currentArrayLength + 1;

                        int[] tempArray = new int[newArrayLenth];

                        for (int i = 0; i < currentArrayLength; i++)
                        {
                            tempArray[i] = resizableArray[i];
                        }

                        resizableArray = tempArray;
                        resizableArray[readValueIndex] = readValue;
                    }

                    readValueIndex++;
                }
            }

            Console.WriteLine("Конец выполнения!");
        }
    }
}

