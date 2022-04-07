using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            int myAccount = 0;
            Queue<int> clientPurchaseAmounts = new Queue<int>();
  
            AddRandomPurchaseAmounts(clientPurchaseAmounts,15);

            while(clientPurchaseAmounts.Count > 0)
            {
                WriteStatusOfQueue(clientPurchaseAmounts);

                int currentPurchase = clientPurchaseAmounts.Dequeue();

                Console.WriteLine("\nНа вашем счету:" + myAccount + " Р");
                Console.WriteLine("\nОбслуживание клиента с суммой покупки " + currentPurchase + " Р");
                Console.WriteLine("Нажмите любую клавишу для обслуживания...");
                Console.ReadKey(true);

                myAccount += currentPurchase;

                Console.Clear();
            }
        }

        static private void AddRandomPurchaseAmounts(Queue<int> queue,int Count)
        {
            int minRandomBound = 1000;
            int maxRandomBound = 10500;

            for (int i = 0; i < Count; i++)
            {
                queue.Enqueue( new Random().Next(minRandomBound,maxRandomBound) );
            }
        }

        static private void WriteStatusOfQueue(Queue<int> queue)
        {
            Console.WriteLine("В очереди на данный момент "+queue.Count+" клиентов");

            foreach (int purchaseAmount in queue)
            {
                Console.WriteLine("Сумма покупки клиента "+purchaseAmount+" $");
            }
        }
    }
}
