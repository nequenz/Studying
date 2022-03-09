using System;

namespace Tired
{
    class Program
    {

        private static int TryToConvert(string value)
        {

            int Result = 0;

            try
            {
                Result = Convert.ToInt32(value);
            }
            catch
            {

                Console.WriteLine("Вы ввели некорректное значение?\nК сожалению задание не может продолжено!:(");

                //без throw

                return 0;

            }

            return Result;
        }

        static void Main(string[] args)
        {

            /*
             *  
             *  Unity Junior Developer - lesson 05
             *  *Старушки это боль
             * 
             */

            const int MinutesPerPerson = 10;

            int oldersCountInQueue = 0;

            int commonTimeToWait = 0;

  


            Console.Write("Введите количество старушек, которые будут в очереди:");

            
            oldersCountInQueue = TryToConvert( Console.ReadLine() );


            if (oldersCountInQueue == 0) return;

            commonTimeToWait = (MinutesPerPerson * oldersCountInQueue);

            int TimeToWaitInMinutes = commonTimeToWait % 60;

            int TimeToWaitInHours = commonTimeToWait / 60;

            Console.WriteLine("Общее время ожидание составляет " + TimeToWaitInHours+" час(-а/-ов) и "+ TimeToWaitInMinutes+" минут(-ы)!");

            Console.ReadLine();

        }
    }
}
