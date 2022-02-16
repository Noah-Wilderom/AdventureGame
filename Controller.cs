using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureGame;


namespace AdventureGame
{
    class Controller
    {
        private static int OptionMenuLevel { get; set; }

        public Controller()
        {
            Console.ReadKey();
            Console.Clear();
            
        }

        public static void OptionMenu()
        {
            int Level = 0;
            if (Controller.OptionMenuLevel == 0) Level = 1;
            if (Controller.OptionMenuLevel != 0) Level = Controller.OptionMenuLevel;
            if(Level == 1)
            {
                Console.WriteLine("Keuzemenu - AdventureGame");
                Console.WriteLine("1 = Bekijk profiel");
                Console.WriteLine("2 = Bekijk Wapens");
                Console.WriteLine("dev = Developmode");
                Console.Write(">> ");
                string Keuze = (string) Console.ReadLine();
                if(Keuze == "dev")
                {
                    Weapons.PrintWeaponStats("mes");
                }
            }
        }
    }
}
