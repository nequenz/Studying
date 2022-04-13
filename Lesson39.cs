using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            Player mainPlayer = new Player(12,8);
            Renderer currentRenderer = new Renderer(mainPlayer);

            currentRenderer.DrawPlayer();
        }
    }

    class Player
    {
        public int positionX { get; private set; } = 0;
        public int positionY { get; private set; } = 0;
        public char symbolSkin { get; set; } = '@';

        public Player(int positionX, int positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;
        }
    }

    class Renderer
    {
        private Player _drawingPlayer = null;

        public Renderer(Player _player)
        {
            _drawingPlayer = _player;
        }

        public void DrawPlayer()
        {
            if (_drawingPlayer != null)
            {
                Draw(_drawingPlayer.positionX, _drawingPlayer.positionY, _drawingPlayer.symbolSkin);
            }
        }

        private void Draw(int positionX, int positionY, char symbol = '0')
        {
            Console.SetCursorPosition(positionX, positionY);
            Console.WriteLine(symbol);
        }
    }
}
