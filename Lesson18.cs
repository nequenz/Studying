using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            string scopesLine = "";
            const char OpenedScope = '(';
            const char ClosedScope = ')';
            int scopeValidCount = 0;
            int maxScopeDeepLevel = 0;

            Console.Write("Введите скобочное выражение:");
            scopesLine = Console.ReadLine();

            foreach (char currentChar in scopesLine)
            {
                if (currentChar == OpenedScope)
                {
                    scopeValidCount++;

                    bool isValidCountHigherDeepLevel = (scopeValidCount > maxScopeDeepLevel);

                    if (isValidCountHigherDeepLevel == true)
                    {
                        maxScopeDeepLevel = scopeValidCount;
                    }
                }

                if(currentChar == ClosedScope)
                {
                    scopeValidCount--;
                }

                if (scopeValidCount < 0)
                {
                    break;
                }
            }

            if (scopeValidCount == 0)
            {
                Console.WriteLine("Итог:Выражение корректно, максимальный уровень вложенности:" + maxScopeDeepLevel);
            }
            else
            {
                Console.WriteLine("Итог:Выражение некорректно");
            }

            Console.ReadKey();
        }
    }
}
