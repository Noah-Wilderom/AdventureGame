using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
                Console.WriteLine("2 = Bekijk Items");
                Console.WriteLine("3 = Bekijk Wapens");
                Console.WriteLine("4 = [Dev] Geef item");
                Console.WriteLine("dev = Developmode");
                Console.Write(">> ");
                string Keuze = (string) Console.ReadLine();
                if(Keuze == "dev")
                {
                    Weapons.PrintWeaponStats("mes");
                    Thread.Sleep(5000);
                    Console.Clear();
                    OptionMenu();
                }

                if(Keuze == "2")
                {
                    Items.Print();
                    Thread.Sleep(5000);
                    Console.Clear();
                    OptionMenu();
                }

                if (Keuze == "3")
                {
                    Weapons.Print();
                    Thread.Sleep(5000);
                    Console.Clear();
                    OptionMenu();
                }
                if (Keuze == "4")
                {
                    Items Item = new Items();
                    Inventory Inven = new Inventory();
                    Item.Add("medkit", 1);
                    Inven.Save();
                    Thread.Sleep(5000);
                    Console.Clear();
                    Console.WriteLine("Succes");
                    OptionMenu();
                }
            }
        }
    }
}
