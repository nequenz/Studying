using System;
using System.Collections.Generic;
using System.Linq;

namespace Tired
{
    public class Program
    {
        private static List<Player> _playerList = new List<Player>();

        private static void Main(string[] args)
        {
            CreateRandomPlayers();

            Console.WriteLine("Топ 3 игрока по уровню");
            RequestTopPlayersByField(PlayerFields.Level);

            Console.WriteLine("\nТоп 3 игрока по силе");
            RequestTopPlayersByField(PlayerFields.Strength);
        }

        public static void RequestTopPlayersByField(PlayerFields fieldIndex)
        {
            const int TopCount = 3;

            var sortedList = _playerList.OrderByDescending(player => player.GetFieldValue(fieldIndex)).Take(TopCount);

            foreach (Player player in sortedList)
            {
                player.PrintInfo();
            }
        }

        public static void CreateRandomPlayers()
        {
            const int MaxPlayersCount = 25;

            for (int i = 0; i < MaxPlayersCount; i++)
            {
                _playerList.Add(StudyHelper.CreateRandomPlayer());
            }
        }
    }

    public enum PlayerFields
    {
        Level,
        Strength
    }

    public struct Field
    {
        private dynamic _value;

        public Field(dynamic field) => _value = field;

        public dynamic GetValue()
        {
            return _value;
        }
    }

    public class Player
    {
        private Field[] _fieldsToCapture = new Field[2];

        public Field Name { get; private set; } = new Field("Undefined");
        public Field Level { get; private set; } = new Field(1);
        public Field Strength { get; private set; } = new Field(1);

        public Player(string name, int level, int strength)
        {
            Name = new Field(name);
            Level = new Field(level);
            Strength = new Field(strength);

            _fieldsToCapture[0] = Level;
            _fieldsToCapture[1] = Strength;
        }

        public int GetFieldValue(PlayerFields index) => (int)_fieldsToCapture[(int)index].GetValue();

        public void PrintInfo()
        {
            Console.WriteLine("Ник:" + Name.GetValue() + ", Уровень:" + Level.GetValue() + ", Сила:" + Strength.GetValue());
        }
    }

    public static class StudyHelper
    {
        private static string[] _names = { "Olov", "Bulty", "Raulf", "Gehri", "Shon", "Loden", "Red", "Drama" };

        public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue, maxValue);

        public static Player CreateRandomPlayer()
        {
            const int MaxLevel = 60;
            const int MaxStrength = 100;

            string name = _names[GetRandomValue(0, _names.Length)] + " " + _names[GetRandomValue(0, _names.Length)];
            int level = GetRandomValue(0, MaxLevel);
            int strength = GetRandomValue(1, MaxStrength);

            return new Player(name, level, strength);
        }
    }
}
