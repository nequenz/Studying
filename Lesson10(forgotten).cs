using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "Jojin";
            string surname = "Mojo";
            string swap = "";

            Console.WriteLine("Имя:"+ name+"\nФамилия:"+ surname);

            swap = name;
            name = surname;
            surname = swap;

            Console.WriteLine("Имя:" + name + "\nФамилия:" + surname);
        }
    }
}
