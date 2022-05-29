using System;
using System.Collections;
using System.Collections.Generic;

namespace Tired
{
	public delegate void IUpdatableHandler(IUpdatable updatable);

	public class Program
	{
		static void Main(string[] args)
		{
			Zoo zoo = new Zoo();

			zoo.Update();
		}
	}

	public sealed class Zoo
    {
		private const int SecondFactor = 1000;
		private const string WordToExit = "exit";

		private readonly int _updatePerSecond = 3;
		private Aviary[] _aviaries = {

			Aviary.CreatePopulatedAviary<Leon>("Вальер с львами",2,2),
			Aviary.CreatePopulatedAviary<Chicken>("Вальер с курицами",2,2),
			Aviary.CreatePopulatedAviary<Monkey>("Вальер с обезьяными",2,2),
			Aviary.CreatePopulatedAviary<Parrot>("Вальер с попугаями",2,2),

		};
		private Aviary _chosenAviary = null;

		public void Update()
        {
			string word="";

			while (word != WordToExit)
            {
				DrawInformation();

				Console.Write("Ввод:");

				word = Console.ReadLine();

				if (word == WordToExit)
				{
					continue;
				}
				else if (int.TryParse(word, out int aviaryNumber) == true
					&& (aviaryNumber >= 0 && aviaryNumber < _aviaries.Length))
				{
					_chosenAviary = _aviaries[aviaryNumber];

					Console.WriteLine("Вы подошли к '" + _chosenAviary.Name + "'");
				}
				else if (word=="")
                {
					UpdateChosenAviary();
					Wait();
				}
				else
				{
					Console.WriteLine("Ошибка ввода...");
				}

				Console.Clear();
			}
        }

		private void DrawInformation()
        {
			Console.WriteLine("Введите цифру вальера, чтобы подойти к нему." +
                "\nВведите "+WordToExit+", чтобы выйти." +
                "\nПропустите ввод, чтобы осмотреть вальер.\n");

			for(int i = 0; i < _aviaries.Length; i++)
            {
				Console.WriteLine("Цифра "+i+", чтобы подойти к '"+_aviaries[i].Name+"'");
            }

			if(_chosenAviary != null)
            {
				Console.WriteLine("Вы стоите у '"+_chosenAviary.Name+"'");
            }

			Console.WriteLine();
        }

		private void Wait() => System.Threading.Thread.Sleep(_updatePerSecond * SecondFactor);

		private void UpdateChosenAviary() => _chosenAviary.Update();
    }

    public sealed class Aviary: UpdatablePool<Animal>
    {
		public string Name { get; private set; } = "Безымянный вальер";

		public Aviary(string name, int maxAnimalCapacity)
        {
			Name = name;
			GetEnumeratedPool().SetCapacity(maxAnimalCapacity);
		}

		public static Aviary CreatePopulatedAviary<T>(string aviaryName,int capacity,int animalCount) where T : Animal, new()
        {
			Aviary aviary = new Aviary(aviaryName, capacity);

			for(int i=0;i< animalCount; i++)
            {
				aviary.GetEnumeratedPool().CreateInstance( new T()).SetGender( 
					(GenderID) Helper.GetRandomValue(0,(int)GenderID.Mutant));
            }

			return aviary;
        }
    }

    public sealed class Parrot : Animal
    {
		public override string Name => "Попугай";

        public override void Sound()
        {
			Console.WriteLine("Чирикает...");
        }
    }

	public sealed class Monkey : Animal
    {
		public override string Name => "Обезьяна";

        public override void Sound()
        {
			Console.WriteLine("Издает неприятные звуки..");
        }
    }

    public sealed class Chicken : Animal
    {
		public override string Name => "Курица";

		public override void Sound() => Console.WriteLine("Кудахчет");
    }

    public sealed class Leon : Animal
    {
		public override string Name => "Лев";

        public override void Sound() => Console.WriteLine("Рычит");
	}

	public enum GenderID
    {
		Male,
		Female,
		Mutant
    }

    public abstract class Animal : IUpdatable
    {
		private static string[] GenderTypeNames = {"мужской","женский","Мутант"};

		private GenderID _genderID = GenderID.Mutant;

		public  abstract string Name { get; }
		public  string Gender
        {
            get
            {
				return GenderTypeNames[(int)_genderID];
            }
        }
		public GenderID GenderId
        {
            get
            {
				return _genderID;
            }
        }
		public bool IsDestroyed => false;

		public void SetGender(GenderID genderID) => _genderID = genderID;

        public void Draw()
        {
            
        }

        public void Update()
        {
			int chanceBound= 4;
			int chanceMax = 10;
			int chanceToSound = Helper.GetRandomValue(0, chanceMax);

            if (chanceToSound < chanceBound)
            {
				Sound();
            }
            else
            {
				Console.WriteLine("Что-то делает...");
            }
        }

		public abstract void Sound();
	}

    public sealed class EnumeratedPool<T> : IEnumerable where T : IUpdatable
	{
		private readonly List<T> _updateablesList = null;

		public int Capacity { get; private set; } = 256;
		public int Amount
        {
            get
            {
				return _updateablesList.Count;
            }
        }

		public EnumeratedPool(List<T> updatables) => _updateablesList = updatables;

		public int SetCapacity(int capacity) => Capacity = capacity;

		public T CreateInstance(T instance)
        {
            if (_updateablesList.Count < Capacity)
            {
				_updateablesList.Add(instance);
			}

			return instance;
		}

		public List<T> FindAllUpdatableObject(Predicate<T> predicate) => _updateablesList.FindAll(predicate);

		public T FindUpdatableObject(Predicate<T> predicate) => _updateablesList.Find(predicate);

		public T GetUpdatableObjectByIndex(int index)
        {
			if (_updateablesList == null || _updateablesList.Count == 0 
				|| (index<0 && index>=_updateablesList.Count) ) return default(T);

			return _updateablesList[index];
        }

		public IEnumerator GetEnumerator()=> _updateablesList.GetEnumerator();
	}

	public class UpdatablePool<T> where T : IUpdatable
	{
		private readonly EnumeratedPool<T> _protectedPool = null;
		private readonly List<T> _updatablesList = new List<T>();
		private readonly List<int> _garbageIndexList = new List<int>();

		private event IUpdatableHandler _onDestroyEvent;

		public UpdatablePool()
        {
			_protectedPool = new EnumeratedPool<T>(_updatablesList);
		}

		public EnumeratedPool<T> GetEnumeratedPool() => _protectedPool;

		private void CollectGarbage()
        {
			for(int i = _garbageIndexList.Count-1; i >=0; i--)
            {
				_updatablesList.RemoveAt(_garbageIndexList[i]);
			}

			_garbageIndexList.Clear();
        }

		public void Update()
		{
			for (int i = 0; i < _updatablesList.Count; i++)
			{
				T updatable = _updatablesList[i];

				if(updatable.IsDestroyed == false)
                {
					updatable.Update();
					updatable.Draw();
                }
                else
                {
					_onDestroyEvent?.Invoke(updatable);
					_garbageIndexList.Add(i);
				}
			}

			CollectGarbage();
		}

		public void Add(T entity) => _updatablesList.Add(entity);

		public void AddListenerOnDestroyEvent(IUpdatableHandler updatableHandler) => _onDestroyEvent += updatableHandler;

		public void RemoveListenerOnDestroyEvent(IUpdatableHandler updatableHandler) => _onDestroyEvent -= updatableHandler;
	}

	public interface IUpdatable
    {
		public bool IsDestroyed { get; }
		public void Update();
		public void Draw();
    }

	public static class Helper
    {
		public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue,maxValue);
    }
	
}
