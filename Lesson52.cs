using System;
using System.Collections.Generic;
using System.Linq;

namespace Tired
{
	public class Program
	{
		static void Main(string[] args)
        {
			int maxCount = 200;
			List < Prisoner > prisoners = new List < Prisoner >();
			
			for(int i=0;i< maxCount; i++)
            {
				prisoners.Add(StudyHelper.CreateRandomPrisoner());
            }

			PrintPrisonerList(prisoners);

			SendRequest(prisoners);
		}

		public static void SendRequest(List<Prisoner> list)
        {
			string nation;

			Console.WriteLine("Введите данные(рост, вес и нацию) для поиска...");

			for(int i = 0; i < StudyHelper.Nations.Length; i++)
            {
				Console.WriteLine(StudyHelper.Nations[i]);
            }

			Console.Write("Введите рост:");

			if (int.TryParse(Console.ReadLine(), out int height) == true)
			{
				Console.Write("Введите вес:");

				if (int.TryParse(Console.ReadLine(), out int weight) == true)
				{
					nation = Console.ReadLine();

					DetectiveRequest(list,height,weight,nation);

					return;
				}
			}

			Console.WriteLine("Данные не обработаны...");
        }

		public static void DetectiveRequest(List<Prisoner> list,int height,int weight,string nation)
        {
			var prisoners = from Prisoner prisoner in list
							where prisoner.Height == height
							&& prisoner.Weight == weight
							&& prisoner.Nationality == nation
							&& prisoner.IsInPrison == false
							select prisoner;

			Console.WriteLine("Кандидаты:");

			foreach(Prisoner prisoner1 in prisoners)
            {
				prisoner1.PrintInfo();
            }
        }

		public static void PrintPrisonerList(List<Prisoner>  list)
        {
			foreach(Prisoner prisoner in list)
            {
				prisoner.PrintInfo();
            }

			Console.WriteLine("-------------------------------------------");
        }
	}

	public class Prisoner
    {
		public string Fullname { get; private set; } = "Безымянный";
		public int Height { get; private set; } = 150;
		public int Weight { get; private set; } = 70;
		public bool IsInPrison { get; private set; } = false;
		public string Nationality { get; private set; } = "Без нации";

		public Prisoner(string fullname, int height,int weight, bool isInPrison,string nation)
        {
			Fullname = fullname;
			Height = height;
			Weight = weight;
			IsInPrison = isInPrison;
			Nationality = nation;
        }

		public void PrintInfo()
        {
			Console.WriteLine("Имя:"+Fullname+", Рост:"+Height+", Вес:"+Weight+", Национальность:"+Nationality.ToString());
        }
	}

	public static class StudyHelper
	{
		private static string[] _names = { "Olov", "Bulty", "Raulf", "Gehri", "Shon", "Loden", "Red", "Drama" };

		public static readonly string[] Nations = { "Русский", "Американец", "Сомалиец", "Австриец", "Британец", "Голладенц" };
		
		public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue, maxValue);

		public static Prisoner CreateRandomPrisoner()
		{
			const int maxAge = 90;
			const int maxHeight = 200;
			const int boolBound = 2;

			string fullname = _names[GetRandomValue(0, _names.Length)] + " " + _names[GetRandomValue(0, _names.Length)];
			int height = GetRandomValue(0, maxAge);
			int Weight = GetRandomValue(0, maxHeight);
			bool isInPrison = Convert.ToBoolean(GetRandomValue(0, boolBound));
			string nation = Nations[GetRandomValue(0, Nations.Length)];

			return new Prisoner(fullname,height, Weight, isInPrison,nation);
		}
	}
}
 
 
