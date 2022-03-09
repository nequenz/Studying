using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MinutesPerPerson = 10;
            const int MinutesInHour = 60;
            int oldersCountInQueue = 0;
            int commonTimeToWait = 0;
            
            Console.Write("Введите количество старушек, которые будут в очереди:");
            
            oldersCountInQueue = Convert.ToInt32(Console.ReadLine());
            commonTimeToWait = (minutesPerPerson * oldersCountInQueue);
            int timeToWaitInMinutes = commonTimeToWait % MinutesInHour;
            int timeToWaitInHours = commonTimeToWait / MinutesInHour;
            
            Console.WriteLine("Общее время ожидание составляет " + TimeToWaitInHours+" час(-а/-ов) и "+ TimeToWaitInMinutes+" минут(-ы)!");
            Console.ReadLine();
        }
    }
}
