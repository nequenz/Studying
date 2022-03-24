using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            char[,] gameMap = CreateMap(20,20);
            RandomizeWallsOnMap(gameMap);
        }

        private static char[,] CreateMap(int width,int height)
        {
            char[,] map = new char[width, height];
            const char wallSymbol = '#';

            for (int i = 0; i < width; i++)
            {
                map[0, i] = wallSymbol;
                map[height - 1, i] = wallSymbol;
            }

            for (int i = 0; i < height; i++)
            {
                map[i, 0] = wallSymbol;
                map[i, width - 1] = wallSymbol;
            }

            return map;
        }

        private static void RandomizeWallsOnMap(char[,] map)
        {
            const int MinRandom = 2;
            const int MaxRandom = 4;
            
            int yWallCount = new Random().Next(MinRandom,MaxRandom);
            
            for(int i=0;)

        }

        private static void DrawMap()
        {
          

        }
    }
}
