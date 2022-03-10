using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "Jojin";
            string surname = "Mojo";
            string temporaryName = "";

            Console.WriteLine("Имя:"+ name+"\nФамилия:"+ surname);

            temporaryName = name;
            name = surname;
            surname = temporaryName;

            Console.WriteLine("Имя:" + name + "\nФамилия:" + surname);
        }
    }
}
