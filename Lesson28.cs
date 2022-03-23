using System;

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
            const string SearchWord = "search";
            string word = "";

            string[] fullnamesRecords = null;
            string[] positionsRecords = null;

            Console.WriteLine("Введите "+ AddWord + " для добавления досье");
            Console.WriteLine("Введите "+ RemoveWord + " для удаления досье");
            Console.WriteLine("Введите "+ ShowAllWord + " для показа всех досье");
            Console.WriteLine("Введите "+ SearchWord + " для поиска досье по фамилии");
            Console.WriteLine("Введите "+ ExitWord + " для выхода из меню\n\n");

            while (word != ExitWord)
            {
                Console.Write("Ввод:");
                word = Console.ReadLine();

                switch (word)
                {
                    case AddWord:
                        string fullnamePerson;
                        string positionPerson;

                        Console.Write("Введите ФИО:");
                        fullnamePerson = Console.ReadLine();
                        Console.Write("Введите Должность:");
                        positionPerson = Console.ReadLine();

                        AddDossier(ref fullnamesRecords, ref positionsRecords, fullnamePerson, positionPerson);
                        Console.WriteLine("Запись добавлена.");
                        break;

                    case RemoveWord:
                        int indexToRemove;
                        bool isRemovingSuccess;

                        Console.Write("Введите индекс для удаления записи:");
                        indexToRemove = Convert.ToInt32( Console.ReadLine() );
                        isRemovingSuccess = RemoveDossierByIndex(ref fullnamesRecords, ref positionsRecords, indexToRemove);

                        if(isRemovingSuccess == true)
                        {
                            Console.WriteLine("\nУдаление прошло успешно!");
                        }
                        else
                        {
                            Console.WriteLine("\nОшибка! Запись не была удалена.");
                        }

                        break;

                    case ShowAllWord:
                        WriteAllDossiers(ref fullnamesRecords, ref positionsRecords);
                        break;

                    case SearchWord:
                        string surname;

                        Console.Write("Введите фамилию для поиска досье:");
                        surname = Console.ReadLine();

                        SearchDossierIndexBySurname(ref fullnamesRecords, ref positionsRecords,surname);
                        break;
                }
            }


        }

        private static void SearchDossierIndexBySurname(ref string[] fullnamesRecords, ref string[] positionRecords, string surname)
        {
            if (IsDossiersCorrect(ref fullnamesRecords, ref positionRecords, 0) == false)
            {
                Console.WriteLine("В базе досье присутствуют ошибки! Не могу найти что требуется.");

                return;
            }

            for (int i = 0; i < fullnamesRecords.Length; i++)
            {
                if( fullnamesRecords[i].Contains(surname) )
                {
                    WriteDossierByIndex(ref fullnamesRecords,ref positionRecords,i);
                }
            }
        }

        private static void WriteAllDossiers(ref string[] fullnamesRecords, ref string[] positionRecords)
        {
            if (IsDossiersCorrect(ref fullnamesRecords, ref positionRecords, 0) == false)
            {
                Console.WriteLine("В базе досье присутствуют ошибки! Не могу ничего вывести.");

                return;
            }

            for (int i = 0; i < fullnamesRecords.Length; i++)
            {
                WriteDossierByIndex(ref fullnamesRecords, ref positionRecords,i);
            }
        }

        private static void WriteDossierByIndex(ref string[] fullnamesRecords, ref string[] positionRecords, int index)
        {
            if(IsDossiersCorrect(ref fullnamesRecords,ref positionRecords, index) == false)
            {
                return;
            }

            Console.WriteLine("[" + index + "] " + fullnamesRecords[index] + " - " + positionRecords[index]);
        }


        private static void AddDossier(ref string[] fullnamesRecords, ref string[] positionRecords, string fullnameRecord, string positionRecord)
        {
            AddValueToArray(ref fullnamesRecords,fullnameRecord);
            AddValueToArray(ref positionRecords,positionRecord);
        }

        private static bool RemoveDossierByIndex(ref string[] fullnamesRecords,ref string[] positionRecords,int index)
        {
            return ( DeleteValueFromArrayByIndex(ref fullnamesRecords, index) && DeleteValueFromArrayByIndex(ref positionRecords, index) );
        }

        private static bool IsDossiersCorrect(ref string[] fullnamesRecords, ref string[] positionRecords,int index)
        {
            if (fullnamesRecords != null && positionRecords != null)
            {
                if(fullnamesRecords.Length == positionRecords.Length)
                {
                    if(index>=0 && index < fullnamesRecords.Length)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool DeleteValueFromArrayByIndex<T>(ref T[] records, int recordIndex)
        {
            T[] newArray;
            int iterator = 0;

            if( records == null || (recordIndex >= 0 && recordIndex < records.Length) == false )
            {
                return false;
            }

            newArray = new T[records.Length - 1];

            for(int i = 0; i < records.Length; i++)
            {
                if(recordIndex == i)
                {
                    continue;
                }

                newArray[iterator] = records[i];

                iterator++;
            }

            records = newArray;

            return true;
        }

        private static void AddValueToArray<T>(ref T[] currentArray, T record)
        {
            ExtendArray(ref currentArray);

            currentArray[currentArray.Length - 1] = record;
        }

        private static void ExtendArray<T>(ref T[] currentArray)
        {
            T[] newArray;

            if (currentArray == null)
            {
                currentArray = new T[1];

                return;
            }

            newArray = new T[currentArray.Length + 1];

            for (int i = 0; i < currentArray.Length; i++)
            {
                newArray[i] = currentArray[i];
            }

            currentArray = newArray;
        }
    }
}
