using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayerCollector playerCollector = new PlayerCollector();
            PlayerCollectorMenu menu = new PlayerCollectorMenu(playerCollector);

            menu.UpdateInput();
        }
    }

    class PlayerCollectorMenu
    {
        private const string WordToExit = "exit";
        private const string WordToAddPlayer = "add";
        private const string WordToRemovePlayer = "remove";
        private const string WordToBanPlayer = "ban";
        private const string WordToUnbanPlayer = "unban";

        private string _wordToInput = "";
        private PlayerCollector _collector;

        public PlayerCollectorMenu(PlayerCollector collector)
        {
            _collector = collector;
        }

        public void UpdateInput()
        {
            Console.WriteLine("Введите команды для выполнения действия с базой данных");

            while(_collector!=null && _wordToInput != WordToExit)
            {
                Console.Write("Введите команду:");
                _wordToInput = Console.ReadLine();
                Console.WriteLine();

                switch (_wordToInput)
                {
                    case WordToExit:
                        break;
                    case WordToAddPlayer:
                        AddPlayerByUserInput();
                        break;
                    case WordToRemovePlayer:
                        RemovePlayerByUserInput();
                        break;
                    case WordToBanPlayer:
                        SetPlayerBanModeByUserInput(true);
                        break;
                    case WordToUnbanPlayer:
                        SetPlayerBanModeByUserInput(false);
                        break;
                }
            }
        }

        private bool TryReadPlayerIdByUserInput(out int id)
        {
            Console.Write("Введите ID:");

            return Int32.TryParse( Console.ReadLine(), out id);
        }

        private void AddPlayerByUserInput()
        {
            string nickname;
            int level;

            Console.Write("Введите ник игрока:");
            nickname = Console.ReadLine();

            Console.Write("Введите уровень игрока:");

            if(Int32.TryParse( Console.ReadLine(),out level))
            {
                _collector.Add(new Player(nickname,level));
            }
            else
            {
                Console.WriteLine("Ошибка! Игрок не был добавлен...");
            }
        }

        private void RemovePlayerByUserInput()
        {
            if( TryReadPlayerIdByUserInput(out int id) && (_collector.RemoveByID(id) == true) )
            {
                Console.WriteLine("Игрок под id " + id + " был удален!");
            }
            else
            {
                Console.WriteLine("Ошибка! Игрок не был удален...");
            }

        }

        private void SetPlayerBanModeByUserInput(bool isBanned)
        {
            if ( TryReadPlayerIdByUserInput(out int id) )
            {
                _collector.SetPlayerBanMode(id, isBanned);

                Console.WriteLine("Режим бана игрока под id " + id + ": " + isBanned.ToString());
            }
        }
    }

    sealed class Player : CollectedEntity
    {
        public int Level { get; private set; }
        public string Name { get; private set; }
        public bool IsBanned { get; private set; } = false;

        public Player(string name = "Unnamed", int level = 1)
        {
            Name = name;
            Level = level;
        }

        public void SetBanMode(bool isBanned) => IsBanned = isBanned;
    }

    sealed class PlayerCollector : EntityCollector
    {
        public Player GetPlayerByID(int id) => GetEntityByID(id) as Player;

        public void SetPlayerBanMode(int id, bool isBanned) => GetPlayerByID(id)?.SetBanMode(isBanned);
    }

    class EntityCollector : ICollectorable
    {
        protected List<CollectedEntity> _entityList = new List<CollectedEntity>();
        private int _nextID = 0;

        public bool Add(CollectedEntity entity)
        {
            if (entity != null && entity.ID == ICollectorable.EmptyID)
            {
                entity.ID = _nextID;
                _nextID++;
                _entityList.Add(entity);

                return true;
            }

            return false;
        }

        public bool RemoveByID(int id)
        {
            CollectedEntity entity = GetEntityByID(id);

            if (entity != null)
            {
                _entityList.Remove(entity);

                return true;
            }

            return false;
        }

        public CollectedEntity GetEntityByID(int id)
        {
            foreach (CollectedEntity entity in _entityList)
            {
                if (entity.ID == id)
                {
                    return entity;
                }
            }

            return null;
        }

        public void ShowEntities()
        {
            foreach (CollectedEntity entity in _entityList)
            {
                Console.WriteLine("Объект типа " + entity.ToString() + " с ид " + entity.ID);
            }
        }
    }

    abstract class CollectedEntity
    {
        private int _id = ICollectorable.EmptyID;

        public int ID
        {
            get => _id;

            set
            {
                if (_id == ICollectorable.EmptyID)
                {
                    _id = value;
                }
            }
        }
    }

    interface ICollectorable
    {
        public static int EmptyID = -1;

        public bool Add(CollectedEntity entity);

        public bool RemoveByID(int id);

        public CollectedEntity GetEntityByID(int id);

        public void ShowEntities();
    }
}
