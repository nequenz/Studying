using System;
using System.Collections.Generic;
using System.Linq;

namespace Tired
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Stew[] stews =
            {
                new Stew("Московская тушенка",new Date(1940),new Date(1987)),
                new Stew("Питерская тушенка",new Date(1978),new Date(2003)),
                new Stew("Омская тушенка",new Date(1911),new Date(1962)),
                new Stew("Карельская тушенка",new Date(2000),new Date(2020)),
                new Stew("Белорусская тушенка",new Date(1990),new Date(2025)),
                new Stew("Инопланетная тушенка",new Date(350),new Date(2066)),
            };

            PrintListOfBestBeforeDate(stews);
        }

        private static void PrintListOfBestBeforeDate(IEnumerable<Stew> stewList)
        {
            var bestBeforeDateList = from Stew stew in stewList
                                     where stew.IsDateOut(DateTime.Now) == true
                                     select stew;

            Console.WriteLine("Список просроченной тушенки:");

            foreach (Stew stew in bestBeforeDateList)
            {
                Console.WriteLine(stew.GetInfo());
            }
        }
    }

    public class Stew
    {
        private string _name;
        private Date _productionDate;
        private Date _bestBeforeDate;

        public Date ProductionDate
        {
            get
            {
                return _productionDate;
            }
        }
        public Date BestBeforeDate
        {
            get
            {
                return _bestBeforeDate;
            }
        }
        public string Name
        {
            get => _name;
        }

        public Stew(string name, Date productonDate, Date bastBeforeDate)
        {
            _name = name;
            _productionDate = productonDate;
            _bestBeforeDate = bastBeforeDate;
        }

        public bool IsDateOut(Date date) => (_bestBeforeDate.GetDate()) < date.GetDate();

        public bool IsDateOut(DateTime dateTime) => (_bestBeforeDate.GetDate()) < dateTime.Year;

        public string GetInfo() => "Название:" + _name + ", год производства:" + _productionDate.GetDate() + ", срок годности до:" + _bestBeforeDate.GetDate();

    }

    public struct Date
    {
        private int _date;

        public Date(int date)
        {
            _date = date;
        }

        public void SetDate(int date) => _date = date;

        public int GetDate() => _date;
    }

    public static class StudyHelper
    {
        public static int GetRandomValue(int minValue = 0, int maxValue = 1) => new Random().Next(minValue, maxValue);

    }
}
