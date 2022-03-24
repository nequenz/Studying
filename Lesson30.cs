using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            int convertedValue = 0;
            
            ConvertToInt(out convertedValue);
        }

        private static void ConvertToInt(out int outValue)
        {
            Console.Write("Ввод:");

            while( int.TryParse( Console.ReadLine(), out outValue) == false )
            {
                Console.Write("Значение не конвертировано, попробуйте еще раз\nВвод:");
            }
        }
    }
}
