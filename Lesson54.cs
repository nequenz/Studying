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
		private const string WordToSortByKey = "фильтровать";
		private const string WordToSortAscending = "сортировать по возрастанию";
		private const string WordToSortDescending = "сортировать по убыванию";

		private List<SickPerson> _sickPeople = new List<SickPerson>();

		public UserMenu()
        {
			CreateRandomSickPeople();

			PrintPeople(_sickPeople);
			PrintHelp();
		}

		public void Update()
        {
			string word="";

            while (word != WordToExit)
            {
				Console.Write("Ввод:");

				word = Console.ReadLine();

				if(word == WordToExit)
                {
					continue;
                }
				else if(word == WordToSortByKey)
                {
					FilterPeopleListByKey();
				}
				else if(word == WordToSortAscending)
                {
					SortAscending();
                }
				else if (word == WordToSortDescending)
                {
					SortDescending();
				}
			}
        }

		private void SortAscending()
        {
			var sortedListByFullname = _sickPeople.OrderBy(person => person.Fullname);
			var sortedListByAge = _sickPeople.OrderBy(person => person.Age);

			PrintPeople(SortByCustom(sortedListByFullname, sortedListByAge));
		}

		private void SortDescending()
		{
			var sortedListByFullname = _sickPeople.OrderByDescending(person => person.Fullname);
			var sortedListByAge = _sickPeople.OrderByDescending(person => person.Age);

			PrintPeople(SortByCustom(sortedListByFullname, sortedListByAge));
		}

		private IEnumerable<SickPerson> SortByCustom(IEnumerable<SickPerson> requestToSortByFullname, IEnumerable<SickPerson> requestToSortByAge)
        {
			const string FullNameField = "ФИО";
			const string AgeField = "Возраст";

			IEnumerable<SickPerson> sortedList = null;
			string word;

			Console.Write("Введите поле для сортировки("+ FullNameField + ", "+ AgeField + "):");

			word = Console.ReadLine();

            switch (word)
            {
				case FullNameField:
					sortedList = requestToSortByFullname;
					break;

				case AgeField:
					sortedList = requestToSortByAge;
					break;
            }

			return sortedList;
		}

		private void FilterPeopleListByKey()
		{
			char key;

			Console.Write("Введите букву для фильтрации больных по названию болезни:");

			key = Console.ReadLine()[0];

			var filteredList = from SickPerson person in _sickPeople
							   where person.IllnesName.ToUpper().StartsWith(char.ToUpper(key))
							   select person;

			PrintPeople(filteredList);
		}

		private void CreateRandomSickPeople()
		{
			const int maxCount = 100;

			for (int i = 0; i < maxCount; i++)
			{
				_sickPeople.Add(StudyHelper.CreatePersonWithRandomIllnes());
			}
		}

		private void PrintPeople(IEnumerable<SickPerson> persons)
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
			Console.WriteLine("Введите '" + WordToSortByKey + "', чтобы фильтровать больных по названию болезни.");
			Console.WriteLine("Введите '" + WordToSortAscending + "', чтобы сортировать больных по возрастанию.");
			Console.WriteLine("Введите '" + WordToSortDescending + "', чтобы сортировать больных по возрастанию.");
		}
	}

	public class SickPerson
    {
		public string Fullname { get; private set; } = "Безымянный";
		public string IllnesName { get; private set; } = "Неидентифицированная болезнь";
		public int Age { get; private set; }

		public SickPerson(string fullname, string illnesName,int age)
        {
			Fullname = fullname;
			IllnesName = illnesName;
			Age = age;
        }


		public void PrintInfo()
        {
			Console.WriteLine("Имя:"+Fullname+", Возраст:"+Age+", Болезнь:"+IllnesName);
        }
	}

	public static class StudyHelper
	{
		private static string[] _names = { "Olov", "Bulty", "Raulf", "Gehri", "Shon", "Loden", "Red", "Drama" };

		public static readonly string[] Illnesses = {"Акне","Зуд","Рак","Амнезия","Бронхит","Корь", "Трахома" };

		public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue, maxValue);

		public static SickPerson CreatePersonWithRandomIllnes()
		{
			const int MaxAge = 120;

			string fullname = _names[GetRandomValue(0, _names.Length)] + " " + _names[GetRandomValue(0, _names.Length)];
			string illnesses = Illnesses[GetRandomValue(0, Illnesses.Length)];
			int age = GetRandomValue(1,MaxAge);


			return new SickPerson(fullname, illnesses, age);
		}
	}
}
