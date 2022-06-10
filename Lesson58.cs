using System;
using System.Collections.Generic;
using System.Linq;

namespace Tired
{
    public class Program
    {
        private static List<Fighter> _teamA = new List<Fighter>();
        private static List<Fighter> _teamB = new List<Fighter>();

        private static void Main(string[] args)
        {
            const char StartChar = 'Б';

            CreateTeam(_teamA);
            CreateTeam(_teamB);

            Console.WriteLine("--Команда А--");
            PrintTeam(_teamA);

            Console.WriteLine("--Команда Б--");
            PrintTeam(_teamB);

            Console.WriteLine("--Команда Б с командой А--");

            var team = from Fighter fighter in _teamA
                       where fighter.Name.StartsWith(StartChar)
                       select fighter;

            var unionTeam = _teamB.Union(team);

            PrintTeam(unionTeam);
        }

        public static void CreateTeam(List<Fighter> list)
        {
            const int MaxCount = 25;
            int randomCount = StudyHelper.GetRandomValue(0, MaxCount);

            for (int i = 0; i < randomCount; i++)
            {
                list.Add(new Fighter(StudyHelper.GetRandomName()));
            }
        }

        public static void PrintTeam(IEnumerable<Fighter> list)
        {
            foreach (Fighter fighter in list)
            {
                Console.WriteLine(fighter.Name);
            }
        }
    }

    public class Fighter
    {
        public string Name { get; private set; } = "Безымянный боец";

        public Fighter(string name) => Name = name;
    }

    public static class StudyHelper
    {
        public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue, maxValue);

        public static string GetRandomName()
        {
            const int MaxChars = 5;

            string name = "";

            for (int i = 0; i < MaxChars; i++)
            {
                name += GetRandomRussianChar();
            }

            return name;
        }

        public static char GetRandomRussianChar()
        {
            const int CodePageStart = 1040;
            const int CodePageEnd = 1071;

            return (char)GetRandomValue(CodePageStart, CodePageEnd + 1);
        }
    }
}
