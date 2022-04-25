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

    class BigСarriage : Сarriage
    {

        public BigСarriage() : base(96)
        {

        }
    }

    abstract class Сarriage
    {
        private const int BadIndex = -1;
        public PassengerPlace[] PassengerPlaces { get; private set; }

        public Сarriage(int placeCount)
        {
            PassengerPlaces = new PassengerPlace[placeCount];
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
            int index = FindFirstFreePlaceIndex();

            if(index != BadIndex)
            {
                PassengerPlaces[index] = place;
                return true;
            }

            return false;
        }

        public bool TryRemovePassenger(PassengerPlace place)
        {
            int index = FindIndexByPassengerPlace(place);

            if(index != BadIndex)
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
    
        private int FindFirstFreePlaceIndex()
        {
            for (int i = 0; i < PassengerPlaces.Length; i++)
            {
                if (PassengerPlaces[i].IsFree() == true)
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
