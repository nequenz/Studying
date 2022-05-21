using System;
using System.Collections;
using System.Collections.Generic;

namespace Tired
{
	public class Program
	{
		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("1");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("2");
		}
	}



	public sealed class Damager : Soilder
    {
		private int _strength = 5;
		private int critChance = 1;

        public override void Update()
        {

        }

        public override void Draw()
        {
           
        }


    }

	public abstract class Soilder : IUpdatable
	{
		private int _health = 100;
		private int _maxHealth = 100;
		private Soilder _target;
		private Vector _position;
		private EnumeratedPool<Soilder> _neighbors;
		private Skin _skin;

		public bool IsDead
        {
            get
            {
				return Health <= 0;
            }
        }
		public bool IsDestroyed => IsDead;
		public int MaxHealth
        {
            get
            {
				return _maxHealth;
            }

			private set
            {
				_maxHealth = value;
            }
        }
		public int Health
        {
            get
            {
				return _health;
            }

			private set
            {
				int overHealth = (_health + value);

				if (overHealth > MaxHealth)
                {
					value -= (overHealth - MaxHealth);
                }
				else if (value < 0)
                {
					value = 0;
                }

				_health = value;
            }
        }
		public int Level { get; private set; } = 1;
		public Vector Position
        {
            get
            {
				return _position;
            }
        }


		public abstract void Update();

		public abstract void Draw();

		public void TakeEnumaratedPool<T>(EnumeratedPool<T> enumeratedPool) where T : IUpdatable => _neighbors = enumeratedPool as EnumeratedPool<Soilder>;

		public virtual void LevelUp(int levelCount)
        {
			SetMaxHealth( (Level - 1) * 25 + 100);
			SetHealth(MaxHealth);
        }

		public void SetSkin(Skin skin) => _skin = skin;

		public void SetTarget(Soilder soilder) => _target = soilder;

		public void SetPosition(Vector position) => _position = position;

		public void AddPosition(Vector position) => _position += position;

		public bool IsValidTarget() => (_target != null && _target.IsDestroyed == false);

		public void TakeDamage(int damageCount) => Health -= damageCount;

		public void RestoreHealth(int restoreCount) => Health += restoreCount;

		public int SetHealth(int health) => Health = health;

		public int SetMaxHealth(int maxHealth) => MaxHealth = maxHealth;

		public Soilder SearchNearest()
        {
			int distance = 1000;

			Soilder foundSoilder = null;

			foreach(Soilder soilder in _neighbors)
            {
				if(soilder != this && Position.GetDistanceTo(soilder.Position) < distance)
                {
					foundSoilder = soilder;
                }
            }

			return foundSoilder;
        }
    }

	public class Game
    {
		private UpdatablePool<Soilder> _objectPool = new UpdatablePool<Soilder>();
    }

	public class EnumeratedPool<T> : IEnumerable where T : IUpdatable
	{
		private readonly List<T> _updateablesList = null;

		public EnumeratedPool(List<T> updatables) => _updateablesList = updatables;

		public List<T> FindAllUpdatableObject(Predicate<T> predicate) => _updateablesList.FindAll(predicate);

		public T FindUpdatableObject(Predicate<T> predicate) => _updateablesList.Find(predicate);

		public T GetUpdatableObjectByIndex(int index)
        {
			if (_updateablesList == null || _updateablesList.Count == 0 
				|| (index<0 && index>=_updateablesList.Count) ) return default(T);

			return _updateablesList[index];
        }

		public IEnumerator GetEnumerator()
        {
			return _updateablesList.GetEnumerator();
		}
	}

	public class UpdatablePool<T> where T : IUpdatable
	{
		private const int GarbageBoundTime = 10000;
		private readonly List<T> _updateablesList = new List<T>();
		private readonly EnumeratedPool<T> _protectedPool = null;
		private readonly List<int> _garbageIndexList = new List<int>();
		private int _garbageTimer = 0;

		public UpdatablePool()
        {
			_protectedPool = new EnumeratedPool<T>(_updateablesList);
		}

		public EnumeratedPool<T> GetEnumeratedPool() => _protectedPool;

		private void CollectGarbage()
        {
			foreach (int index in _garbageIndexList)
            {
				_updateablesList.RemoveAt(index);
            }

			_garbageIndexList.Clear();
        }

		private void Update()
		{
			for (int i = 0; i < _updateablesList.Count; i++)
			{
				T updatable = _updateablesList[i];

				updatable.TakeEnumaratedPool(_protectedPool);
				updatable.Update();
				updatable.Draw();

				if (updatable.IsDestroyed == true)
				{
					_garbageIndexList.Add(i);
				}
			}

			_garbageTimer++;

			if(_garbageTimer >= GarbageBoundTime)
            {
				CollectGarbage();

				_garbageTimer = 0;
            }
		}

		public void Add(T entity) => _updateablesList.Add(entity);
	}

	public struct Vector
	{
		public int X;
		public int Y; 

		public static Vector operator +(Vector a, Vector b) => new Vector { X = a.X + b.X, Y = a.Y + b.Y };
		public static Vector operator -(Vector a, Vector b) => new Vector { X = a.X - b.X, Y = a.Y - b.Y };


		public int GetLengthSqrt() => (int)Math.Sqrt( (X * X + Y * Y) );
		public int GetDistanceTo(Vector target) => (target - this).GetLengthSqrt();
	}

	public struct Skin
    {
		private char _symbol;
		private ConsoleColor _color;

		public Skin(char symbol, ConsoleColor color)
        {
			_color = color;
			_symbol = symbol;
        }

		public void SetSymbol(char symbol) => _symbol = symbol;

		public void SetColor(ConsoleColor color) => _color = color;

		public void Draw(ref Vector position)
		{
			Console.ForegroundColor = _color;
			Console.SetCursorPosition(position.X, position.Y);
			Console.Write(_symbol);
			Console.ResetColor();
		}

    }

	public interface IUpdatable
    {
		public bool IsDestroyed { get; }
		public void Update();
		public void Draw();
		public void TakeEnumaratedPool<T>(EnumeratedPool<T> enumeratedPool) where T : IUpdatable;
    }

	public static class Helper
    {
		public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue,maxValue);
    }
	
}
