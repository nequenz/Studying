using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] line0 = new string[] { "pool", "cool", "null", "crul", "crul" };
            string[] line1 = new string[] { "dool", "null", "prool", "fool" };
            List<string> list = new List<string>();

            ConcateStringsToList(ref list, line0);
            ConcateStringsToList(ref list, line1);

            Console.WriteLine("Результат:");

            foreach(string element in list)
            {
                Console.Write(element+" ");
            }
        }

        private static void ConcateStringsToList(ref List<string> outList,string[] line)
        {
            foreach(string element in line)
            {
                if(outList.Contains(element) == false)
                {
                    outList.Add(element);
                }
            }
        }
    }
}
