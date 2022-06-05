using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tired
{
	//public delegate void IUpdatableHandler(IUpdatable updatable);

	public class Program
	{
		static void Main(string[] args)
        {
			CarService carService = new CarService();

			carService.Update();
        }
	}

	public class CarService
    {
		private const string WordToExit = "выход";
		private const string WordToRepair = "починить";
		private const string WordToRefuse = "отказать";
		private const int DetailsNotEnoughFine = 3000;

		private Account _account = new Account();
		private List<DetailID> _detailsInStock = new List<DetailID>();

		public Car TakenCar { get; private set; } = null;

		public CarService()
        {
			AddRandomDetailSet();

			_account.Put(25000);
		}

		public void Update()
        {
			string word = "";

            while (word != WordToExit)
            {
				PrintInfo();
				PrintMyDetails();
				UpdateRandomCar();

				word = Console.ReadLine();

				if(word == WordToExit)
                {
					continue;
                }
				else if (word == WordToRepair)
                {
					RepairByUserInput();
				}
				else if(word == WordToRefuse)
                {
					_account.TryRemove(DetailsNotEnoughFine);
					Console.WriteLine("Вы отказали клиенту!");
                }

				Console.Clear();
            }

        }

		private void RepairByUserInput()
        {
			Console.Write("Введите название детали:");

			RepairDetailOfTakenCar( Console.ReadLine() );
        }

		private void AddRandomDetailSet()
		{
			const int MinCount = 10;
			const int MaxCount = 25;
			DetailID detailID;

			for (int i = 0; i < DetailDataBase.GetDetailCount(); i++)
			{
				detailID = DetailID.New(i);
				detailID.SetAmount(Helper.GetRandomValue(MinCount, MaxCount));

				_detailsInStock.Add(detailID);
			}

			Console.WriteLine("Вам добавлен случайный набор деталей");
		}

		private void PrintMyDetails()
        {
			Console.WriteLine("\nМои детали:");

			foreach(DetailID id in _detailsInStock)
            {
				Console.WriteLine("Деталь '"+id.MatchedDetail.Name+"', кол-во:"+id.Amount);
            }

			Console.WriteLine();
        }

		private void PrintInfo()
        {
			Console.WriteLine("Введите " + WordToExit + ", чтобы выйти.");
			Console.WriteLine("Введите " + WordToRepair + ", чтобы починить деталь.");
			Console.WriteLine("Введите " + WordToRefuse + ", чтобы отказать в починке.");
			Console.WriteLine("На счету автосервиса:"+_account.CurrentMoney+" P");
		}

		private void UpdateRandomCar()
        {
			if(TakenCar == null || TakenCar.IsRepained() == true)
            {
				if(TakenCar != null && TakenCar.IsRepained() == true)
					Console.WriteLine("Клиент доволен работой.");

				Console.WriteLine("К вам прибыл новый клиент.");

				TakenCar = new Car();
			}

			Console.WriteLine("Обслужите машину под номером:" + TakenCar.Number);
			TakenCar.PrintDetails();
		}

		private void RepairDetailOfTakenCar(string detailName)
        {
			int detailsInStockIndex = GetIndexOfDetailByName(detailName, _detailsInStock);
			int carDetailIndex = GetIndexOfDetailByName(detailName, TakenCar.GetDetails());

            if (carDetailIndex == -1)
            {
				WriteSleepingLine("Ошибка ввода! Такой детали нет.");

				return;
            }

			if (detailsInStockIndex != -1)
            {
				DetailID carDetailID = TakenCar.GetDetails()[carDetailIndex];

				if(carDetailID.IsBroken == true)
                {
					_account.Put(carDetailID.MatchedDetail.FullPrice);

					carDetailID.SetBroken(false);

					TakenCar.GetDetails()[carDetailIndex] = carDetailID;

					WriteSleepingLine("Вы починили деталь");
                }
                else
                {
					_account.TryRemove(carDetailID.MatchedDetail.FinePrice);

					WriteSleepingLine("Вы починили не ту деталь!");
				}
            }
            else
            {
				WriteSleepingLine("У вас нет деталей на складе..");

				_account.TryRemove(DetailsNotEnoughFine);
			}
		}

		private int GetIndexOfDetailByName(string detailName, List<DetailID> list)
        {
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].MatchedDetail.Name == detailName)
				{
					

					return i;
				}
			}

			return -1;
		}

		public void WriteSleepingLine(string text)
		{
			const int time = 4000;
			Console.WriteLine(text);
			System.Threading.Thread.Sleep(time);
		}
	}

	public class Account
    {
		public int CurrentMoney { get; private set; } = 0;

		public void Put(int money) => CurrentMoney += money;

		public bool TryRemove(int money)
        {
			if (CurrentMoney <= 0)
            {
				return false;
            }
            else
            {
				CurrentMoney -= money;

				return true;
            }
		}

    }

	public class Car
	{
		private List<DetailID> _details = DetailDataBase.GetAllDetailIDs();

		public int Number { get; private set; } 

		public Car()
        {
			SetBrokenDetailsByRandom();

			Number = GetHashCode();
		}

		private void SetBrokenDetailsByRandom()
        {
			const int MaxRandomBound = 2;

			for(int i = 0; i < _details.Count; i++)
			{
				if (Helper.GetRandomValue(0, MaxRandomBound) == 0)
				{
					DetailID id = _details[i];
					id.SetBroken(true);
					_details[i] = id;
				}
			}
        }

		public void PrintDetails()
        {
			Console.WriteLine("Детали машины под номером ("+Number+")");

			for(int i = 0; i < _details.Count; i++)
            {
				//_details[i].
				_details[i].PrintShortDetails();
            }
        }

		public List<DetailID> GetDetails() => _details;

		public bool IsRepained()
        {
			foreach(DetailID detailID in _details)
            {
				if (detailID.IsBroken == true) return false;
            }

			return true;
        }
	}

	public sealed class Detail
    {
		public string Name { get; private set; } = "Безымянная деталь";
		public int Price { get; private set; } = 0;
		public int FinePrice { get; private set; } = 0;
		public int WorkPrice { get; private set; } = 0;
		public int FullPrice
        {
            get
            {
				return (WorkPrice + Price);
            }
        }
		

		public Detail(string name, int price, int workPrice, float fineFactor)
        {
			Name = name;
			Price = price;
			WorkPrice = workPrice;
			FinePrice = (int)(Price * fineFactor);
        }
    }

	public struct DetailID : IEquatable<DetailID>
	{
		private int _id;
		private bool _isBroken;
		public int Id
        {
			get => _id;
		}
		public bool IsBroken
        {
			get => _isBroken;
        }
		public int Amount { get; private set; }
		public Detail MatchedDetail
		{
			get
			{
				return DetailDataBase.GetDetailInfoByID(this);
			}
		}

		public static DetailID New(int id)
		{
			DetailID detailID = new DetailID();
			detailID.SetID(id);

			return detailID;
		}

		public void SetID(int id) => _id = id;

		public void SetAmount(int amount) => Amount = amount;

		public void SetBroken(bool isBroken) => _isBroken = isBroken;

		public void PrintShortDetails()
		{
			string status = (IsBroken == false) ? "в хорошем состоянии" : "повреждена";

			Console.WriteLine("Деталь '" + DetailDataBase.GetDetailInfoByID(this).Name + "' " + status);
		}

		public void PrintAllDetails()
		{
			Detail detail = MatchedDetail;

			Console.WriteLine("\nНазвание детали:" + detail.Name);
			Console.WriteLine("Цена детали:" + detail.Price);
			Console.WriteLine("Цена работы:" + detail.WorkPrice);
			Console.WriteLine("Цена полной замены:" + detail.FullPrice);
			Console.WriteLine("Цена в случае неудачной замены:" + detail.FinePrice);
		}

		public bool Equals(DetailID other) => Id == other.Id;
	}

	public static class DetailDataBase
    {
		private static Dictionary<DetailID, Detail> _list = new Dictionary<DetailID, Detail>();

		public static readonly DetailID GlassDetail = CreateNewDetail(new Detail("Стекло",4000,2000,1.25f));
		public static readonly DetailID WheelDetail = CreateNewDetail(new Detail("Колесо", 3200, 1700, 1.10f));
		public static readonly DetailID DoorDetail = CreateNewDetail(new Detail("Дверь", 7000, 6600, 1.75f));
		public static readonly DetailID SitDetail = CreateNewDetail(new Detail("Сиденье", 12100, 2300, 1.40f));
		public static readonly DetailID ColorDetail = CreateNewDetail(new Detail("Внешний вид", 1000, 800, 1.05f));

		public static DetailID CreateNewDetail(Detail detail)
		{
			DetailID dataBaseID = DetailID.New(_list.Count);
			_list.Add(DetailID.New(_list.Count), detail);

			return (DetailID)dataBaseID;
		}

		public static Detail GetDetailInfoByID(DetailID id)
        {
			return _list.TryGetValue(id, out Detail detail) == true ? detail : default;
		}

		public static int GetDetailCount() => _list.Count;

		public static List<DetailID> GetAllDetailIDs()
        {
			List<DetailID> listOfIDs = new List<DetailID>();

			foreach(KeyValuePair<DetailID, Detail> pair in _list)
            {
                listOfIDs.Add(pair.Key);
            }

			return listOfIDs;
		}
	}

	public static class Helper
	{
		public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue, maxValue);
	}
}
