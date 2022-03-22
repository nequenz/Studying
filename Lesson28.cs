using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] personRecords = null;
            string[] personPositions = {"Капитан","Кассир","Менеджер","Разработчик","Художник","Грузчик","Логист"};

            AddDossier(personRecords, "");
        }

        private static void AddDossier(string[] personRecords,string record)
        {
            int newLength = personRecords.Length + 1;
            string[] newRecords = new string[newLength];

            for(int i = 0; i < personRecords.Length; i++)
            {
                newRecords[i] = personRecords[i];
            }

            newRecords[newLength - 1] = record;
        }

        private static void GetByName(string name)
        {

        }
    }
}
