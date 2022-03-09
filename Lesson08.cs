using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ExitWord = "exit";
            string inputWord = "";

            while(inputWord != ExitWord)
            {
                Console.WriteLine("Введите слово для выхода из цикла:");
                inputWord = Console.ReadLine();
            }

            Console.WriteLine("Вы покинули цикл!");
        }
    }
}
