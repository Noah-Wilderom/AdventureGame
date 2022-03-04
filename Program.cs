using System;
using AdventureGame;

namespace AdventureGame
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.Title = "AdventureGame";
            if (args.Length <= 0)
            {
                Console.WriteLine("Welkom bij AdventureGame!");
                Console.WriteLine("Voer je naam in om te beginnen...");
                string PlayerName = Console.ReadLine();
                Console.ReadKey();
                Player player = new Player(PlayerName);
                Controller Controller = new Controller();
                Controller.OptionMenu();
            }
            else
            {
                if(Player.ValidatePlayer(args[0]))
                {
                    Player.SavePlayer();
                    Console.WriteLine("Welkom terug bij AdventureGame " + Player.getName(args[0]) + "!");
                    Controller Controller = new Controller();
                    Controller.OptionMenu();
                }
                else
                {
                    Console.WriteLine("Welkom bij AdventureGame");
                    Console.WriteLine("Helaas hebben we je gegevens niet kunnen vinden...");
                }
            }
            
        }
    }
}
