using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            int convertedValue = GetIntValueByReadline();

            Console.WriteLine("Значение:" + convertedValue);
        }

        private static int GetIntValueByReadline()
        {
            int result = 0;

            Console.Write("Ввод:");

            while (int.TryParse(Console.ReadLine(), out result) == false)
            {
                Console.Write("Значение не конвертировано, попробуйте еще раз\nВвод:");
            }

            return result;
        }
    }
}
