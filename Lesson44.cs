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
        private const string WordToCreateCarriages = "создать_вагоны";
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

                    case WordToCreateCarriages:
                        CreateCarriageByUser();
                        break;

                    case WordToSellTicket:
                        SellTicketsByUser();
                        break;

                    case WordToDepart:
                        DepartDirectionByUser();
                        break;
                }
            }
        }

        private void CreateDirectionPathByUser()
        {
            DirectionPath direction;

            if(TryReadDirectionPathByUser(out string arrivalName, out string departName) == true)
            {
                direction = new DirectionPath(departName, arrivalName);

                _directionPathHandler.Add(direction);

                Console.WriteLine(direction.ToString() + " создано!");
            }
        }

        private bool TryReadDirectionPathByUser(out string arrivalPoint, out string departurePoint)
        {
            Console.Write("Введите точку прибытия:");
            arrivalPoint = Console.ReadLine();

            Console.Write("Введите точку посадки:");
            departurePoint = Console.ReadLine();

            if (arrivalPoint == "" || departurePoint == "")
            {
                Console.WriteLine("Ошибка ввода");

                return false;
            }

            return true;
        }

        private DirectionPath GetDirectionPathByReadLine()
        {
            if (TryReadDirectionPathByUser(out string arrivalName, out string departName) == true)
            {
                return _directionPathHandler.FindDirectionPathByPoints(arrivalName, departName);
            }

            return null;
        }

        private void CreateTrainByUser()
        {
            string trainNumber;
            DirectionPath direction;
            Train train;

            Console.WriteLine("Организация поезда");
            Console.WriteLine("Введите данные направления, к которому будет присоединен поезд");

            direction = GetDirectionPathByReadLine();

            if (direction != null)
            {
                Console.Write("\nВведите номер поезда:");

                trainNumber = Console.ReadLine();

                if (trainNumber != "")
                {
                    train = new Train(trainNumber);

                    if (direction.TryJoinTrain(train) == true)
                    {
                        Console.WriteLine(train.ToString() + " создан и прикреплен к направлению!");
                    }
                }

                return;
            }

            Console.WriteLine("Произошла ошибка при создании поезда...");
        }

        private void CreateCarriageByUser()
        {
            DirectionPath direction;

            Console.WriteLine("Присоединение вагона к поезду");
            Console.WriteLine("Осуществите поиск направления:");

            direction = GetDirectionPathByReadLine();

            if (direction != null && direction.HasTrain() == true && direction.IsTrainDeparted() == false)
            {
                Console.WriteLine(direction.ToAdvancedString());
                Console.Write("Введите размер вагона(короткий,средний, длинный):");

                switch (Console.ReadLine())
                {
                    case "короткий":
                        direction.JoinCarriageToTrain(new Carriage(СarriageСapacityTypes.Short));
                        break;

                    case "средний":
                        direction.JoinCarriageToTrain(new Carriage(СarriageСapacityTypes.Medium));
                        break;

                    case "длинный":
                        direction.JoinCarriageToTrain(new Carriage(СarriageСapacityTypes.Long));
                        break;
                }

                return;
            }

            Console.WriteLine("Ошибка в создании вагона...");
        }

        private void SellTicketsByUser()
        {
            DirectionPath direction;

            Console.WriteLine("\nПродажа билетов\nОсуществите поиск направления:");

            direction = GetDirectionPathByReadLine();

            if (direction != null)
            {
                direction.ShowWaitingList();

                Console.Write("\nВведите номер пассажира для резерва пассажирского места:");

                if ((int.TryParse(Console.ReadLine(), out int index) == true) && (direction.TryAddPassengerFromWaitingList(index) == true))
                {
                    Console.WriteLine("Билет продан!");
                }

                return;
            }

            Console.WriteLine("Ошибка продажи билета...");
        }

        private void DepartDirectionByUser()
        {
            DirectionPath direction;

            Console.WriteLine("Присоединение вагона к поезду");
            Console.WriteLine("Осуществите поиск направления:");

            direction = GetDirectionPathByReadLine();

            if(direction.HasTrain() == true && direction.IsTrainDeparted() == true)
            {
                direction.DepartTrain();
            }
            else
            {
                Console.WriteLine("У данного направления нет поезда или он отправлен!");
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\n---Меню управление отправкой поездов дальнего следования---");
            Console.WriteLine("Введите <" + WordToExit + "> для выхода из программы");
            Console.WriteLine("Введите <" + WordToClear + "> для очистки консоли");
            Console.WriteLine("Введите <" + WordToCreateDirection + "> чтобы создать направление следования");
            Console.WriteLine("Введите <" + WordToSellTicket + "> чтобы продать билеты");
            Console.WriteLine("Введите <" + WordToCreateTrain + "> чтобы создать поезд");
            Console.WriteLine("Введите <" + WordToCreateCarriages + "> чтобы прицепить вагон");
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

            for (int i = 0; i < _directions.Count; i++)
            {
                Console.Write("     [" + i + "] " + _directions[i].ToAdvancedString());
            }
        }

        public DirectionPath FindDirectionPathByPoints(string arrivalPoint,string departurePoint)
        {
            return _directions.Find(direction => direction.ArrivalPoint.Equals(arrivalPoint) && direction.DeparturePoint.Equals(departurePoint));
        }

    }

    class DirectionPath
    {
        private List<PassengerPlace> _waitingPassengerList = new List<PassengerPlace>();
        private Train _currentTrain;

        public string DeparturePoint { get; private set; }
        public string ArrivalPoint { get; private set; }
        public int WaitingPassengerCount
        {
            get
            {
                return _waitingPassengerList.Count;
            }
        }

        public DirectionPath(string departurePoint, string arrivalPoint)
        {
            DeparturePoint = departurePoint;
            ArrivalPoint = arrivalPoint;

            _waitingPassengerList.AddRange(PassengerRandomizer.CreatePassengerArray());
        }

        public bool TryJoinTrain(Train train)
        {
            if (train == null || (_currentTrain != null && _currentTrain.IsDeparted == true) )
            {
                return false;
            }

            _currentTrain = train;

            return true;
        }

        public bool TryAddPassengerFromWaitingList(int index)
        {
            if (_currentTrain == null)
            {
                return false;
            }

            if ( (index >= 0 && index < WaitingPassengerCount) && _currentTrain.TryAddPassenger(_waitingPassengerList[index]) == true )
            {
                _waitingPassengerList.RemoveAt(index);

                return true;
            }

            return false;
        }

        public void ShowWaitingList()
        {
            Console.WriteLine("Список пассажиров ждущих поезда:");

            for(int i = 0; i < WaitingPassengerCount; i++)
            {
                Console.WriteLine("[" + i + "] " + _waitingPassengerList[i].ToString());
            }

            Console.WriteLine();
        }

        public override string ToString()
        {
            return "Направление " + DeparturePoint + " - " + ArrivalPoint;
        }

        public string ToAdvancedString()
        {
            return (_currentTrain != null) ? (ToString() + ":" + _currentTrain?.ToAdvancedString() + ", ждущие пассажиры:" + WaitingPassengerCount) : ToString();
        }

        public bool HasTrain() => (_currentTrain != null);

        public bool IsTrainDeparted() => (_currentTrain != null) ? _currentTrain.IsDeparted : false;

        public void JoinCarriageToTrain(Carriage carriage) => _currentTrain?.JoinCarriage(carriage);

        public void DepartTrain() => _currentTrain?.Depart();
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

        public Train(string number) => Number = number;

        public void JoinCarriage(Carriage carriage)
        {
            if (IsDeparted == true)
            {
                Console.WriteLine("Поезд отправлен, сцепка вагона запрещена!");

                return;
            }

            _carriages.Add(carriage);

            Console.WriteLine("Вагон сцеплен!");
        }

        public void ShowFullPassengerList()
        {
            Console.WriteLine("Список пассажиров поезда " + Number);

            foreach (Carriage car in _carriages)
            {
                car.ShowPassengerList();
            }
        }

        public void ShowCarriageInfoByIndex(int index)
        {
            if (index >= 0 && index < _carriages.Count)
            {
                _carriages[index].ShowPassengerList();
            }
        }

        public int GetPlacesCount()
        {
            int count = 0;

            foreach (Carriage car in _carriages)
            {
                count += car.GetPlaceCount();
            }

            return count;
        }

        public int GetOccupiedPlaces() => (GetPlacesCount() - GetFreePlaces());

        public int GetFreePlaces()
        {
            int count = 0;

            foreach (Carriage car in _carriages)
            {
                count += car.GetFreePlaceCount();
            }

            return count;
        }

        public override string ToString()
        {
            return "Поезд #" + Number;
        }

        public string ToAdvancedString()
        {
            return ToString() + " [Всего мест:" + GetPlacesCount() + ", занятых:" + GetOccupiedPlaces() + ", свободных:" + GetFreePlaces() + "]";
        }

        public bool TryAddPassenger(PassengerPlace place)
        {
            if (IsDeparted == true)
            {
                return false;
            }

            foreach (Carriage car in _carriages)
            {
                if (car.TryAddPassenger(place) == true)
                {
                    return true;
                }
            }

            return false;
        }

        public void Depart() => IsDeparted = true;
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

        private PassengerPlace[] _passengerPlaces;

        public Carriage(СarriageСapacityTypes type)
        {
            _passengerPlaces = new PassengerPlace[(int)type];

            ClearPassengerPlaces();
        }

        public void ShowPassengerList()
        {
            Console.WriteLine("Список пассажиров вагона:");

            for (int i = 0; i < _passengerPlaces.Length; i++)
            {
                PassengerPlace place = _passengerPlaces[i];
                if (place.IsFree() == false)
                {
                    Console.WriteLine("Место #" + i + " занято " + _passengerPlaces[i].ToString());
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
            for (int i = 0; i < _passengerPlaces.Length; i++)
            {
                _passengerPlaces[i].Clear();
            }
        }

        public int GetPlaceCount() => _passengerPlaces.Length;

        public int GetOccupiedPlaceCount() => (GetPlaceCount() - GetFreePlaceCount());

        public int GetFreePlaceCount()
        {
            int count = 0;

            for (int i = 0; i < _passengerPlaces.Length; i++)
            {
                if (_passengerPlaces[i].IsFree() == true)
                {
                    count++;
                }
            }

            return count;
        }

        public bool TryAddPassenger(PassengerPlace place)
        {
            for (int i = 0; i < _passengerPlaces.Length; i++)
            {
                if (_passengerPlaces[i].IsFree() == true)
                {
                    _passengerPlaces[i] = place;

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
                _passengerPlaces[index].Clear();

                return true;
            }

            return false;
        }

        private int FindIndexByPassengerPlace(PassengerPlace place)
        {
            for (int i = 0; i < _passengerPlaces.Length; i++)
            {
                if (_passengerPlaces[i].Equals(place) == true)
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

        public PassengerPlace(string fullname, string passport)
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

        public override string ToString()
        {
            return FullName + "  " + Passport;
        }
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
            int randomCount = new Random().Next(MinRandomBound, MaxRandomBound);

            PassengerPlace[] passengerPlaces = new PassengerPlace[randomCount];

            for (int i = 0; i < randomCount; i++)
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
            Random random = new Random();

            return random.Next(1000, 10000).ToString() + " " + random.Next(100000, 1000000).ToString();
        }
    }
}
