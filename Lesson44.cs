using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class DirectionPath
    {
        public Train currentTrain;
        public string DeparturePoint { get; private set; }
        public string ArrivalPoint { get; private set; }


    }

    class Train
    {
        public List<Carriage> сarriages { get; private set; } = new List<Carriage>();
    }

    enum СarriageСapacityTypes
    {
        Short = 32,
        Medium = 64,
        Long = 128,
    }

    class Carriage
    {
        private const int BadIndex = -1;
        public PassengerPlace[] PassengerPlaces { get; private set; }
        public int TrainNumber { get; private set; } = 0;

        public Carriage(СarriageСapacityTypes type)
        {
            PassengerPlaces = new PassengerPlace[(int)type];
        }

        public void ClearPassengerPlaces()
        {
            for (int i = 0; i < PassengerPlaces.Length; i++)
            {
                PassengerPlaces[i].Clear();
            }
        }

        public void SetNumber(int number) => TrainNumber = number;

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

        public void SetFullName(string name) => FullName = name;

        public void SetPassport(string passport) => Passport = passport;

        public void Clear()
        {
            FullName = "";
            Passport = "";
        }

        public bool IsFree() => (FullName == "" && Passport == "");

    }
}
