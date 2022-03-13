using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            int currentGameMove = 0;

            const int PlayerMoveMultipleNumber = 2;
            const int MaxPlayerHealth = 150;
            int playerHealth = MaxPlayerHealth;
            int playerAbsorbDamage = 0;
            bool isPlayerDead = false;

            const int BossThinkTime = 500;
            const int MaxBossThinkCount = 5;
            const int MaxBossHealth = 600;
            int bossHealth = MaxBossHealth;
            int bossDamage = 0;
            bool isBossStunned = false;
            bool isBossPolyformed = false;
            bool isBossDead = false;


            const string MagicShotNameToUse = "выстрел";
            const int MaxMagicshotReloadTime = 1;
            const int MaxMagicshotActionTime = 1;
            int magicshotReloadTime = 0;
            int magicshotActionTime = 0;
            int magicshotInstanceDamage = 50;
            string magicshotDescription = MagicShotNameToUse + " - Выстрел, наносящий "+ magicshotInstanceDamage+" ед. урона.";
            
            const string FireballNameToUse = "огненный шар";
            const int MaxFireballReloadTime = 2;
            const int MaxFireballActionTime = 5;
            int fireballReloadTime = 0;
            int fireballActionTime = 0;
            int fireballInstanceDamage = 150;
            int fireballPerMoveDamage = 50;
            string fireballDescription = FireballNameToUse+" - Поджигает врага, нанося " + fireballInstanceDamage + " ед. урона и дополнительно еще "+ fireballPerMoveDamage + " в течении "+ MaxFireballActionTime + " следующих ходов.";

            const string PolyformNameToUse = "полиформ";
            const int MaxPolyformReloadTime = 7;
            const int MaxPolyformActionTime = 3;
            int polyformReloadTime = 0;
            int polyformActionTime = 0;
            string polyformDescription = PolyformNameToUse + " - Превращает врага в безобидное животное на "+ MaxPolyformActionTime + " хода. В течении этих ходов враг не может атаковать. Нанесенный урон сбивает эффект.";

            const string FleshlightNameToUse = "исцеление";
            const int MaxFleshlightReloadTime = 3;
            const int MaxFleshlightActionTime = 1;
            int fleshlightReloadTime = 0;
            int fleshlightActionTime = 0;
            int fleshlightHealth = 200;
            string fleshlightDescription = FleshlightNameToUse+ " - Исцеляет на " + fleshlightHealth + " ед. здоровья.";

            const string KarmaLawNameToUse = "закон кармы";
            const int MaxKarmaLawReloadTime = 8;
            const int MaxKarmaLawActionTime = 2;
            const int MaxKarmaLawStunTime = 1;
            int karmaLawReloadTime = 0;
            int karmaLawActionTime = 0;
            int karmaLawAbsorbDamage = 50;
            string karmaLawDescription = KarmaLawNameToUse + " - Меняет реальность, поглащая " + karmaLawAbsorbDamage + " входящего урон в течении следующих " + MaxKarmaLawActionTime + " ходов, и в последствии оглушая врага на "+ MaxKarmaLawStunTime+" ход.";

            Console.WriteLine("Убейте босса!\nКаждый четный ход - ваш, а нечетный босса.\n");

            while ( (isPlayerDead == false) && (isBossDead == false) )
            {
                Console.WriteLine("\nНомер хода:" + currentGameMove);

                if (bossDamage != 0)
                {
                    playerHealth -= (bossDamage - playerAbsorbDamage);
                    bossDamage = 0;
                }

                if (magicshotActionTime > 0)
                {
                    isBossPolyformed = false;
                    polyformActionTime = 0;
                    bossHealth -= magicshotInstanceDamage;
                    magicshotActionTime--;
                }

                if (fireballActionTime > 0)
                {
                    isBossPolyformed = false;
                    polyformActionTime = 0;

                    if (fireballActionTime == MaxFireballActionTime)
                    {
                        bossHealth -= fireballInstanceDamage;
                    }

                    bossHealth -= fireballPerMoveDamage;
                    fireballActionTime--;
                }

                if (polyformActionTime > 0)
                {
                    isBossPolyformed = true;
                    polyformActionTime--;

                    if (polyformActionTime == 0)
                    {
                        isBossPolyformed = false;
                    }
                }

                if (fleshlightActionTime > 0)
                {
                    playerHealth += fleshlightHealth;

                    if (playerHealth > MaxPlayerHealth)
                    {
                        playerHealth = MaxPlayerHealth;
                    }

                    fleshlightActionTime--;
                }

                if (karmaLawActionTime > 0)
                {
                    playerAbsorbDamage = karmaLawAbsorbDamage;
                    karmaLawActionTime--;

                    if (karmaLawActionTime < MaxKarmaLawStunTime)
                    {
                        playerAbsorbDamage = 0;
                        isBossStunned = true;
                    }

                    if (karmaLawActionTime == 0)
                    {
                        isBossStunned = false;
                        playerAbsorbDamage = 0;
                    }
                }

                if (magicshotReloadTime > 0)
                {
                    magicshotReloadTime--;
                }

                if (fireballReloadTime > 0)
                {
                    fireballReloadTime--;
                }

                if(polyformReloadTime > 0)
                {
                    polyformReloadTime--;
                }

                if(fleshlightReloadTime > 0)
                {
                    fleshlightReloadTime--;
                }

                if(karmaLawReloadTime > 0)
                {
                    karmaLawReloadTime--;
                }

                if (bossHealth <= 0)
                {
                    isBossDead = true;
                }

                if(playerHealth <= 0)
                {
                    isPlayerDead = true;
                }

                bool isSomeoneDead = isPlayerDead | isBossDead;

                if(isSomeoneDead == false)
                {
                    Console.WriteLine("\nВаше здоровье:" + playerHealth + "/" + MaxPlayerHealth + " ед.\nЗдоровье босса:" + bossHealth + "/" + MaxBossHealth + " .ед");

                    bool isYourMove = (currentGameMove % PlayerMoveMultipleNumber) == 0;

                    if (isYourMove == true)
                    {
                        Console.WriteLine("\nВаш ход! *Введите наименование способности для его использования*\n");
                        Console.WriteLine(magicshotDescription + "\n");
                        Console.WriteLine(fireballDescription + "\n");
                        Console.WriteLine(polyformDescription + "\n");
                        Console.WriteLine(fleshlightDescription + "\n");
                        Console.WriteLine(karmaLawDescription + "\n");
                        Console.Write("\nВведите название:");

                        string abilityName = Console.ReadLine();

                        switch (abilityName)
                        {
                            case MagicShotNameToUse:
                                if (magicshotReloadTime <= 0)
                                {
                                    magicshotActionTime = MaxMagicshotActionTime;
                                    magicshotReloadTime = MaxMagicshotReloadTime;
                                }
                                else
                                {
                                    Console.WriteLine("Способность будет доступна через " + magicshotReloadTime + " ход(-а/ов).");
                                    currentGameMove--;
                                }
                                break;

                            case FireballNameToUse:
                                if (fireballReloadTime <= 0)
                                {
                                    fireballActionTime = MaxFireballActionTime;
                                    fireballReloadTime = MaxFireballReloadTime;
                                }
                                else
                                {
                                    Console.WriteLine("Способность будет доступна через " + fireballReloadTime + " ход(-а/ов).");
                                    currentGameMove--;
                                }
                                break;

                            case PolyformNameToUse:
                                if (polyformReloadTime <= 0)
                                {
                                    polyformActionTime = MaxPolyformActionTime;
                                    polyformReloadTime = MaxPolyformReloadTime;
                                }
                                else
                                {
                                    Console.WriteLine("Способность будет доступна через " + polyformReloadTime + " ход(-а/ов).");
                                    currentGameMove--;
                                }
                                break;

                            case FleshlightNameToUse:
                                if (fleshlightReloadTime <= 0)
                                {
                                    fleshlightActionTime = MaxFleshlightActionTime;
                                    fleshlightReloadTime = MaxFleshlightReloadTime;
                                }
                                else
                                {
                                    Console.WriteLine("Способность будет доступна через " + fleshlightReloadTime + " ход(-а/ов).");
                                    currentGameMove--;
                                }
                                break;


                            case KarmaLawNameToUse:
                                if (karmaLawReloadTime <= 0)
                                {
                                    karmaLawReloadTime = MaxKarmaLawReloadTime;
                                    karmaLawActionTime = MaxKarmaLawActionTime + MaxKarmaLawStunTime;
                                }
                                else
                                {
                                    Console.WriteLine("Способность будет доступна через " + karmaLawReloadTime + " ход(-а/ов).");
                                    currentGameMove--;
                                }
                                break;

                            default:
                                Console.WriteLine("\nТакой способности в вашем арсенале нет.\nВведите еще раз\n");
                                currentGameMove--;
                                break;
                        }
                    }
                    else
                    {
                        if (isBossStunned == false && isBossPolyformed == false)
                        {
                            Console.WriteLine("Ход босса\n");

                            for (int i = 0; i < MaxBossThinkCount; i++)
                            {
                                System.Threading.Thread.Sleep(BossThinkTime);
                                Console.Write(".");
                            }

                            bossDamage = new Random().Next(50, 150);

                            Console.WriteLine("\nБосс наносит " + bossDamage + " ед. урона по вам!");
                        }
                        else
                        {
                            Console.WriteLine("Босс пропускает ход!");
                        }
                    }
                }

                currentGameMove++;
            }

            if(isBossDead == true)
            {
                Console.WriteLine("Поздравляю! Вы убили босса");
            }

            if(isPlayerDead == true)
            {
                Console.WriteLine("Вы погибли! Потрачено...");
            }

            if(isPlayerDead == true && isBossDead == true)
            {
                Console.WriteLine("Взаимная смерть...");
            }

            Console.WriteLine("\nИгра окончена!");
        }
    }
}
