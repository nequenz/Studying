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
        public PassengerPlace[] PassengerPlaces { get; private set; }

        public Сarriage(int placeCount)
        {
            PassengerPlaces = new PassengerPlace[placeCount];
        }

        public void ClearPassengerPlaces()
        {
            for(int i = 0; i < PassengerPlaces.Length; i++)
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
                if( PassengerPlaces[i].IsFree() == true)
                {
                    count++;
                }
            }

            return count;
        }

        public int GetOccupiedPlacesCount() => ( GetPlaceCount() - GetFreePlaceCount() );

        public bool TryAddPassenger()
        {

        }
    }

    struct PassengerPlace
    {
        public string FullName { get; private set; }

        public void SetFullName(string name) => FullName = name;

        public void Clear() => FullName = "";

        public bool IsFree() => (FullName == "");
    }
}
