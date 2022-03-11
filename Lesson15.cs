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

            string passwordToEnter = "";

            for(int i=0;i< MaxTryCount; i++)
            {
                Console.Write("Введите пароль:");
                passwordToEnter = Console.ReadLine();

                if (passwordToEnter == Password)
                {
                    Console.WriteLine(SecretMessage);
                    break;
                }

                int leftAttempts = MaxTryCount - i;
                Console.WriteLine("У вас осталось " + leftAttempts + " попыток");
            }
        }
    }
}
