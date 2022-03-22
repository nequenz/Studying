using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            DrawBar(100, 30, 10, 3);
        }

        private static void DrawBar(int maxValue, int currentValue, int posX = 0, int posY = 0)
        {
            const int BarSize = 10;
            int barCellsCount = maxValue / BarSize;

            for (int i = 0; i < posY; i++)
            {
                Console.WriteLine();
            }

            for (int i = 0; i < posX; i++)
            {
                Console.Write(' ');
            }

            Console.Write('[');

            for (int i = 0; i < BarSize; i++)
            {
                int drawnCellCount = currentValue / barCellsCount;

                if (i < drawnCellCount)
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('_');
                }
            }

            Console.Write(']');
        }
    }
}
