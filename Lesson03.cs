using System;

namespace Tired
{
    class Program
    {

        static void Main(string[] args)
        {

            /*
             *  
             *  Unity Junior Developer - lesson 03
             * 
             */


            const int maxPictureCount = 52;

            const int rowOutputPictureCount = 3;

            int fullRowsCount = maxPictureCount / rowOutputPictureCount;

            int pictureOverCount = maxPictureCount % rowOutputPictureCount;

            Console.WriteLine("Максимальное количество заполненных рядов можно вывести "+ fullRowsCount+"\nВ последнем ряду на " + pictureOverCount+" картинку больше!");

        }
    }
}
