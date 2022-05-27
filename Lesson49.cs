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
			Aquarium aquarium = new Aquarium();

			aquarium.Update();
		}
	}


	public sealed class Aquarium
    {
		private const int SecondFactor = 1000;
		private const ConsoleKey KeyToRemove = ConsoleKey.Spacebar;
		private const ConsoleKey KeyToAdd = ConsoleKey.Enter;

		private readonly int _updatePerSecond = 2;
		private UpdatablePool<Fish> _updatablePool = new UpdatablePool<Fish>();

		public int MaxFishCount { get; private set; } = 16;

		public Aquarium()
        {
			_updatablePool.GetEnumeratedPool().SetCapacity(MaxFishCount);
			_updatablePool.GetEnumeratedPool().CreateInstance(new Fish("Золотая рыбка",15));
			_updatablePool.GetEnumeratedPool().CreateInstance(new Fish("Синия рыбка", 7));
			_updatablePool.GetEnumeratedPool().CreateInstance(new Fish("Черная рыбка", 30));
			_updatablePool.GetEnumeratedPool().CreateInstance(new Fish("Лиловая рыбка", 3));
			_updatablePool.GetEnumeratedPool().CreateInstance(new Fish("Красная рыбка", 12));
		}

		public void Update()
        {
			while (_updatablePool != null)
            {
				ShowInfo();

				if(HandleKeys(out ConsoleKey key) == true)
                {
					if(key == KeyToAdd)
                    {
						AddFishByUserInput();
                    }
					else if(key == KeyToRemove)
                    {
						RemoveFishByUserInput();
                    }
                }

				_updatablePool.Update();

				System.Threading.Thread.Sleep(_updatePerSecond * SecondFactor);
				Console.Clear();
            }
        }

		public bool HandleKeys(out ConsoleKey outKey)
        {
			if(Console.KeyAvailable == true)
            {
				outKey = Console.ReadKey(true).Key;

				return true;
			}

			outKey = default;

			return false;
		}

		public void AddFishByUserInput()
        {
			string name;

            if (_updatablePool.GetEnumeratedPool().Amount < MaxFishCount-1)
            {
				Console.Write("Новая рыбка\nВведите имя новой рыбки:");
				name = Console.ReadLine();

				Console.Write("\nВведите ее максимальный возраст:");
				
				if(int.TryParse(Console.ReadLine(),out int maxAge) == true)
                {
					_updatablePool.Create( new Fish(name,maxAge) );

					WriteSleepingLine("Рыбка добавлена!");

					return;
                }
            }

			WriteSleepingLine("Произошла ошибка!");
        }

		public void RemoveFishByUserInput()
        {
			string name;

			Console.Write("Вытащить рыбку из аквариума\nВведите имя рыбки:");
			name = Console.ReadLine();

			Fish fish = _updatablePool.GetEnumeratedPool().FindUpdatableObject(fish => fish.Name == name);

			if(fish != null)
            {
				fish.Destroy();

				WriteSleepingLine("Рыбка удалена с аквириума!");
			}
            else
            {
				WriteSleepingLine("Такой рыбки нет!");
			}
		}

		public void WriteSleepingLine(string text)
        {
			Console.WriteLine(text);
			System.Threading.Thread.Sleep(_updatePerSecond * SecondFactor);
		}

		public void ShowInfo()
        {
			Console.WriteLine("----------Рыбки стареют раз в "+_updatePerSecond+" секунды!------------\n");
			Console.WriteLine("Введите "+KeyToAdd+" чтобы добавить рыбку\nВведите "+KeyToRemove+" чтобы вытащить рыбку\n");
        }
    }


	public sealed class Fish : IUpdatable
    {
		private static string[] _actionnames = {"плавает","спит","кушает","пялится в стенку аквариума","какает","мертва"};
		private int _currentActionID = 0;
		private int _deathActionID = 5;

		public int MaxAge { get; private set; } = 10;
		public int CurrentAge { get; private set; } = 0;
		public bool IsDropped { get; private set; } = false;
		public bool IsDestroyed => IsDropped;
		public string Name { get; private set; } = "Безымянная рыбка";

		public Fish(string name,int maxAge)
        {
			Name = name;
			MaxAge = maxAge;
        }

		public void Destroy() => IsDropped = true;

		public void ChooseRandomAction() => _currentActionID = Helper.GetRandomValue(0, _actionnames.Length - 1);

		public string GetCurrentAction() => _actionnames[_currentActionID];

		public void Draw()
        {
			string lineInfo = "Рыбка " + Name + ", " + CurrentAge + " лет, ";

			Console.WriteLine(lineInfo + GetCurrentAction());
		}

        public void Update()
        {
            if (CurrentAge >= MaxAge)
            {
				_currentActionID = _deathActionID;
            }

			if (_currentActionID != _deathActionID)
            {
				CurrentAge++;

				ChooseRandomAction();
			}
        }
    }

    public class EnumeratedPool<T> : IEnumerable where T : IUpdatable
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

		public void CreateInstance(T instance)
        {
            if (_updateablesList.Count < Capacity)
            {
				_updateablesList.Add(instance);
			}
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

		public void Create(T entity) => _updatablesList.Add(entity);

		public void AddListenerOnDestroyEvent(IUpdatableHandler updatableHandler) => _onDestroyEvent += updatableHandler;

		public void RemoveListenerOnDestroyEvent(IUpdatableHandler updatableHandler) => _onDestroyEvent -= updatableHandler;
	}

	public interface IUpdatable
    {
		public bool IsDestroyed { get; }
		public void Destroy();
		public void Update();
		public void Draw();
    }

	public static class Helper
    {
		public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue,maxValue);
    }
	
}
