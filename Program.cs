using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите сообщение, которое будет повторяться");
            string message = Console.ReadLine();

            Console.Write("Сколько раз повторять сообщение?\n(Введите значение)");
            int repeatCount = Convert.ToInt32(Console.ReadLine());

            for(int i=0; i<repeatCount; i++)
            {
                Console.WriteLine(message);
            }
        }
    }
}
