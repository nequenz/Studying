using System;
using System.Collections.Generic;

namespace Tired
{
	class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game();
			UserInputMenu menu = new UserInputMenu(game);

			menu.Update();
		}
	}

	class UserInputMenu
	{
		private const string WordToExit = "выход";
		private const string WordToChooseHero = "герои";
		private const string WordToStartGame = "начать";

		private string _wordToRead;
		private Game _game;
		private Dictionary<string, Hero> _heroListToChoose = new Dictionary<string, Hero>();

		public UserInputMenu(Game game)
		{
			_game = game;

			InitHeroListToChoose();
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

					case WordToChooseHero:
						ChooseHeroesByUser();
						break;

					case WordToStartGame:
						StartGameByUser();
						break;
				}

				UpdateGame();
			}
		}

		private void InitHeroListToChoose()
		{
			_heroListToChoose.Add("Воин", new Warrior());
			_heroListToChoose.Add("Маг", new Mage());
			_heroListToChoose.Add("Разбойник", new Rogue());
			_heroListToChoose.Add("Демон", new Demon());
			_heroListToChoose.Add("Скелет", new Skeleton());
		}

		private void ShowMenu()
		{
			const int sleepTime = 1500;

			System.Threading.Thread.Sleep(sleepTime);
			Console.Clear();
			Console.WriteLine("--Игра 'Сражение'---\nИгра закончиться только тогда, когда один из двух сражающихся не погибнет!");
			Console.WriteLine("Введите <" + WordToExit + "> для выхода!");
			Console.WriteLine("Введите <" + WordToChooseHero + "> чтобы выбрать героя из списка!");
			Console.WriteLine("Введите <" + WordToStartGame + "> чтобы начать сражение!");
			Console.WriteLine("Перед игрой вы должны выбрать двух персонажей и дать им имена!");
			ShowChosenHeroes();
		}

		private void StartGameByUser()
        {
			_game.Start();

			if(_game.IsStarted == true && _game.IsOver == false)
            {
				Hero first = (_game.GetEntity(0) as Hero);
				Hero second = (_game.GetEntity(1) as Hero);

				first.SetTarget(second);
				second.SetTarget(first);
			}
		}

		private void UpdateGame()
        {
			_game.Update();

			if(_game.IsOver == true)
			{
				Hero first = _game.GetEntity(0) as Hero;
				Hero second = _game.GetEntity(1) as Hero;

				Console.WriteLine("Бой окончен!");
				Console.WriteLine("Боец " + first.GetStatus());
				Console.WriteLine("Боец " + second.GetStatus());

			}
		}

		private void ShowChosenHeroes()
		{
			Hero first = (_game.GetEntity(0) as Hero);
			Hero second = (_game.GetEntity(1) as Hero);

			Console.WriteLine();

			Console.WriteLine("Первый выбранный персонаж:" + first?.GetStatus());
			Console.WriteLine("Второй выбранный персонаж:" + second?.GetStatus());
		}

		private void ChooseHeroesByUser()
		{
			Console.WriteLine("Выберите персонажа для сражения:");

			foreach(KeyValuePair<string, Hero> pair in _heroListToChoose)
            {
				Console.WriteLine("[" + pair.Key + "]");
			}

			Console.Write("Введите тип бойца(типы перечислены выше):");

			if(_heroListToChoose.TryGetValue(Console.ReadLine(),out Hero hero) == true)
            {
				hero = hero.Clone() as Hero;

				Console.Write("Введите имя бойца:");

				hero.SetName(Console.ReadLine());

				if (_game.TryAddEntity(hero) == true)
				{
					Console.WriteLine("Боец выбран!");
				}

				return;
			}

			Console.WriteLine("Ошибка в создании бойца...");
		}
	}

	sealed class Game
	{
		private List<IUpdateable> _entities = new List<IUpdateable>();
		private readonly int _stepPerMilisecond = 1500;

		public int MaxEntityCount { get; private set; } = 2;
		public int Step { get; private set; } = 0;
		public bool IsStarted { get; private set; } = false;
		public bool IsOver { get; private set; } = false;

		public void Start()
        {
			IsStarted = IsEntityListFull();
			IsOver = false;
		}

		public bool IsEntityListFull() => (_entities.Count == MaxEntityCount);

		public int GetEntitiesCount() => _entities.Count;

		public IUpdateable GetEntity(int index) => (index >= _entities.Count) ? null : _entities[index];

		public bool TryAddEntity(IUpdateable hero)
		{
			if (_entities.Count < MaxEntityCount)
			{
				_entities.Add(hero);

				return true;
			}

			return false;
		}

		public void Update()
		{
            while (IsStarted == true && IsOver == false)
            {
				UpdateEntities();

				Step++;
			}
		}

		private void UpdateEntities()
		{
			foreach (IUpdateable entity in _entities)
			{
				Hero hero = entity as Hero;

				if(hero.IsDead == false)
                {
					entity.Update();
                }
                else
                {
					IsOver = true;
					IsStarted = false;

					return;
				}

				SleepStep();
			}
		}

		private void SleepStep() => System.Threading.Thread.Sleep(_stepPerMilisecond);
	}

	class Skeleton : Hero
	{
		public Skeleton()
        {
			SetStats(50, 5, 0.01f);
			_specialAbility.SetValues(2.0f, 0.0f, 0.0f);
			_specialAbility.SetInfo("Тык", "Вы тыкаете выпирающей костью, нанося урон в размере "
				+ (_specialAbility.DamageValue * 100) + " % от вашей силы. Также у вас есть шанс рассыпаться во время сражения!");

		}

		public override void Update()
		{
			base.Update();
			UseSpecialAbility();
		}

		public override void UseSpecialAbility()
		{
			int randomMax = 10;
			int randomMin = 0;
			base.UseSpecialAbility();

			int randomDeath = new Random().Next(randomMin,randomMax);

			if(randomDeath == 0)
            {
				TakeDamage(Health);
            }
		}
	}

	class Demon : Hero
	{
		public Demon()
        {
			SetStats(570, 85, 0.70f);
			_specialAbility.SetValues(2.75f, 18.0f, 0.0f);
			_specialAbility.SetInfo("Пожирание", "Вы пожираете душу противника, нанося урон в размере "
				+ (_specialAbility.DamageValue * 100) + " % от вашей силы" +
                " и восполняя "+_specialAbility.RestoreValue+" ед. здоровья");
		}

		public override void Update()
		{
			base.Update();
			UseSpecialAbility();
		}
	}

	class Rogue : Hero
	{
		public Rogue()
        {
			SetStats(180, 12, 0.10f);
			_specialAbility.SetValues(3.75f, 0.0f, 0.0f);
			_specialAbility.SetInfo("Удар в спину", "Вы бьете кинжалом в спину, нанося урон в размере " + (_specialAbility.DamageValue * 100) + " % от вашей силы.");
		}

		public override void Update()
		{
			base.Update();
			UseSpecialAbility();
		}
	}

	class Mage : Hero
	{
		public Mage()
        {
			SetStats(120, 43, 0.05f);
			_specialAbility.SetValues(1.25f, 0.0f, 0.0f);
			_specialAbility.SetInfo("Огненный шар", "Вы наносите огненный шаром урон, в размере " + (_specialAbility.DamageValue * 100) + " % от вашей силы.");
		}

		public override void Update()
		{
			base.Update();
			UseSpecialAbility();
		}
    }

	class Warrior : Hero
	{
		public Warrior()
		{
			SetStats(200, 20, 0.22f);
			_specialAbility.SetValues(2.4f, 0.0f, 0.0f);
			_specialAbility.SetInfo("Рывок", "Вы совершаете рывок противнику, нанося урон в размере " + (_specialAbility.DamageValue * 100) + " % от вашей силы.");
		}

        public override void Update()
        {
			base.Update();
			UseSpecialAbility();
		}
	}

	abstract class Hero : IUpdateable, ICloneable
	{
		protected int _health;
		protected Ability _specialAbility;
		
		public bool IsDead { get; private set; } = false;	
		public string Name { get; private set; } = "Безымянный";
		public int Strenght { get; private set; } = 10;
		public float Armory { get; private set; } = 0.0f;
		public float Stunned { get; private set; } = 0.0f;
		public int MaxHealth { get; private set; } = 100;
		public int Health
		{
			get
			{
				return _health;
			}

			private set
			{
				if (value > MaxHealth)
				{
					_health = MaxHealth;
				}
				else if (value <= 0)
				{
					_health = 0;
					IsDead = true;
				}
				else
				{
					_health = value;
				}
			}

		}

		protected Hero Target { get; private set; }

		public void SetStats(int maxHealth, int strength, float armory)
		{
			SetMaxHealth(maxHealth);
			SetHealth(maxHealth);
			SetStrength(strength);
			SetArmory(armory);
		}

		public void SetTarget(Hero target) => Target = target;

		public void SetName(string name) => Name = name;

		public void SetStrength(int strength) => Strenght = strength;

		public void SetArmory(float armory) => Armory = armory;

		public void SetMaxHealth(int maxHealth) => MaxHealth = maxHealth;

		public void SetHealth(int health) => Health = health;

		public void TakeDamage(int damageCount) => Health -= (int)(damageCount * (1.0f - Armory));

		public void TakeStun(float stunTime) => Stunned = stunTime;

		public void RestoreHealth(int health) => Health += health;

		public void Attack() => Target?.TakeDamage(Strenght);

		public string GetStatus() => (IsDead == false) ? Name + ":"+Health+"/"+MaxHealth+" жив" : Name + ":" + Health + "/" + MaxHealth + " мертв";

		public bool IsTargetValid() => (Target != null && Target.IsDead == false);

		public virtual void UseSpecialAbility()
        {
			if (IsTargetValid())
			{
				ShowAbilityUsing();

				Target.TakeDamage((int)(Strenght * _specialAbility.DamageValue));
				Target.TakeStun(_specialAbility.StunValue);
				RestoreHealth((int)_specialAbility.RestoreValue);
			}
			else
			{
				Console.WriteLine("Цель не выбрана!");
			}
		}

		public virtual void Update()
        {
			Console.WriteLine("Ход персонажа "+Name+"\nЗдоровье:"+Health+"/"+MaxHealth);
        }

		public virtual void ShowAbilityUsing() => Console.WriteLine("Персонаж " + Name + " использует " + _specialAbility.ToString());

		public virtual object Clone()
        {
			Hero cloned = MemberwiseClone() as Hero;
			cloned._specialAbility = _specialAbility;

			return cloned;
		}
    }

	struct Ability 
	{
		public string Name { get; private set; } 
		public string Description { get; private set; }
		public float DamageValue { get; private set; } 
		public float RestoreValue { get; private set; } 
		public float StunValue { get; private set; } 

		public Ability(string name, string description)
        {
			Name = name;
			Description = description;
			DamageValue = 0.0f;
			RestoreValue = 0.0f;
			StunValue = 0.0f;
		}

		public void SetValues(float damage, float restore, float stun)
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
			string info = Name + "\n'" + Description + "'\n";

			if (DamageValue != 0)
			{
				info += "[Урон:" + DamageValue + "]\n";
			}

			if (RestoreValue != 0)
			{
				info += "[Восстановление:" + RestoreValue + "]\n";
			}

			if (StunValue != 0)
			{
				info += "[Время оглушение:" + StunValue + "]\n";
			}

			return info;
		}
	}

	interface IUpdateable
	{
		public void Update();
	}

}
