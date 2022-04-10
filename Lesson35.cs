using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ExitWord = "exit";
            const string SumWord = "sum";
            string currentWord = "";
            List<int> numberList = new List<int>();

            Console.WriteLine("Введите число чтобы добавить его в список или " + ExitWord + " или " + SumWord + " для выполнения команд");

            while (currentWord != ExitWord)
            {
                Console.Write("Значение:");
                currentWord = Console.ReadLine();

                if( currentWord == ExitWord)
                {
                    continue;
                }
                else if(currentWord == SumWord)
                {
                    Console.WriteLine("Сумма всего списка равна:" + SumList(numberList));
                }
                else if(Int32.TryParse(currentWord, out int parsedValue) == true)
                {
                    numberList.Add(parsedValue);
                    Console.WriteLine("Значение добавлено!");
                }
                else
                {
                    Console.WriteLine("Не удалось добавить значение!");
                }
            }
        }

        private static int SumList(List<int> selectedList)
        {
            int result = 0;

            foreach(int number in selectedList)
            {
                result += number;
            }

            return result;
        }
    }
}
