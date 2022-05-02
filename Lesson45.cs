using System;
using System.Collections.Generic;

/*
	TO-DO

*/

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class UserInputMenu
    {
        private const string WordToExit = "выход";

        private string _wordToRead;
		private Game _game;
		private Dictionary<string, Hero> _heroListToChoose = new Dictionary<string, Hero>();

        public UserInputMenu(Game game)
        {
			_game = game;
			
			InitHeroListToChoose();
        }
		
		public void InitHeroListToChoose()
		{
			_heroListToChoose.Add("Воин",new Warrior());
			_heroListToChoose.Add("Маг",new Mage());
			_heroListToChoose.Add("Разбойник",new Rogue());
			_heroListToChoose.Add("Демон",new Demon());
			_heroListToChoose.Add("Скелет",new Skeleton());
		}

        public void Update()
        {
            while (_game != null && _wordToRead != WordToExit)
            {
                ShowMenu();

                _wordToRead = Console.ReadLine();

                Console.WriteLine();

                switch (_wordToRead)
                {
                    case WordToExit:
                        break;

                    case WordToClear:
                        Console.Clear();
                        break;

                    case WordToCreateDirection:
                        CreateDirectionPathByUser();
                        break;

                    case WordToCreateTrain:
                        CreateTrainByUser();
                        break;
                }
            }
        }

        private void ShowMenu()
        {
			Console.WriteLine("--Игра 'Сражение'---\nИгра закончиться только тогда, когда один из двух сражающихся не погибнет!");
			Console.WriteLine("Перед игрой вы должны выбрать двух персонажей и дать имя имена!");
			ShowHeroesToChoose();
			
        }
		
		public void ShowHeroesToChoose()
		{
			todo:
			
			Console.WriteLine("Первый выбранный персонаж:"+_game.GetEntity(0)?.Name);
			Console.WriteLine("Второй выбранный персонаж:"+_game.GetEntity(1)?.Name);
		}
		
		public void ChooseHeroes()
		{	
			Console.WriteLine("Выберите двух персонажей для сражения:");
			
			for(int i=0;i<_heroListToChoose.Count;i++)
			{
				Console.WriteLine("["+i+"] "+_heroListToChoose[i].key);
			}
			
			Console.Write("Введите тип бойца(типы перечислены выше):");
			
			Hero hero = _heroListToChoose.Get( Console.ReadLine() )?.duplicate();
			
			if(hero != null)
			{
				Console.Write("Введите имя бойца:");
				
				hero.SetName( Console.ReadLine() );
				

				
				if( _game.TryAddHero( hero ) == true )
				{
					Console.WriteLine("Боец создан!");
				}
				
				return;
			}

			Console.WriteLine("Ошибка в создании бойца...");
		}
    }

	sealed class Game
	{
		private List<IUpdateable> _entities = new List<IUpdateable>();

		public int MaxHeroCount {get;private set;} = 2;
		public int Step {get;private set;} = 0;
		public bool IsStarted {get; private set;} = false;
		
		public void Start() => IsStarted = (_entities.Count == MaxHeroCount - 1) ? true : false;
		
		public int GetEntitiesCount() => _entities.Count;
		
		public Hero GetEntity(int index) => (index >= _entities.Count ) ? null : _entities[index];
		
		public bool TryAddHero( Hero hero ) 
		{
			if(_entities.Count<MaxHeroCount)
			{
				_entities.Add(hero);
				
				return true;
			}
			
			return false;
		}
		
		public void Update()
		{
			if(IsStarted == true)
			{
				UpdateEntities();
		
				Step++;
			}
		}
		
		public void UpdateEntities()
		{
			foreach(IUpdateable entity in _entities)
			{
				entity.Update();
			}
		}
		
	}
	
	class Skeleton : Hero
	{
		
	}
	
	class Demon : Hero
	{
		
	}
	
	class Rogue : Hero
	{
		
	}
	
	class Mage : Hero
	{
		
	}
	
	class Warrior : Hero
	{
		
		public Warrior()
		{
			SetStats(200,20,0.12f);
			ability.SetValues(2.2f,0.0f,2.0f);
			ability.SetInfo("Рывок","Вы совершаете рывок противнику, нанося урон в размере "+(ability.DamageValue*100)+" % от вашей силы и оглушая на "+ability.StunValue+" хода.");
		}
		
		public override void UseSpecialAbility()
		{
			if( _target != null )
			{
				ShowAbilityUsing();
				
				_target.TakeDamage( (int)(Strenght*ability.DamageValue) );
				_target.TakeStun(ability.StunValue);
				
			}
			else
			{
				Console.WriteLine("Цель не выбрана!");
			}
		}
	}
	
	abstract class Hero : IUpdateable
	{
		private int _health;
		private Ability _specialAbility;
		private Hero _target;
		
		public bool IsDead {get;private set;} = false;
		public string Name {get;private set;} = "Безымянный";
		public int Strenght {get;private set;} = 10;
		public float Armory {get;private set;} = 0.0f;
		public float Stunned {get;private set;} = 0.0f;
		public int MaxHealth{get;private set;} = 100;
		public int Health
		{
			get
			{
				return _health;
			}
			
			private set
			{
				if(value>MaxHealth)
				{
					_health = MaxHealth;
				}
				else if(value<=0)
				{
					_health = 0;
					IsDead = true;
				}
				else
				{
					IsDead = true;
					_health = value;
				}
			}
			
		}
		
		public void SetStats(int maxHealth, int strength, float armory)
		{
			SetHealth(maxHealth);
			SetMaxHealth(maxHealth);
			SetStrength(strength);
			SetArmory(armory);
		}
		
		public void SetTarget(Hero target) => _target = target;
		
		public void SetName(string name) => Name= name;
		
		public void SetStrength(int strength) => Strenght => strength;
		
		public void SetArmory(float armory) => Armory = armory;
		
		public void SetMaxHealth(int maxHealth) => MaxHealth = maxHealth;
		
		public void SetHealth(int health) => Health = health;
		
		public void TakeDamage(int damageCount) => Health -= (int)damageCount*(1.0f - Armory);
		
		public void TakeStun(float stunTime) => Stunned = stunTime;
		
		public void RestoreHealth(int health) => Health += health;
		
		public void Attack() => _target?.TakeDamage(Strenght);
		
		public abstract void UseSpecialAbility();
		
		public abstract void Update();
		
		public virtual void ShowAbilityUsing() => Console.WriteLine("Персонаж "+Name+" использует "+_specialAbility.ToString());
	}
	
	struct Ability
	{
		public string Name {get;private set;} = "Нет способности";
		public string Description {get;private set;} = "Нет описания";
		public float DamageValue {get;private set;} = 0.0f;
		public float RestoreValue {get;private set;} = 0.0f;
		public float StunValue {get;private set;} = 0.0f;
		
		public Ability(string name, string description) => Set(name,description);
		
		public void SetValues(float damage,float restore,float stun)
		{
			DamageValue = damage;
			RestoreValue = restore;
			StunValue = stun;
		}
		
		public void SetInfo(string name, string description)
		{
			Name = name;
			Description = description;
		}
		
		public override string ToString()
		{
			string info = Name+"\n"+Description+"\n";
			
			if(DamageValue != 0)
			{
				info += "Урон:"+DamageValue+"\n";
			}
			
			if(RestoreValue != 0)
			{
				info += "Восстановление:" + RestoreValue + "\n";
			}
			
			if(StunValue != 0)
			{
				info += "Время оглушение:" + StunValue + "\n";
			}
			
			return info;
		}
	}

	interface IUpdateable
	{
		public void Update();
	}
	
}

