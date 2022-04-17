using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck gameDeck = new Deck(24, new GameCardType[] {
                GameCardType.Warrior,
                GameCardType.Archer,
                GameCardType.Wolf,
                GameCardType.Priest
            });

            UserInputMenu menu = new UserInputMenu(gameDeck);

            menu.Update();
        }
    }

    class UserInputMenu
    {
        private const string WordToExit = "exit";
        private const string WordToTakeCards = "take_range";
        private const string WordToTakeCard = "take";

        private string _wordToInput = "";
        private Deck _currentDeck;

        public UserInputMenu(Deck deck)
        {
            _currentDeck = deck;
        }

        public void Update()
        {
            while (_currentDeck != null && _wordToInput != WordToExit)
            {
                switch (_wordToInput)
                {
                    case WordToExit:
                        break;
                    case WordToTakeCards:
                        TakeCardsByUserInput();
                        break;
                }
            }
        }

        private void TakeCardsByUserInput()
        {
            Console.Write("Введите количество карт, которое вы хотите вытащить:");

            if( Int32.TryParse(Console.ReadLine(), out int count) )
            {
                _currentDeck.TakeCards(count);
            }
            else
            {
                Console.Write("Ошибка ввода!");
            }
        }

        private void TakeCardByUserInput()
        {
            Console.Write("Введите номер карты, который вы хотите вытащить (в колоде "+_currentDeck.GetCardsCount()+"карт):");

            if (Int32.TryParse(Console.ReadLine(), out int number))
            {
                //_currentDeck.TakeCard(number);
                
            }
            else
            {
                Console.Write("Ошибка ввода!");
            }

        }

    }

    enum GameCardType
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
        Count
    }

    sealed class Card
    {
        public bool IsTakenByPlayer { get; private set; } = false;
        public GameCardType Type { get; private set; } = GameCardType.Empty;
        public int Level { get; private set; } = 1;

        
        public Card(GameCardType type,int level)
        {
            Type = type;
            Level = level;
        }

        public void TakeMe() => IsTakenByPlayer = true;
    }

    sealed class Deck
    {
        private List<Card> _cards = new List<Card>();
        
        public Deck(int cardCount, GameCardType[] cardTypesArray = null)
        {
            ResetCards(cardCount,cardTypesArray);
        }

        public void ResetCards(int cardCount, GameCardType[] cardTypesArray = null)
        {
            Random currentType = new Random();
            int maxRandomBound = 0;
        
            if(cardTypesArray != null && cardTypesArray.Length != 0)
            {
                maxRandomBound = cardTypesArray.Length;
            }

            for (int i = 0; i < cardCount; i++)
            {
                _cards[i] = new Card( (GameCardType)currentType.Next(0,maxRandomBound), 1 );
            }
        }

        public void TakeCards(int count)
        {
            int takenCards = 0;

            for(int i = 0; i < count; i++)
            {
                Card currentCard = _cards[i];

                if (currentCard != null && currentCard.IsTakenByPlayer == false)
                {
                    currentCard.TakeMe();
                    takenCards++;
                }
                else
                {
                    i--;
                    continue;
                }
            }

            Console.WriteLine("Вы вытащили " + takenCards + " карт(у/ы) из колоды!");
            Console.WriteLine("В колоде осталось "+GetFreeCardsCount()+" карт(ы)!");
        }

        public void TakeCurrentCard(int number)
        {
            if(number>=0 && number < _cards.Count)
            {
                if(_cards[number] != null && _cards[number].IsTakenByPlayer == false)
                {
                    _cards[number].TakeMe();
                }
            }

        }

        public int GetFreeCardsCount()
        {
            int freeCardsCount = 0;

            for (int i = 0; i < _cards.Count; i++)
            {
                if(_cards[i].IsTakenByPlayer == false)
                {
                    freeCardsCount++;
                }
            }

            return freeCardsCount;
        }

        public int GetCardsCount() => _cards.Count;
    }

}
