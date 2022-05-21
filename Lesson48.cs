using System;
using System.Collections;
using System.Collections.Generic;

namespace Tired
{
	public class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game();

			game.Update();
		}
	}

	public class Game
	{
		private readonly int _updatePerSecond = 100;
		private UpdatablePool<Soilder> _objectPool = new UpdatablePool<Soilder>();

		public bool IsOver { get; private set; } = false;

		public Game()
        {
			CreateExamples();
        }

		public void CreateExamples()
        {
			int soilderCount = 50;
			int offsetX = soilderCount / 2;
			int distanceBeetwen = 4;

			for (int i = 0; i < soilderCount; i++)
			{
				Damager damager = new Damager();

				if (i < offsetX)
				{
					damager.CurrentPosition = new Vector(i * distanceBeetwen, 1);
					damager.SetSkin(new RenderSkin('#', ConsoleColor.Red));
				}
				else
				{
					damager.CurrentPosition = new Vector(i * distanceBeetwen - offsetX * distanceBeetwen, 30);
					damager.SetSkin(new RenderSkin('#', ConsoleColor.Blue));
				}

				_objectPool.Add(damager);
			}
		}
		
		public void Update()
		{
			while (_objectPool != null && IsOver == false)
			{
				_objectPool.Update();

				System.Threading.Thread.Sleep(_updatePerSecond);
				Console.Clear();
			}
		}
	}

    public class Bullet : IUpdatable
    {
		private EnumeratedPool<Soilder> _neighbors;
		private RenderSkin _skin = new RenderSkin('o',ConsoleColor.DarkGreen);
		private Vector _position;
		private Vector _target;
		private int _damage = 0;
		private int _speed = 3;

		public bool IsDestroyed => false;
		
		public Bullet(Vector position,int damage)
        {
			_position = position;
			_damage = damage;
        }


		public void TakeEnumaratedPool<T>(EnumeratedPool<T> enumeratedPool) where T : IUpdatable => _neighbors = enumeratedPool as EnumeratedPool<Soilder>;
		
		public void Update()
		{
			_position.MoveTo(_target,_speed);

            if (_position.GetDistanceTo(_target) < 1)
            {
				IsDestroyed = true;
            }
		}

		public void Draw()
        {
			_skin.Draw(_position);
        }

    
    }

    public sealed class Damager : Soilder
    {
		private int _strength = 5;
		private Vector _safePlace;
		private int _speed = 1;

		public Damager()
        {
			int minStrenght = 5;
			int MaxStrenght = 15;

			SetSkin(new RenderSkin('#',ConsoleColor.Red));

			_strength = Helper.GetRandomValue(minStrenght,MaxStrenght);
        }

		public override void Update()
		{
			SetTarget(SearchNearest(soilder => soilder.CurrentSkin.Color != Skin.Color));

			if (IsValidTarget() == true)
			{
				Position.MoveTo(CurrentTarget.CurrentPosition, _speed);

				if (Position.GetDistanceTo(CurrentTarget.CurrentPosition) < 1)
				{
					Target.TakeDamage(_strength);
				}

			}
		}

        public override void Draw()
        {
			Skin.Draw(CurrentPosition);
		}
    }

	public abstract class Soilder : IUpdatable
	{
		private EnumeratedPool<Soilder> _neighbors;
		protected int Health = 100;
		protected int MaxHealth = 100;
		protected Soilder Target = null;
		protected Vector Position;
		protected RenderSkin Skin;

		public int Fraction
		{
			get
			{
				return (int)Skin.Color;
			}
		}
		public bool IsDead
		{
			get
			{
				return CurrentHealth <= 0;
			}
		}
		public bool IsDestroyed => IsDead;
		public int CurrentMaxHealth
		{
			get
			{
				return MaxHealth;
			}

			private set
			{
				MaxHealth = value;
			}
		}
		public int CurrentHealth
		{
			get
			{
				return Health;
			}

			private set
			{
				int overHealth = (Health + value);

				if (overHealth > CurrentMaxHealth)
				{
					value -= (overHealth - CurrentMaxHealth);
				}
				else if (value < 0)
				{
					value = 0;
				}

				Health = value;
			}
		}
		public int Level { get; private set; } = 1;
		public Vector CurrentPosition
		{
			get
			{
				return Position;
			}

            set
            {
				Position = value;
            }
		}
		public Soilder CurrentTarget
        {
            get
            {
				return Target;
            }
        }
		public RenderSkin CurrentSkin
        {
            get
            {
				return Skin;
            }
        }

		public abstract void Update();

		public abstract void Draw();

		public void TakeEnumaratedPool<T>(EnumeratedPool<T> enumeratedPool) where T : IUpdatable => _neighbors = enumeratedPool as EnumeratedPool<Soilder>;

		public virtual void LevelUp(int levelCount)
        {
			SetMaxHealth( (Level - 1) * 25 + 100);
			SetHealth(CurrentMaxHealth);
        }

		public void SetSkin(RenderSkin skin) => Skin = skin;

		public void SetTarget(Soilder soilder) => Target = soilder;

		public void SetPosition(Vector position) => Position = position;

		public void AddPosition(Vector position) => Position += position;

		public bool IsValidTarget() => (Target != null && Target.IsDestroyed == false);

		public void TakeDamage(int damageCount) => CurrentHealth -= damageCount;

		public void Kill() => Health = 0;

		public void RestoreHealth(int restoreCount) => CurrentHealth += restoreCount;

		public int SetHealth(int health) => CurrentHealth = health;

		public int SetMaxHealth(int maxHealth) => CurrentMaxHealth = maxHealth;

		public Soilder SearchNearest(Predicate<Soilder> additinalPredicate)
        {
			int distance = 1000;

			Soilder foundSoilder = null;

			foreach(Soilder soilder in _neighbors)
            {
				if(soilder != this && Position.GetDistanceTo(soilder.CurrentPosition) < distance && additinalPredicate(soilder) == true)
                {
					distance = Position.GetDistanceTo(soilder.CurrentPosition);
					foundSoilder = soilder;
                }
            }

			return foundSoilder;
        }

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

		public void Update()
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
		public int X { get; private set; }
		public int Y { get; private set; }

		public Vector(int x,int y)
        {
			X = x;
			Y = y;
        }

		public void Set(Vector vector)
        {
			X = vector.X;
			Y = vector.Y;
        }

		public static Vector operator +(Vector a, Vector b) => new Vector { X = a.X + b.X, Y = a.Y + b.Y };

		public static Vector operator -(Vector a, Vector b) => new Vector { X = a.X - b.X, Y = a.Y - b.Y };

		public static Vector operator /(Vector a, Vector b) => new Vector { X = a.X / b.X, Y = a.Y / b.Y };

		public static Vector operator /(Vector a, int b) => new Vector { X = a.X / b, Y = a.Y / b };

		public static Vector operator *(Vector a, int b) => new Vector { X = a.X * b, Y = a.Y * b };

		public int GetLengthSqrt() => (int)Math.Sqrt( (X * X + Y * Y) );

		public int GetDistanceTo(Vector other) => (other - this).GetLengthSqrt();

		public void MoveTo(Vector other, int speed)
        {
			Vector differenceVec = (other - this);
			int sqrtLength = differenceVec.GetLengthSqrt();

			if(sqrtLength == 0)
            {
				sqrtLength = 1;
            }

			Vector normalizedVector = differenceVec / sqrtLength;

			this += normalizedVector * speed;
        }
	}

	public struct RenderSkin
    {
		public char Symbol { get; private set; }
		public ConsoleColor Color { get; private set; }

		public RenderSkin(char symbol, ConsoleColor color)
        {
			Color = color;
			Symbol = symbol;
        }

		public void SetSymbol(char symbol) => Symbol = symbol;

		public void SetColor(ConsoleColor color) => Color = color;

		public void Draw(Vector position)
		{
			Console.ForegroundColor = Color;
			Console.SetCursorPosition(position.X, position.Y);
			Console.Write(Symbol);
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
