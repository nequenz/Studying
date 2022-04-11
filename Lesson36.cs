using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ExitWord = "exit";
            const string AddWord = "add";
            const string RemoveWord = "remove";
            const string ShowAllWord = "show_all";
            string word = "";

            List<string[]> records = new List<string[]>();

            Console.WriteLine("Введите " + AddWord + " для добавления досье");
            Console.WriteLine("Введите " + RemoveWord + " для удаления досье");
            Console.WriteLine("Введите " + ShowAllWord + " для показа всех досье");
            Console.WriteLine("Введите " + ExitWord + " для выхода из меню\n\n");

            while (word != ExitWord)
            {
                Console.Write("Ввод:");
                word = Console.ReadLine();

                switch (word)
                {
                    case AddWord:
                        AddDossierByUserInput(records);
                        break;

                    case RemoveWord:
                        RemoveDossierByUserInput(records);
                        break;

                    case ShowAllWord:
                        WriteAllDossiers(records);
                        break;
                }
            }
        }

        private static void AddDossierByUserInput(List<string[]> list)
        {
            string fullnamePerson;
            string positionPerson;

            Console.Write("Введите ФИО:");
            fullnamePerson = Console.ReadLine();

            Console.Write("Введите Должность:");
            positionPerson = Console.ReadLine();

            list.Add(new string[] { fullnamePerson, positionPerson });

            Console.WriteLine("Запись добавлена.");
        }

        private static void RemoveDossierByUserInput(List<string[]> list)
        {
            Console.Write("Введите индекс для удаления записи:");

            int removeIndex = Convert.ToInt32(Console.ReadLine());

            if(removeIndex>=0 && removeIndex < list.Count)
            {
                list.RemoveAt(removeIndex);
                Console.WriteLine("\nУдаление прошло успешно!");

                return;
            }

            Console.WriteLine("\nОшибка! Запись не была удалена.");
        }

        private static void WriteAllDossiers(List<string[]> list)
        {
            const int FullnameIndex = 0;
            const int PositionIndex = 1;


            for(int i = 0; i < list.Count; i++)
            {
                string[] dossierElement = list[i];

                Console.WriteLine("[" + i + "] " + dossierElement[FullnameIndex] + " - " + dossierElement[PositionIndex]);
            }
        }
    }
}
