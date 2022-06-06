using System;
using System.Collections.Generic;
using System.Linq;

namespace Tired
{
	public class Program
	{
		static void Main(string[] args)
        {
			int maxCount = 50;
			List < Person > people = new List < Person >();
			
			for(int i=0;i< maxCount; i++)
            {
				people.Add(StudyHelper.CreateRandomPerson());
            }

			Console.WriteLine("До амнистии");
			PrintPerson(people);

			Console.WriteLine("\nПосле амнистии\n");
			PrintPerson( Amnesty(people, "Антиправительственное") );
		}

		public static IEnumerable<Person> Amnesty(List<Person> list,string crimeName)
        {
			 return from Person prisoner in list where prisoner.CrimeName != crimeName select prisoner;
        }

		public static void PrintPerson(IEnumerable<Person> list)
        {
			foreach(Person prisoner in list)
            {
				prisoner.PrintInfo();
            }
        }
	}

	public class Person
    {
		public string Fullname { get; private set; } = "Безымянный";
		public string CrimeName { get; private set; } = "Мы узнаем...";

		public Person(string fullname, string crimeName)
        {
			Fullname = fullname;
			CrimeName = crimeName;
        }

		public void PrintInfo()
        {
			Console.WriteLine("Имя:"+Fullname+", Преступление:"+CrimeName);
        }
	}

	public static class StudyHelper
	{
		private static string[] _names = { "Olov", "Bulty", "Raulf", "Gehri", "Shon", "Loden", "Red", "Drama" };

		public static readonly string[] Crimes = {"Убийство","Насилие","Сепаратизм","Терроризм","Нацизм","Накакал на кровать", "Антиправительственное" };

		public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue, maxValue);

		public static Person CreateRandomPerson()
		{
			string fullname = _names[GetRandomValue(0, _names.Length)] + " " + _names[GetRandomValue(0, _names.Length)];
			string crime = Crimes[GetRandomValue(0, Crimes.Length)];

			return new Person(fullname, crime);
		}
	}
}
