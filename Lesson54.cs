using System;
using System.Collections.Generic;
using System.Linq;

namespace Tired
{
    public class Program
    {

        static void Main(string[] args)
        {
            UserMenu userMenu = new UserMenu();

            userMenu.Update();
        }
    }

    public class UserMenu
    {
        private const string WordToExit = "выход";
        private const string WordToSortByIllness = "фильтровать";
        private const string WordToSortAscending = "сортировать по возрастанию";
        private const string WordToSortDescending = "сортировать по убыванию";
        private const string WordToClearSort = "очистить фильтр";

        private List<SickPerson> _sickPeople = new List<SickPerson>();
        private IEnumerable<SickPerson> _sortedList = null;

        public UserMenu()
        {
            CreateRandomSickPeople();

            _sortedList = _sickPeople.ToArray();
        }

        public void Update()
        {
            string word = "";

            while (word != WordToExit)
            {
                PrintList(_sickPeople);
                Console.WriteLine("-------------Отфильтрованный список-------------");
                PrintList(_sortedList);

                PrintHelp();
                Console.Write("Ввод:");

                word = Console.ReadLine();

                switch (word)
                {
                    case WordToExit:
                        continue;
                        break;

                    case WordToSortByIllness:
                        FilterPeopleListByIllnes();
                        break;

                    case WordToSortAscending:
                        SortAscending();
                        break;

                    case WordToSortDescending:
                        SortDescending();
                        break;

                    case WordToClearSort:
                        _sortedList = _sickPeople.ToArray();
                        break;
                }

                Console.Clear();
            }
        }

        private void SortAscending()
        {
            var sortedListByFullname = _sickPeople.OrderBy(person => person.Fullname);
            var sortedListByAge = _sickPeople.OrderBy(person => person.Age);

            SortByCustom(sortedListByFullname, sortedListByAge);
        }

        private void SortDescending()
        {
            var sortedListByFullname = _sortedList.OrderByDescending(person => person.Fullname);
            var sortedListByAge = _sortedList.OrderByDescending(person => person.Age);

            SortByCustom(sortedListByFullname, sortedListByAge);
        }

        private void SortByCustom(IEnumerable<SickPerson> requestToSortByFullname, IEnumerable<SickPerson> requestToSortByAge)
        {
            const string FullNameField = "ФИО";
            const string AgeField = "Возраст";

            string word;

            Console.Write("Введите поле для сортировки(" + FullNameField + ", " + AgeField + "):");

            word = Console.ReadLine();

            switch (word)
            {
                case FullNameField:
                    _sortedList = requestToSortByFullname;
                    break;

                case AgeField:
                    _sortedList = requestToSortByAge;
                    break;
            }
        }

        private void FilterPeopleListByIllnes()
        {
            string illness;

            Console.Write("Наименование болезни:");

            illness = Console.ReadLine();

            _sortedList = from SickPerson person in _sickPeople
                          where person.IllnesName.ToUpper().StartsWith(illness.ToUpper())
                          select person;
        }

        private void CreateRandomSickPeople()
        {
            const int maxCount = 50;

            for (int i = 0; i < maxCount; i++)
            {
                _sickPeople.Add(StudyHelper.CreatePersonWithRandomIllnes());
            }
        }

        private void PrintList(IEnumerable<SickPerson> persons)
        {
            Console.WriteLine("---------------------------------------------------");

            foreach (SickPerson prisoner in persons)
            {
                prisoner.PrintInfo();
            }

            Console.WriteLine("---------------------------------------------------");
        }

        private void PrintHelp()
        {
            Console.WriteLine("\nВведите '" + WordToExit + "', чтобы выйти из программы.");
            Console.WriteLine("Введите '" + WordToSortByIllness + "', чтобы фильтровать больных по названию болезни.");
            Console.WriteLine("Введите '" + WordToSortAscending + "', чтобы сортировать больных по возрастанию.");
            Console.WriteLine("Введите '" + WordToSortDescending + "', чтобы сортировать больных по возрастанию.");
            Console.WriteLine("Введите '" + WordToClearSort + "', чтобы очистить фильтр и сортировку.");
        }
    }

    public class SickPerson
    {
        public string Fullname { get; private set; } = "Безымянный";
        public string IllnesName { get; private set; } = "Неидентифицированная болезнь";
        public int Age { get; private set; }

        public SickPerson(string fullname, string illnesName, int age)
        {
            Fullname = fullname;
            IllnesName = illnesName;
            Age = age;
        }


        public void PrintInfo()
        {
            Console.WriteLine("Имя:" + Fullname + ", Возраст:" + Age + ", Болезнь:" + IllnesName);
        }
    }

    public static class StudyHelper
    {
        private static string[] _names = { "Olov", "Bulty", "Raulf", "Gehri", "Shon", "Loden", "Red", "Drama" };

        public static readonly string[] Illnesses = { "Акне", "Зуд", "Рак", "Амнезия", "Бронхит", "Корь", "Трахома" };

        public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue, maxValue);

        public static SickPerson CreatePersonWithRandomIllnes()
        {
            const int MaxAge = 120;

            string fullname = _names[GetRandomValue(0, _names.Length)] + " " + _names[GetRandomValue(0, _names.Length)];
            string illnesses = Illnesses[GetRandomValue(0, Illnesses.Length)];
            int age = GetRandomValue(1, MaxAge);


            return new SickPerson(fullname, illnesses, age);
        }
    }
}
