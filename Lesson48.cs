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
			Game game = new Game();

			game.Update();
		}
	}

	public class Game
	{
		private readonly int _updatePerSecond = 25;
		private UpdatablePool<IUpdatable> _objectPool = new UpdatablePool<IUpdatable>();
		private int _blueSoildersCount = 0;
		private int _redSoildersCount = 0;

		public bool IsOver
        {
            get
            {
				return (_blueSoildersCount==0 || _redSoildersCount==0) ? true : false;
            }
        }

		public Game()
        {
			CreateSoilders();

			_objectPool.AddListenerOnDestroyEvent(ListenDestroyedObjects);
		}

		public void CreateSoilders()
        {
			int soilderCount = 10;
			float offsetX = soilderCount / 2.0f;
			float offsetY = 30.0f;
			float distanceBeetwen = 4.0f;

			for (int i = 0; i < soilderCount; i++)
			{
				Damager damager = new Damager();

				if (i < offsetX)
				{
					damager.CurrentPosition = new Vector(offsetX+i * distanceBeetwen, 1+i);
					damager.SetSkin(new RenderSkin('#', ConsoleColor.Red));
				}
				else
				{
					damager.CurrentPosition = new Vector(i * distanceBeetwen + offsetX, offsetY - i);
					damager.SetSkin(new RenderSkin('#', ConsoleColor.Blue));
				}
 
				_objectPool.Create(damager);
			}

			_blueSoildersCount = soilderCount / 2;
			_redSoildersCount = soilderCount / 2;
		}

		public void ListenDestroyedObjects(IUpdatable updatable)
        {
			Soilder soilder = updatable as Soilder;

            if (soilder != null)
            {
				if(soilder.CurrentSkin.Color == ConsoleColor.Blue)
                {
					_blueSoildersCount--;
                }
                else if(soilder.CurrentSkin.Color == ConsoleColor.Red)
                {
					_redSoildersCount--;
                }
            }
        }

		public void DrawInformation()
        {
			Console.SetCursorPosition(0,0);
			Console.WriteLine("Количество солдат синей армии:" + _blueSoildersCount);
			Console.WriteLine("Количество солдат красной армии:" + _redSoildersCount);
		}
		
		public void Update()
		{
			while (_objectPool != null && IsOver == false)
			{
				_objectPool.Update();
				DrawInformation();
				System.Threading.Thread.Sleep(_updatePerSecond);
				Console.Clear();
			}

			Console.WriteLine("Игра закончена!");

            if (_blueSoildersCount == 0)
            {
				Console.WriteLine("Выиграли красные!");
            }
            else
            {
				Console.WriteLine("Выиграли синие!");
			}
		}
	}

    public class Bullet : IUpdatable
    {
		private EnumeratedPool<IUpdatable> _neighbors;
		private RenderSkin _skin = new RenderSkin('o',ConsoleColor.DarkGreen);
		private bool _isDestroyed = false;
		private Vector _position; 

		public Soilder SoilderTarget { get; private set; } = null;
		public Vector Target { get; private set; }
		public int ShooterFractionColor { get; private set; } = (int)ConsoleColor.White;
		public int Damage { get; private set; } = 0;
		public int Speed { get; private set; } = 3;
		public bool IsDestroyed => _isDestroyed;
		
		public Bullet(Vector from,Vector to,int damage, ConsoleColor shooterFractionColor)
        {
			_position = from;
			Target = to;
			Damage = damage;
			ShooterFractionColor = (int)shooterFractionColor;
        }

		public void Destroy() => _isDestroyed = true;

		public void TakeEnumaratedPool<T>(EnumeratedPool<T> enumeratedPool) where T : IUpdatable 
			=> _neighbors = enumeratedPool as EnumeratedPool<IUpdatable>;

		public void SetSoilderTarget(Soilder soilder) => SoilderTarget = soilder;

		public void Update()
		{
			float minDistanceToCollision = 2.0f;

			_position.MoveTo(Target, Speed);

            if ( _position.GetDistanceTo(Target) < minDistanceToCollision)
            {
				Destroy();

				if(SoilderTarget != null && _position.GetDistanceTo(SoilderTarget.CurrentPosition) < minDistanceToCollision)
                {
					SoilderTarget.TakeDamage(Damage);
                }
			}
		}

		public void Draw()
        {
			_skin.Draw(_position);
        }
    }

    public class Damager : Soilder
    {
		private int _strength = 5;
		private int _reloadTime = 0;
		private int _maxReloadTime = 20;
		private float _speed = 0.5f;
		private float _currentTimeToAvoid = 0.0f;
		private Vector _safePosition;
		
		public Damager()
        {
			int minStrenght = 5;
			int MaxStrenght = 15;

			SetSkin(new RenderSkin('#',ConsoleColor.Red));

			_strength = Helper.GetRandomValue(minStrenght,MaxStrenght);
			_reloadTime = _maxReloadTime/10;
        }

		public void Shoot()
        {
			if(Target != null && _reloadTime <=0)
            {
				Bullet bullet = new Bullet(Position, Target.CurrentPosition, _strength, Skin.Color);

				bullet.SetSoilderTarget(Target);
				Neighbors.CreateInstance(bullet);

				_reloadTime = _maxReloadTime;
			}
		}

		public void Reload()
        {
			if (_reloadTime > 0)
            {
				_reloadTime--;
			}
		}

		public void Avoid()
        {
			int maxBound = 100;

			_currentTimeToAvoid--;

			if (_currentTimeToAvoid <= 0.0f)
            {
				_safePosition.Set( new Vector(Helper.GetRandomValue(0, Console.WindowWidth), Helper.GetRandomValue(0, Console.WindowHeight) ) );

				_currentTimeToAvoid = Helper.GetRandomValue(0, maxBound);
			}


			Position.MoveTo(_safePosition, _speed);
		}


		public override void Update()
		{
			SetTarget( SearchNearestSoilder(soilder => soilder.CurrentSkin.Color != Skin.Color) );
			Reload();
			Shoot();
			Avoid();
		}

        public override void Draw()
        {
			Skin.Draw(CurrentPosition);
		}
    }

	public abstract class Soilder : IUpdatable
	{
		protected EnumeratedPool<IUpdatable> Neighbors;
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

		public void Destroy() => Kill();

		public void TakeEnumaratedPool<T>(EnumeratedPool<T> enumeratedPool) where T : IUpdatable 
			=> Neighbors = enumeratedPool as EnumeratedPool<IUpdatable>;

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

		public Soilder SearchNearestSoilder(Predicate<Soilder> additinalPredicate)
        {
			float distance = 1000.0f;

			Soilder foundSoilder = null;

			foreach(IUpdatable iupdatable in Neighbors)
            {
				Soilder soilder = iupdatable as Soilder;

				if(soilder == null)
                {
					continue;
                }

				if (soilder != this && Position.GetDistanceTo(soilder.CurrentPosition) < distance && additinalPredicate(soilder) == true)
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

		public void CreateInstance(T instance) => _updateablesList.Add(instance); 

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
					updatable.TakeEnumaratedPool(_protectedPool);
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

	public struct Vector
	{
		public float X { get; private set; }
		public float Y { get; private set; }

		public Vector(float x, float y)
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

		public static Vector operator /(Vector a, float b) => new Vector { X = a.X / b, Y = a.Y / b };

		public static Vector operator *(Vector a, float b) => new Vector { X = a.X * b, Y = a.Y * b };

		public float GetLengthSqrt() => MathF.Sqrt( (X * X + Y * Y) );

		public float GetDistanceTo(Vector other) => (other - this).GetLengthSqrt();

		public void MoveTo(Vector other, float speed)
        {
			Vector differenceVec = (other - this);
			float sqrtLength = differenceVec.GetLengthSqrt();

			if(sqrtLength <= 0.5f)
            {
				sqrtLength = 0.5f;
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
			if(position.X>=0 && position.Y>= 0)
            {
				Console.ForegroundColor = Color;
				Console.SetCursorPosition((int)position.X, (int)position.Y);
				Console.Write(Symbol);
				Console.ResetColor();
			}
		}
    }
 
	public interface IUpdatable
    {
		public bool IsDestroyed { get; }
		public void Destroy();
		public void Update();
		public void Draw(); 
		public void TakeEnumaratedPool<T>(EnumeratedPool<T> enumeratedPool) where T : IUpdatable;
    }

	public static class Helper
    {
		public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue,maxValue);
    }
	
}
