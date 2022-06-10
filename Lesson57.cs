using System;
using System.Collections.Generic;
using System.Linq;

namespace Tired
{
    public class Program
    {
        private static void Main(string[] args)
        {
            new Barracks();
        }
    }

    public class Barracks
    {
        private List<Soilder> _soilders = new List<Soilder>();

        public Barracks()
        {
            AddSoilders();

            PrintSoilderSomeInfo();
        }

        public void AddSoilders()
        {
            _soilders.Add(new Soilder("Тупко", "Солдат", 6,
                new Weapon[]
                {
                    new Weapon("Автомат"),
                    new Weapon("Палка"),
                }));

            _soilders.Add(new Soilder("Пробко", "Сержант", 14,
                new Weapon[]
                {
                    new Weapon("Автомат"),
                    new Weapon("Фонарик"),
                    new Weapon("Нож")
                }));

            _soilders.Add(new Soilder("Харч", "Старший сержант", 19,
                new Weapon[]
                {
                    new Weapon("Автомат новый"),
                    new Weapon("Фонарик"),
                    new Weapon("Длинный нож"),
                    new Weapon("Пистолет"),
                }));

            _soilders.Add(new Soilder("Шматько", "Прапорщик", 35,
                new Weapon[]
                {
                    new Weapon("Пистолет"),
                    new Weapon("Ключи от склада"),
                    new Weapon("Журнал с анекдотами"),
                }));

            _soilders.Add(new Soilder("Жарбенный", "Полковник", 137,
                new Weapon[]
                {
                    new Weapon("Базука"),
                    new Weapon("Еще бузука"),
                    new Weapon("Опять базука..."),
                }));
        }

        public void PrintSoilderSomeInfo()
        {
            Console.WriteLine("Список из данных:");

            var soilderRecords = from Soilder soilder in _soilders
                                 select new
                                 {
                                     Name = soilder.Name,
                                     Rank = soilder.Rank
                                 };

            foreach (var record in soilderRecords)
            {
                Console.WriteLine("Имя:" + record.Name + ", звание:" + record.Rank);
            }
        }
    }

    public class Soilder
    {
        private List<Weapon> _armament = new List<Weapon>();
        public string Name { get; private set; } = "Безымянный солдат";
        public string Rank { get; private set; } = "Солдат 1-го ранга";
        public int DutyTime { get; private set; } = 1;
        public void GiveWeapon(Weapon weapon) => _armament.Add(weapon);

        public Soilder(string name, string rank, int dutyTime, IEnumerable<Weapon> armament)
        {
            Name = name;
            Rank = rank;
            DutyTime = dutyTime;
            _armament.AddRange(armament);
        }

        public string GetInfo() => "Имя:" + ", Звание:" + ", Срок службы:" + ", Вооружение:\n" + GetArmamentNames();

        public string GetArmamentNames()
        {
            string names = "";

            foreach (Weapon weapon in _armament)
            {
                names += weapon.Name + "\n";
            }

            return names;
        }
    }

    public struct Weapon
    {
        public string Name { get; private set; }

        public Weapon(string name) => Name = name;
    }

    public static class StudyHelper
    {
        public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue, maxValue);

    }
}
