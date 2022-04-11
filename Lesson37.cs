using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] line0 = new string[] { "pool", "cool", "null", "crul" };
            string[] line1 = new string[] { "dool", "null", "prool", "fool" };

            Console.WriteLine("Результат:");

            foreach(string element in ConcateStrings(line0, line1))
            {
                Console.Write(element+" ");
            }
        }

        private static List<string> ConcateStrings(string[] line0, string[] line1)
        {
            List<string> resultList = new List<string>();

            resultList.AddRange(line0);

            foreach(string stringElement in line1)
            {
                if(resultList.Contains(stringElement) == false)
                {
                    resultList.Add(stringElement);
                }
            }

            return resultList;
        }
    }
}
