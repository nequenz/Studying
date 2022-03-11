using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            const string Password = "jojomojo";
            const string SecretMessage = "Hash-functions are crying nearby:'(";
            const int MaxTryCount = 3;

            int currentTry = 0;
            string passwordToEnter = "";

            while(currentTry < MaxTryCount)
            {
                Console.Write("Введите пароль:");
                passwordToEnter = Console.ReadLine();

                if( passwordToEnter == Password)
                {
                    Console.WriteLine(SecretMessage);
                    break;
                }

                currentTry++;
                Console.WriteLine("У вас осталось "+(MaxTryCount - currentTry)+" попыток");
            }
        }
    }
}
