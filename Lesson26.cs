using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "mem love smoke juicy king lion fruit middle";

            string[] splitedText = text.Split(' ');

            foreach(string splitedWord in splitedText)
            {
                Console.WriteLine(splitedWord);
            }
        }
    }
}

