using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int minutesPerPerson = 10;

            const int нour = 60;

            int oldersCountInQueue = 0;

            int commonTimeToWait = 0;

            Console.Write("Введите количество старушек, которые будут в очереди:");

            oldersCountInQueue = Convert.ToInt32(Console.ReadLine());

            if (oldersCountInQueue == 0) return;

            commonTimeToWait = (minutesPerPerson * oldersCountInQueue);

            int TimeToWaitInMinutes = commonTimeToWait % нour;

            int TimeToWaitInHours = commonTimeToWait / нour;

            Console.WriteLine("Общее время ожидание составляет " + TimeToWaitInHours+" час(-а/-ов) и "+ TimeToWaitInMinutes+" минут(-ы)!");

            Console.ReadLine();
        }
    }
}
