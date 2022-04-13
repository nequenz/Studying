using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            Player mainPlayer = new Player(12, 8);
            Renderer currentRenderer = new Renderer(50, 50, mainPlayer);

            currentRenderer.DrawPlayer();
        }
    }

    class Player
    {
        private char _symbolSkin = '@';
        public int PositionX { get; private set; } = 0;
        public int PositionY { get; private set; } = 0;
        public char SymbolSkin
        {
            get => _symbolSkin;

            set
            {
                if (value != '\0')
                {
                    _symbolSkin = value;
                }
                else
                {
                    _symbolSkin = '@';
                }
            }
        }

        public Player(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }
    }

    class Renderer
    {
        private readonly int _maxRenderViewWidth = 50;
        private readonly int _maxRenderViewHeight = 50;

        private Player _drawingPlayer = null;

        public Renderer(int maxRenderViewWidth, int maxRenderViewHeight, Player _player)
        {
            _maxRenderViewWidth = maxRenderViewWidth;
            _maxRenderViewHeight = maxRenderViewHeight;
            _drawingPlayer = _player;
        }

        public void DrawPlayer()
        {
            if (_drawingPlayer != null)
            {
                Draw(_drawingPlayer.PositionX, _drawingPlayer.PositionY, _drawingPlayer.SymbolSkin);
            }
        }

        private void Draw(int positionX, int positionY, char symbol = '0')
        {
            if ((positionX >= 0 && positionX < _maxRenderViewWidth) && (positionY >= 0 && positionY < _maxRenderViewHeight))
            {
                Console.SetCursorPosition(positionX, positionY);
                Console.WriteLine(symbol);
            }
        }
    }
}
