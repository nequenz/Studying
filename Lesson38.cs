using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            Player mainPlayer = new Player("John Wild", 150);

            mainPlayer.ShowInformation();
        }

    }

    class Player
    {
        private int _health;
        private int _maxHealth;
        private string _name;

        public Player(string name="Unnamed",int health=100)
        {
            _name = name;
            _maxHealth = health;
            _health = health;
        }

        public void ShowInformation()
        {
            Console.WriteLine("Информация о игроке\nМаксимальное здоровье:"+_maxHealth+"\nЗдоровье:"+_health+"\nИмя:"+_name);
        }
    }
}
