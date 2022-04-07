using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const ConsoleKey KeyboardButtonExit = ConsoleKey.Escape;
            ConsoleKey pressedKeyboardButton = ConsoleKey.Enter;

            char[,] gameMap = CreateMap(40, 20);

            int xPlayerPosition = 0;
            int yPlayerPosition = 0;

            RandomizeBlocksOnMap(gameMap);
            SpawnPlayer(gameMap,ref xPlayerPosition,ref yPlayerPosition);

            while (pressedKeyboardButton != KeyboardButtonExit)
            {         
                Console.Clear();
                Console.WriteLine("Передвигайтесь по карте с помощью клавиш A-W\nДля выхода нажмите клавишу " + KeyboardButtonExit.ToString() + "\n");

                UpdatePlayer(gameMap,ref xPlayerPosition, ref yPlayerPosition, pressedKeyboardButton);
                DrawMap(gameMap);

                pressedKeyboardButton = Console.ReadKey(true).Key;
            }

            Console.WriteLine("\n\n\nИгра завершена!");
        }

        private static void UpdatePlayer(char[,] map, ref int xPlayerPosition, ref int yPlayerPosition, ConsoleKey keyboardButton)
        {
            const ConsoleKey KeyboardButtonMoveUp = ConsoleKey.W;
            const ConsoleKey KeyboardButtonMoveDown = ConsoleKey.S;
            const ConsoleKey KeyboardButtonMoveLeft = ConsoleKey.A;
            const ConsoleKey KeyboardButtonMoveRight = ConsoleKey.D;

            ConsoleKey PressedCurrentButton = keyboardButton;

            switch (PressedCurrentButton)
            {
                case KeyboardButtonMoveUp:
                    PlayerMove(map,ref xPlayerPosition,ref yPlayerPosition,0,-1);
                    break;

                case KeyboardButtonMoveDown:
                    PlayerMove(map, ref xPlayerPosition, ref yPlayerPosition, 0, 1);
                    break;

                case KeyboardButtonMoveLeft:
                    PlayerMove(map, ref xPlayerPosition, ref yPlayerPosition, -1, 0);
                    break;
                case KeyboardButtonMoveRight:
                    PlayerMove(map, ref xPlayerPosition, ref yPlayerPosition, 1, 0);
                    break;
            }
        }

        private static void PlayerMove(char[,] map,ref int xPlayerPosition,ref int yPlayerPosition,int localX,int localY)
        {
            if (GetMapCell(map, xPlayerPosition + localX, yPlayerPosition + localY) == GetEmptySymbol())
            {
                SetMapCell(map, GetEmptySymbol(), xPlayerPosition, yPlayerPosition);

                xPlayerPosition += localX;
                yPlayerPosition += localY;

                SetMapCell(map,GetPlayerSymbol(),xPlayerPosition,yPlayerPosition);
            }
        }

        private static void SpawnPlayer(char[,] map, ref int xPlayerPosition, ref int yPlayerPosition)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int k = 0; k < map.GetLength(1); k++)
                {
                    if( GetMapCell(map,k,i) == GetEmptySymbol() )
                    {
                        xPlayerPosition = k;
                        yPlayerPosition = i;

                        return;
                    }
                }
            }
        }

        private static char[,] CreateMap(int width, int height)
        {
            char[,] map = new char[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int k = 0; k < width; k++)
                {
                    if( i == 0 || i == (height-1))
                    {
                        map[i, k] = GetBlockSymbol();
                    }
                    else
                    {
                        map[i, k] = GetEmptySymbol();
                    }
                }

                map[i, 0] = GetBlockSymbol();
                map[i, width - 1] = GetBlockSymbol();
            }

            return map;
        }

        private static char GetBlockSymbol() => '#';

        private static char GetEmptySymbol() => '\0';

        private static char GetPlayerSymbol() => '@';

        private static void RandomizeBlocksOnMap(char[,] map)
        {
            const int MinBlockSize = 2;
            const int MaxBlockSize = 4;
            const int BlockCount = 10;

            for (int i = 0; i < BlockCount; i++)
            {
                int randomPositionX = new Random().Next(0, map.GetLength(0));
                int randomPositionY = new Random().Next(0, map.GetLength(0));

                SetBlock( map,randomPositionX,randomPositionY,new Random().Next(MinBlockSize,MaxBlockSize) );
            }
        }

        private static void DrawMap(char [,] map)
        {
            int yOffset = 5;
            
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int k = 0; k < map.GetLength(1); k++)
                {
                    Console.SetCursorPosition(k, yOffset + i);
                    Console.Write( GetMapCell(map,k,i) );
                }
            }
        }

        private static void SetBlock(char [,] map, int xPosition, int yPosition,int Size)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int k = 0; k < Size; k++)
                {
                    SetMapCell(map, GetBlockSymbol(),xPosition + k,yPosition + i);
                }
            }
        }

        private static bool IsPositionCorrect(char[,] map, int xPosition, int yPosition)
        {
            bool isXDimenstionCorrect = (xPosition >= 0 && xPosition < map.GetLength(1));
            bool isYDimenstionCorrect = (yPosition >= 0 && yPosition < map.GetLength(0));

            return (isXDimenstionCorrect && isYDimenstionCorrect);
        }

        private static void SetMapCell(char[,] map,char value, int xPosition,int yPosition)
        {
            if (IsPositionCorrect(map, xPosition, yPosition))
            {
                map[yPosition, xPosition] = value;
            }
        }

        private static char GetMapCell(char[,] map, int xPosition, int yPosition)
        {
            if (IsPositionCorrect(map, xPosition, yPosition))
            {
                return map[yPosition, xPosition];
            }

            return GetBlockSymbol();
        }
    }
}
