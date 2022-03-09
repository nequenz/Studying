using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Как вас зовут?\nМое имя ");
            string userName = Console.ReadLine();

            Console.Write("Сколько вам лет?\nВаш возраст ");
            string userAge = Console.ReadLine();

            Console.Write("Где вы работаете?\nМесто работы ");
            string userJob = Console.ReadLine();

            Console.Write("Ваща зарплата?\nМоя зарплата ");
            string userSalary = Console.ReadLine();

            Console.WriteLine("Вас зовут "+ userName + ", мне "+ userAge + ", я работаю на/в "+ userJob + ", и моя зарплата "+ userSalary);

        }
    }
}
