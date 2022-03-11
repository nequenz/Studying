using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            string nameToEnter = "";
            char rectangleChar = '\0';
            int offsetInRectangle = 1;
            const int RectangleHeight = 3;
           
            Console.Write("Введите свое имя:");
            nameToEnter = Console.ReadLine();

            Console.Write("\nВведите символ:");
            rectangleChar = Console.ReadLine()[0];

            for (int i = 0; i < RectangleHeight; i++)
            {
                int additinalNameSize = 2;

                for(int k = 0; k < nameToEnter.Length + additinalNameSize; k++)
                {
                    if( i == offsetInRectangle )
                    {
                        Console.Write(rectangleChar + nameToEnter + rectangleChar);

                        break;
                    }

                    Console.Write(rectangleChar);
                }
                Console.Write("\n");
            }
        }
    }
}
