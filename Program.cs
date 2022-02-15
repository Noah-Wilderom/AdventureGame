using System;
using AdventureGame;

namespace AdventureGame
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("Welkom bij AdventureGame!");
                Console.WriteLine("Voer je naam in om te beginnen...");
                string PlayerName = Console.ReadLine();
                Player player = new Player(PlayerName);
                Controller Controller = new Controller();
            } else
            {
                if(Player.ValidatePlayer(args[0]))
                {
                    Console.WriteLine("Welkom terug bij AdventureGame " + Player.getName(args[0]) + "!");
                } else
                {
                    Console.WriteLine("Welkom bij AdventureGame");
                    Console.WriteLine("Helaas hebben we je gegevens niet kunnen vinden...");
                }
            }
            
        }
    }
}
