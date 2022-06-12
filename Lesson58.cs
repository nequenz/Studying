using System;
using System.Collections.Generic;
using System.Linq;

namespace Tired
{
    public class Program
    {
        private static void Main(string[] args)
        {
            FightClub club = new FightClub();

            club.UnionTeams();
        }
    }

    public class FightClub
    {
        private List<Fighter> _teamA = new List<Fighter>();
        private List<Fighter> _teamB = new List<Fighter>();

        public void UnionTeams()
        {
            const char StartChar = 'Б';
            Func<Fighter, bool> conditionToSelect = (fighter => fighter.Name.StartsWith(StartChar) == true);

            OrganizeTeam(_teamA);
            OrganizeTeam(_teamB);

            Console.WriteLine("--Команда А--");
            PrintTeam(_teamA);

            Console.WriteLine("--Команда Б--");
            PrintTeam(_teamB);

            Console.WriteLine("--Команда Б с командой А--");

            var resultTeam = _teamA.Where(conditionToSelect);

            _teamB = _teamB.Union(resultTeam).ToList();
            _teamA = _teamA.Except(resultTeam).ToList();

            PrintTeam(_teamB);

            Console.WriteLine("--Остатки команды А--");
            PrintTeam(_teamA);
        }

        private void OrganizeTeam(List<Fighter> list)
        {
            const int MaxCount = 25;

            StudyRandomizer randomizer = new StudyRandomizer();
            CodePages codePages = new CodePages();
            int randomCount = randomizer.GetRandomValue(0, MaxCount);

            for (int i = 0; i < randomCount; i++)
            {
                list.Add(new Fighter(randomizer.GetRandomName(5, codePages.GetCodePage("RussianTest"))));
            }
        }

        private void PrintTeam(IEnumerable<Fighter> list)
        {
            foreach (Fighter fighter in list)
            {
                Console.WriteLine(fighter.Name);
            }
        }
    }

    public class Fighter
    {
        public string Name { get; private set; }

        public Fighter(string name) => Name = name;
    }

    public struct CodePage
    {
        public int PageStart { get; private set; }
        public int PageEnd { get; private set; }
        public string Name { get; private set; }

        public CodePage(string name, int start, int end)
        {
            Name = name;
            PageStart = start;
            PageEnd = end;
        }
    }

    public class CodePages
    {
        private List<CodePage> _pages = new List<CodePage>();

        public CodePages()
        {
            InitDefaultPages();
        }

        private void InitDefaultPages()
        {
            _pages.Add(new CodePage("RussianUpper", 1040, 1071));
            _pages.Add(new CodePage("RussianTest", 1040, 1044));
        }

        public CodePage GetCodePage(string name) => _pages.Find(codePage => codePage.Name == name);
    }

    public class StudyRandomizer
    {
        private Random _randomizer = new Random();

        public int GetRandomValue(int minValue = 0, int maxValue = 1) => _randomizer.Next(minValue, maxValue + 1);

        public string GetRandomName(int charCount, CodePage page)
        {
            string name = "";

            for (int i = 0; i < charCount; i++)
            {
                name += GetRandomChar(page);
            }

            return name;
        }

        public char GetRandomChar(CodePage page) => (char)GetRandomValue(page.PageStart, page.PageEnd);
    }
}
