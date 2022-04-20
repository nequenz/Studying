using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            UserInputMenu menu = new UserInputMenu(new Deck(20));

            menu.Update();
        }
    }

    class UserInputMenu
    {
        private const string WordToExit = "exit";
        private const string WordToTake = "take";
        private const string WordToTakeSome = "some";

        private string _wordToRead;
        private Deck _deck;

        public UserInputMenu(Deck deck)
        {
            _deck = deck;
        }

        public void Update()
        {
            while (_deck != null && _deck.AreAllCardsTaken() == false && _wordToRead != WordToExit)
            {
                Draw();
                Console.Write("Ввод:");

                _wordToRead = Console.ReadLine();

                Console.WriteLine();

                switch (_wordToRead)
                {
                    case WordToExit:
                        break;

                    case WordToTake:
                        TakeCardByUser();
                        break;

                    case WordToTakeSome:
                        TakeCardsByUser();
                        break;
                }

                Console.Clear();
            }

            _deck.ShowTakenCards();
        }

        public void TakeCardByUser()
        {
            Console.WriteLine("Введите X и Y позицию карты:");

            if ( Int32.TryParse(Console.ReadLine(), out int positionX) && Int32.TryParse(Console.ReadLine(), out int positionY) )
            {
                _deck.TakeCardByPosition(positionX,positionY);
            }
            else
            {
                Console.Write("Ошибка ввода!");
            }
        }

        public void TakeCardsByUser()
        {
            Console.Write("Введите количество карт:");

            if (Int32.TryParse(Console.ReadLine(), out int count) )
            {
                _deck.TakeCards(count);
            }
            else
            {
                Console.Write("Ошибка ввода!");
            }
        }

        public void DrawInfo()
        {
            Console.WriteLine("\nВведите take и по очередно номер карты по горизонтали, а потом по вертикали чтобы вытащить ее!");
            Console.WriteLine("Введите some и количество карт чтобы вытащить случайные!\n");
        }

        public void Draw()
        {
            DrawInfo();
            _deck.DrawCards();
        }
    }

    class Deck
    {
        private const int LineWidth = 10;

        private List<Card> _cards = new List<Card>();
        private CardLocalization _cardLocalization = new CardLocalization();
        
        public Deck(int cardCount = LineWidth)
        {
            ResetCards(cardCount);
            RandomizeCards();
        }

        public bool IsCorrectPosition(int positionX, int positionY)
        {
            bool isXCorrect = (positionX >= 0 && positionX < LineWidth);
            bool isYCorrect = (positionY >= 0 && positionY < (_cards.Count/LineWidth) );

            return isXCorrect & isYCorrect;
        }

        public void TakeCardByPosition(int positionX, int positionY)
        {
            positionX--;
            positionY--;

            if( IsCorrectPosition(positionX, positionY) )
            {
                Card card = _cards[positionY * LineWidth + positionX];

                if(card.IsTakenByPlayer == false)
                {
                    card.TakeMe();

                    return;
                }
                else
                {
                    Console.WriteLine("Эта карта уже ваша...\nПовторите выбор");
                }
            }

            Console.WriteLine("Ошибка координат выбора карты");
        }

        public void TakeCards(int count)
        {
            count = count > GetCardsCount() == true ? GetCardsCount() : count;

            for (int i=0; i < GetCardsCount(); i++)
            {
                if (count == 0)
                {
                    return;
                }

                Card card = _cards[i];

                if(card.IsTakenByPlayer == false)
                {
                    card.TakeMe();
                    count--;
                }
            }
        }

        public int GetCardsCount() => _cards.Count;

        public int GetCardsCountWith(bool isTakenPlayer = false)
        {
            int count = 0;

            foreach(Card card in _cards)
            {
                if( card.IsTakenByPlayer == isTakenPlayer)
                {
                    count++;
                }
            }

            return count;
        }

        public bool AreAllCardsTaken() => (GetCardsCountWith(false) == 0);

        public void ResetCards(int cardCount = LineWidth)
        {
            _cards.Clear();

            if (cardCount < LineWidth)
            {
                cardCount = LineWidth;
            }

            for (int i = 0; i < cardCount; i++)
            {
                _cards.Add( new Card(CardType.Empty) );
            }
        }

        public void RandomizeCards()
        {
            for(int i = 0; i < GetCardsCount(); i++)
            {
                CardType randomType = (CardType)new Random().Next(0, (int)CardType.Top);

                _cards[i]?.SetType(randomType);
            }
        }

        public void DrawCards()
        {
            int lineY = 1;

            Console.Write("   ");

            for(int i = 1; i <= LineWidth; i++)
            {
                Console.Write(i + "  ");
            }

            for (int i = 0; i < GetCardsCount(); i++)
            {
                if ( (i % LineWidth == 0) && (i > 0) || (i == 0) )
                {
                    Console.WriteLine();
                    Console.Write(lineY);

                    lineY++;
                }

                if (_cards[i].IsTakenByPlayer == true)
                {
                    Console.Write("  0");
                }
                else
                {
                    Console.Write("  #");
                }
            }

            Console.WriteLine();
        }

        public void ShowTakenCards()
        {
            Console.WriteLine("\nВаши карты:");

            foreach(Card card in _cards)
            {
                if (card.IsTakenByPlayer == true)
                {
                    Console.WriteLine("Карта: " + _cardLocalization.GetCardInfoByType(card.Type));
                }
            }
        }

    }

    enum CardType
    {
        Empty,
        Mage,
        Warrior,
        Archer,
        Wolf,
        Dragon,
        Rat,
        Priest,
        Rogue,
        Shaman,
        Top
    }

    class CardLocalization
    {
        private Dictionary<CardType, string> _dictionary = new Dictionary<CardType, string>();

        public CardLocalization()
        {
            _dictionary.Add(CardType.Empty, "Пусткая карта");
            _dictionary.Add(CardType.Mage, "Маг");
            _dictionary.Add(CardType.Warrior, "Воин");
            _dictionary.Add(CardType.Archer, "лучник");
            _dictionary.Add(CardType.Wolf, "Волк");
            _dictionary.Add(CardType.Dragon, "Дракон");
            _dictionary.Add(CardType.Rat, "Мышь");
            _dictionary.Add(CardType.Priest, "Жрец");
            _dictionary.Add(CardType.Rogue, "разбойник");
            _dictionary.Add(CardType.Shaman, "Шаман");
        }

        public string GetCardInfoByType(CardType type) => _dictionary.TryGetValue(type, out string name) == true ? name : "Непределенная карта";
    }

    class Card
    {
        public CardType Type { get; private set; }
        public bool IsTakenByPlayer { get; private set; } = false;

        public Card(CardType type = CardType.Empty)
        {
            Type = type;
        }

        public void SetType(CardType type)
        {
            if( IsTakenByPlayer == false)
            {
                Type = type;
            }
        }

        public void TakeMe() => IsTakenByPlayer = true;
    }
}
