using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            string wordToRead = "";

            const string WordToExit = "exit";
            const string WordToGetRandomValue = "get_random_value";
            const string WordToSumPairs = "sum_values";
            const string WordToGetCurrentDate = "get_date";
            const string WordToSearchChar = "search_char";
            const string WordToPlayLine = "play_line";
            const int MinRandomBound = 0;
            const int MaxRandomBound = 1001;
            const int PlayLineLength = 15;
            const int PlayLineSpeed = 200;

            Console.WriteLine("Введите команду для выполнения\n(Команды для выполнения)");
            Console.WriteLine("exit - выход");
            Console.WriteLine("get_random_value - вывести случайное значение от "+ MinRandomBound +" до " + MaxRandomBound);
            Console.WriteLine("sum_values - суммировать два значения");
            Console.WriteLine("get_date - вывести текущую дату");
            Console.WriteLine("search_char - найти набранный символ в строке");
            Console.WriteLine("play_line - проиграть волшебную строку");

            while (wordToRead != WordToExit)
            {
                Console.Write("\nВведите команду для выполнения:");
                wordToRead = Console.ReadLine();

                float floatNumberArgument01 = 0.0f;
                float floatNumberArgument02 = 0.0f;
                char charArgument = '\0';
                string stringArgumentArg = "";

                switch (wordToRead)
                {
                    case WordToGetRandomValue:
                        Console.WriteLine("Случайное значение:" + new Random().Next(MinRandomBound,MaxRandomBound));
                        break;

                    case WordToSumPairs:
                        Console.Write("\nВведите первое значение для суммирования:");
                        floatNumberArgument01 = (float)Convert.ToDouble( Console.ReadLine() );

                        Console.Write("\nВведите второе значение для суммирования:");
                        floatNumberArgument02 = (float)Convert.ToDouble( Console.ReadLine() );

                        float sumResult = floatNumberArgument01 + floatNumberArgument02;

                        Console.WriteLine("\nРезультат суммы:" + sumResult );
                        break;

                    case WordToGetCurrentDate:
                        Console.Write("\nТекущая дата:"+DateTime.Now.ToShortDateString());
                        break;

                    case WordToSearchChar:
                        Console.Write("\nВведите символ, который нужно подсчетать в строке, которую вы введите далее\n*если введите строку, автоматически выберется первый символ\nсимвол:");
                        charArgument = Console.ReadLine()[0];

                        Console.Write("\nВведите строку, в которой будет произведен подсчет символа:");
                        stringArgumentArg = Console.ReadLine();

                        int foundCharCount = 0;

                        foreach(char gottenChar in stringArgumentArg)
                        {
                            if (gottenChar == charArgument)
                            {
                                foundCharCount++;
                            }
                        }

                        Console.Write("\nКоличество символов в строке составляет:" + foundCharCount);
                        break;

                    case WordToPlayLine:
                        for(int i = 0; i < PlaiLineLength; i++)
                        {
                            System.Threading.Thread.Sleep(PlayLineSpeed);
                            Console.Write("#");
                        }
                        break;
                }
            }

            Console.WriteLine("Вы покинули меню программы");
        }
    }
}
