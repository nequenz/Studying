﻿using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите сообщение, которое будет повторяться\n");
            string message = Console.ReadLine();

            Console.Write("Сколько раз повторять сообщение?(Введите значение)\n");
            int repeatCount = Convert.ToInt32(Console.ReadLine());

            for(int i=0; i<repeatCount; i++)
            {
                Console.WriteLine(message);
            }
        }
    }
}
