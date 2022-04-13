using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayerCollector playerCollector = new PlayerCollector();

            playerCollector.Add(new Player("Lolz",4));
            playerCollector.Add(new Player("DarkStalker98", 1));
            playerCollector.Add(new Player("PinkPopa", 45));
            playerCollector.Add(new Player("superAnton2004", 12));

            playerCollector.SetPlayerBanMode(3, true);
            playerCollector.ShowEntities();

            Console.WriteLine("-----------------------");

            playerCollector.RemoveByID(2);
            playerCollector.ShowEntities();
        }
    }

    sealed class Player : CollectedEntity
    {
        public int Level { get; private set; }
        public string Name { get; private set; }
        public bool IsBanned { get; set; } = false;

        public Player(string name = "Unnamed", int level = 1)
        {
            Name = name;
            Level = level;
        }
    }
        
    sealed class PlayerCollector : EntityCollector
    {
        public Player GetPlayerByID(int id) => GetEntityByID(id) as Player;

        public void SetPlayerBanMode(int id,bool isBanned)
        {
            Player player = GetPlayerByID(id);

            if (player != null)
            {
                player.IsBanned = isBanned;
            }
        }
    }

    abstract class CollectedEntity
    {
        private int _id = EntityCollector.EmptyID;

        public int ID
        {
            get => _id;

            set
            {
                if (_id == EntityCollector.EmptyID)
                {
                    _id = value;
                }
            }
        }
    }

    class EntityCollector
    {
        public static readonly int EmptyID = -1;

        protected List<CollectedEntity> _entityList = new List<CollectedEntity>();
        private int _nextID = 0;
        
        public bool Add(CollectedEntity entity)
        {
            if(entity !=null && entity.ID == EmptyID)
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
            foreach(CollectedEntity entity in _entityList)
            {
                if(entity.ID == id)
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
                Console.WriteLine("Объект типа "+entity.ToString()+" с ид "+entity.ID);
            }
        }
    }
}
