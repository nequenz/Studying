using System;
using System.Collections.Generic;

namespace Tired
{
	public class Program
	{
		static void Main(string[] args)
		{
	
		}
	}

	public sealed class Game
    {
		private List<IUpdateable> _entities = new List<IUpdateable>();
		private List<int> _garbageIndixes = new List<int>();

		public void AddEntity(IUpdateable entity) => _entities.Add(entity);

		public void Update()
        {
			UpdateEntities();
			DestroyGarbage();
        }

		private void UpdateEntities()
        {
			for(int i = 0; i < _entities.Count; i++)
            {
				IUpdateable entity = _entities[i];

				entity.Update();

				if (entity.IsDestroyed() == true)
				{
					_garbageIndixes.Add(i);
				}
			}
        }

		private void DestroyGarbage()
        {
			foreach(int index in _garbageIndixes)
            {
				_entities.RemoveAt(index);
            }

			_garbageIndixes.Clear();
        }
    }

	enum SoilderRole
    {
		Tank,
		Damager,
		Healer
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


	public abstract class Soilder : IUpdateable
    {
		private int _health = 100;
		private int _maxHealth = 100;
		private Soilder _target;
		private Vector _position;

		public bool IsDead
        {
            get
            {
				return Health <= 0;
            }
        }
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
		public char DrawSymbol { get; private set; } = '_';

		public Vector Position
        {
            get
            {
				return _position;
            }
        }

		public abstract void Update();

		public abstract void Draw();

		public virtual void LevelUp(int levelCount)
        {
			SetMaxHealth( (Level - 1) * 25 + 100);
			SetHealth(MaxHealth);
        }

		public void SetTarget(Soilder soilder) => _target = soilder;

		public void SetPosition(Vector position) => _position = position;

		public void AddPosition(Vector position) => _position += position;

		public bool IsValidTarget() => (_target != null && _target.IsDestroyed() == false);

		public void TakeDamage(int damageCount) => Health -= damageCount;

		public void RestoreHealth(int restoreCount) => Health += restoreCount;

		public int SetHealth(int health) => Health = health;

		public int SetMaxHealth(int maxHealth) => MaxHealth = maxHealth;

		public bool IsDestroyed() => IsDead;
	}

	public struct Vector
	{
		public int X;
		public int Y;

		public static Vector operator +(Vector a, Vector b) => new Vector { X = a.X + b.X, Y = a.Y + b.Y };
		public static Vector operator -(Vector a, Vector b) => new Vector { X = a.X - b.X, Y = a.Y - b.Y };
	}

	public interface IUpdateable
    {
		public void Update();
		public void Draw();
		public bool IsDestroyed();
    }

	public static class Helper
    {
		public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue,maxValue); 
    }
}
