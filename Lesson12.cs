using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            float userRUBBalance = 56000.0f;
            float userUSDBalance = 678.0f;
            float userEURBalance = 320.0f;

            float rateRUBtoUSD = 115.37f;
            float rateRUBtoEUR = 148.61f;
            float rateUSDtoEUR = 91.0f;

            int numberToRead = -1;

            const int NumberToExit = 0;
            const int RUBtoUSDPairNumber = 1;
            const int RUBtoEURPairNumber = 2;
            const int USDtoRUBPairNumber = 3;
            const int USDtoEURPairNumber = 4;
            const int EURtoUSDPairNumber = 5;
            const int EURtoRUBPairNumber = 6;

            Console.WriteLine($"Текущий курс валюты RUB к USD составляет  {rateRUBtoUSD:F2}");
            Console.WriteLine($"Текущий курс валюты RUB к EUR составляет  {rateRUBtoEUR:F2}");
            Console.WriteLine($"Текущий курс валюты USD к EUR составляет  {rateUSDtoEUR:F2}\n");

            Console.WriteLine($"Текущий RUB баланс составляет:{userRUBBalance:F2}");
            Console.WriteLine($"Текущий USD баланс составляет:{userUSDBalance:F2}");
            Console.WriteLine($"Текущий EUR баланс составляет:{userEURBalance:F2}\n");

            while (numberToRead != NumberToExit)
            {
                float balanceToConvert = 0.0f;

                Console.WriteLine("Выберите пару для конвертации (введите цифру соответствующее паре)");
                Console.WriteLine("0. Выйти");
                Console.WriteLine("1. RUB к USD");
                Console.WriteLine("2. RUB к EUR");
                Console.WriteLine("3. USD к RUB");
                Console.WriteLine("4. USD к EUR");
                Console.WriteLine("5. EUR к USD");
                Console.WriteLine("6. EUR к RUB\n");                

                numberToRead = Convert.ToInt32( Console.ReadLine() );

                bool isPairChosen = (numberToRead >= RUBtoUSDPairNumber) && (numberToRead <= EURtoRUBPairNumber);

                if (isPairChosen == true)
                {
                    Console.WriteLine("Введите сумму для конвертации:");

                    balanceToConvert = (float)Convert.ToDouble( Console.ReadLine() );
                }
                else
                {
                    Console.WriteLine("\nПара не выбрана");
                    continue;
                }

                switch (numberToRead)
                {
                    case RUBtoUSDPairNumber:
                        userRUBBalance -= balanceToConvert;
                        userUSDBalance += (1 / rateRUBtoUSD) * balanceToConvert;
                        break;

                    case RUBtoEURPairNumber:
                        userRUBBalance -= balanceToConvert;
                        userEURBalance += (1 / rateRUBtoEUR) * balanceToConvert;
                        break;

                    case USDtoRUBPairNumber:
                        userUSDBalance -= balanceToConvert;
                        userRUBBalance += rateRUBtoUSD * balanceToConvert;
                        break;

                    case USDtoEURPairNumber:
                        userUSDBalance -= balanceToConvert;
                        userEURBalance += rateUSDtoEUR * balanceToConvert;
                        break;

                    case EURtoUSDPairNumber:
                        userEURBalance -= balanceToConvert;
                        userUSDBalance += (1 / rateUSDtoEUR) * balanceToConvert;
                        break;

                    case EURtoRUBPairNumber:
                        userEURBalance -= balanceToConvert;
                        userRUBBalance += (1 / rateRUBtoEUR) * balanceToConvert;
                        break;
                }

                Console.WriteLine($"\nТекущий RUB баланс составляет:{userRUBBalance:F2}");
                Console.WriteLine($"Текущий USD баланс составляет:{userUSDBalance:F2}");
                Console.WriteLine($"Текущий EUR баланс составляет:{userEURBalance:F2}\n");
            }

            Console.WriteLine("\nСпасибо за использование конвектора валют");

            Console.ReadKey();
        }
    }
}
