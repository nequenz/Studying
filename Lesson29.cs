using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            DrawBar(100, 30, 10, 3);
        }

        private static void DrawBar(int maxValue, int currentValue, int positionX = 0, int positionY = 0)
        {
            const int BarSize = 10;
            int barCellsCount = maxValue / BarSize;
            int drawnCellCount = currentValue / barCellsCount;

            Console.SetCursorPosition(positionX, positionY);
            Console.Write('[');

            for (int i = 0; i < BarSize; i++)
            {
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
