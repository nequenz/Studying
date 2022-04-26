using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectionPathOrganizer directionPathHandler = new DirectionPathOrganizer(3);
            UserInputMenu menu = new UserInputMenu(directionPathHandler);

            menu.Update();
        }
    }

    class UserInputMenu
    {
        private const string WordToExit = "выход";
        private const string WordToCreateDirection = "создать_направление";
        private const string WordToSellTicket = "продать_билеты";
        private const string WordToCreateTrain = "создать_поезд";
        private const string WordToDepart = "отправить_поезд";
        private const string WordToClear = "очистить";

        private string _wordToRead;
        private DirectionPathOrganizer _directionPathHandler;

        public UserInputMenu(DirectionPathOrganizer directionPathHandler)
        {
            _directionPathHandler = directionPathHandler;
        }

        public void Update()
        {
            while (_directionPathHandler != null && _wordToRead != WordToExit)
            {
                ShowMenu();

                Console.Write("\nВвод:");

                _wordToRead = Console.ReadLine();

                Console.WriteLine();

                switch (_wordToRead)
                {
                    case WordToExit:
                        break;

                    case WordToClear:
                        Console.Clear();
                        break;

                    case WordToCreateDirection:
                        CreateDirectionPathByUser();
                        break;

                    case WordToCreateTrain:
                        CreateTrainByUser();
                        break;
                }
            }
        }

        private void CreateDirectionPathByUser()
        {
            string departName;
            string arrivalName;
            DirectionPath direction;

            Console.WriteLine("Введите точку отправки и точку прибытия");

            Console.Write("Место посадки:");
            departName = Console.ReadLine();

            Console.Write("Место прибытия:");
            arrivalName = Console.ReadLine();

            if(departName == "" || arrivalName == "")
            {
                Console.WriteLine("Ошибка ввода");
            }

            direction = new DirectionPath(departName, arrivalName);

            _directionPathHandler.Add(direction);

            Console.WriteLine(direction.ToString()+" создано!");
        }

        private void CreateTrainByUser()
        {
            int directionIndex = 0;
            string trainNumber;
            Train train;

            Console.WriteLine("Организация поезда");
            Console.Write("Введите номер направления, к которому будет присоединен поезд:");

            if (int.TryParse( Console.ReadLine(),out directionIndex) == true && (directionIndex>=0 && directionIndex<_directionPathHandler.CurrentDirectionPathCount))
            {
                Console.Write("\nВведите номер поезда:");
                trainNumber = Console.ReadLine();

                if(trainNumber != "")
                {
                    train = new Train(trainNumber);
                    _directionPathHandler.TryJoinTrainToByIndex(directionIndex, train);

                    Console.WriteLine(train.ToString()+" создан и прикреплен к направлению!");
                }

                return;
            }

            Console.WriteLine("Произошла ошибка при создании поезда...");
        }

        private void CreateCarriageByUser()
        {

        }

        private void SellTicketsByUser()
        {
            Console.WriteLine("");

        }

        private void ShowMenu()
        {
            Console.WriteLine("\n---Меню управление отправкой поездов дальнего следования---");
            Console.WriteLine("Введите <" + WordToExit + "> для выхода из программы");
            Console.WriteLine("Введите <" + WordToClear + "> для очистки консоли");
            Console.WriteLine("Введите <" + WordToCreateDirection + "> чтобы создать направление следования");
            Console.WriteLine("Введите <" + WordToSellTicket + "> чтобы продать билеты");
            Console.WriteLine("Введите <" + WordToCreateTrain + "> чтобы создать поезд");
            Console.WriteLine("Введите <" + WordToDepart + "> чтобы отправить поезд");

            Console.WriteLine();

            _directionPathHandler.ShowDirections();
        }
    }

    class DirectionPathOrganizer
    {
        private List<DirectionPath> _directions = new List<DirectionPath>();

        public readonly int maxDirectionsCount;

        public int CurrentDirectionPathCount
        {
            get
            {
                return _directions.Count;
            }
        }

        public DirectionPathOrganizer(int maxCount)
        {
            maxDirectionsCount = maxCount;
        }

        public void Add(DirectionPath direction)
        {
            if (_directions.Count < maxDirectionsCount)
            {
                _directions.Add(direction);
            }
        }

        public void ShowDirections()
        {
            Console.WriteLine("---Организованные направления---");

            for(int i = 0; i < _directions.Count; i++)
            {
                Console.Write("     ["+i+"] "+_directions[i].ToAdvancedString());
            }
        }

        public bool TryJoinTrainToByIndex(int index,Train train)
        {
            if(index>=0 && index < _directions.Count && _directions[index] != null)
            {
                return _directions[index].TryJoinTrain(train) == true;
            }

            return false;
        }
    }

    class DirectionPath
    {
        private Train _currentTrain;

        public string DeparturePoint { get; private set; }
        public string ArrivalPoint { get; private set; }
        public List<PassengerPlace> WaitingPassengerList { get; private set; } =  new List<PassengerPlace>();
        public int WaitingPassengerCount
        {
            get
            {
                return WaitingPassengerList.Count;
            }
        }

        public DirectionPath(string departurePoint,string arrivalPoint)
        {
            DeparturePoint = departurePoint;
            ArrivalPoint = arrivalPoint;

            WaitingPassengerList.AddRange( PassengerRandomizer.CreatePassengerArray() );
        }

        public void ShowTrainInfo()
        {
            ToAdvancedString();

            for(int i = 0; i < _currentTrain.CarriageCount; i++)
            {
                _currentTrain.ShowCarriageInfoByIndex(i);
            }
        }

        public bool TryJoinTrain(Train train)
        {
            if ((_currentTrain != null && _currentTrain.IsDeparted == true) || train == null)
            {
                return false;
            }

            _currentTrain = train;

            return true;
        }

        public override string ToString()
        {
            return "Направление " + DeparturePoint + " - " + ArrivalPoint;
        }

        public string ToAdvancedString() => (ToString() + ":" + _currentTrain?.ToAdvancedString()+", ждущие пассажиры:"+ WaitingPassengerCount);
    }

    class Train
    {
        private List<Carriage> _carriages = new List<Carriage>();

        public int CarriageCount
        {
            get
            {
                return _carriages.Count;
            }
        }
        public string Number { get; private set; }
        public bool IsDeparted { get; private set; } = false;

        public Train(string number)
        {
            Number = number;
        }

        public void Depart()
        {
            IsDeparted = true;
        }

        public void JoinCarriage(Carriage carriage)
        {
            _carriages.Add(carriage);

            Console.WriteLine("Вагон сцеплен!");
        }

        public void ShowFullPassengerList()
        {
            Console.WriteLine("Список пассажиров поезда "+Number);

            foreach(Carriage car in _carriages)
            {
                car.ShowPassengerList();
            }
        }

        public void ShowCarriageInfoByIndex(int index)
        {
            if(index>=0 && index < _carriages.Count)
            {
                _carriages[index].ShowPassengerList();
            }
        }

        public int GetPlacesCount()
        {
            int count = 0;

            foreach(Carriage car in _carriages)
            {
                count += car.GetPlaceCount();
            }

            return count;
        }

        public int GetFreePlaces()
        {
            int count = 0;

            foreach (Carriage car in _carriages)
            {
                count += car.GetFreePlaceCount();
            }

            return count;
        }

        public int GetOccupiedPlaces() => (GetPlacesCount() - GetFreePlaces());

        public override string ToString()
        {
            return "Поезд #" + Number;
        }

        public string ToAdvancedString()
        {
            return ToString() + " [Всего мест:" + GetPlacesCount() + ", занятых:" + GetOccupiedPlaces() + ", свободных:" + GetFreePlaces()+"]";
        }
    }

    enum СarriageСapacityTypes
    {
        Short = 16,
        Medium = 32,
        Long = 64,
    }

    class Carriage
    {
        private const int BadIndex = -1;

        public PassengerPlace[] PassengerPlaces { get; private set; }
        
        public Carriage(СarriageСapacityTypes type)
        {
            PassengerPlaces = new PassengerPlace[(int)type];
        }

        public void ShowPassengerList()
        {
            Console.WriteLine("Список пассажиров вагона:");

            for(int i = 0; i < PassengerPlaces.Length; i++)
            {
                PassengerPlace place = PassengerPlaces[i];
                if(place.IsFree() == false)
                {
                    Console.WriteLine("Место #" + i + " занято " + PassengerPlaces[i].FullName + "  " + PassengerPlaces[i].Passport);
                }
                else
                {
                    Console.WriteLine("Место #" + i + " свободно");
                }
            }

            Console.WriteLine();
        }

        public void ClearPassengerPlaces()
        {
            for (int i = 0; i < PassengerPlaces.Length; i++)
            {
                PassengerPlaces[i].Clear();
            }
        }

        public int GetPlaceCount() => PassengerPlaces.Length;

        public int GetFreePlaceCount()
        {
            int count = 0;

            for (int i = 0; i < PassengerPlaces.Length; i++)
            {
                if (PassengerPlaces[i].IsFree() == true)
                {
                    count++;
                }
            }

            return count;
        }

        public int GetOccupiedPlacesCount() => (GetPlaceCount() - GetFreePlaceCount());

        public bool TryAddPassenger(PassengerPlace place)
        {
            for (int i = 0; i < PassengerPlaces.Length; i++)
            {
                if (PassengerPlaces[i].IsFree() == true)
                {
                    PassengerPlaces[i] = place;

                    return true;
                }
            }

            return false;
        }

        public bool TryRemovePassenger(PassengerPlace place)
        {
            int index = FindIndexByPassengerPlace(place);

            if (index != BadIndex)
            {
                PassengerPlaces[index].Clear();

                return true;
            }

            return false;
        }

        private int FindIndexByPassengerPlace(PassengerPlace place)
        {
            for (int i = 0; i < PassengerPlaces.Length; i++)
            {
                if (PassengerPlaces[i].Equals(place) == true)
                {
                    return i;
                }
            }

            return BadIndex;
        }

    }

    struct PassengerPlace
    {
        public string FullName { get; private set; }
        public string Passport { get; private set; }

        public PassengerPlace(string fullname,string passport)
        {
            FullName = fullname;
            Passport = passport;
        }

        public void SetFullName(string name) => FullName = name;

        public void SetPassport(string passport) => Passport = passport;

        public void Clear()
        {
            FullName = "";
            Passport = "";
        }

        public bool IsFree() => (FullName == "" && Passport == "");

    }

    static class PassengerRandomizer
    {
        public const int MinRandomBound = 10;
        public const int MaxRandomBound = 256;

        private static string[] _names = {
            "Иван",
            "Анатолий",
            "Сергей",
            "Александр",
            "Наталья",
            "Александра",
            "Яна",
            "Алена"
        };
        private static string[] _surnames = {
            "Прививко",
            "Копилко",
            "Монетко",
            "Буренко",
            "Бобовко",
            "Чубайко",
            "Креветко",
            "Деревко"
        };

        public static PassengerPlace[] CreatePassengerArray()
        {
            int randomCount = new Random().Next(MinRandomBound,MaxRandomBound);

            PassengerPlace[] passengerPlaces = new PassengerPlace[randomCount];

            for(int i=0;i< randomCount; i++)
            {
                passengerPlaces[i] = CreatePassenger();
            }

            return passengerPlaces;
        }

        public static PassengerPlace CreatePassenger() => new PassengerPlace(CreateFullname(), CreatePassport());

        private static string CreateFullname()
        {
            Random random = new Random();

            return _names[random.Next(0, _names.Length)] + " " + _surnames[random.Next(0, _surnames.Length)]; 
        }

        private static string CreatePassport()
        {
            Random  random = new Random();

            return random.Next(1000, 10000).ToString()+" "+random.Next(100000,1000000).ToString();
        }
    }
}
