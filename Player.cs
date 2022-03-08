using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading;


namespace AdventureGame
{
    class Player
    {
        public static string PublicPlayerKey { get; private set; }
        public static string Name { get; private set; }
        public static int Level { get; private set; }
        public static int Exp { get; private set; }
        public static int Health { get; private set; }
        //private static Dictionary<string, int> Inventory_old;

        public Player(string PlayerName)
        {
            Player.Name = PlayerName;
            InitializeNewPlayer();
        }

        public void InitializeNewPlayer()
        {
            Items Item = new Items();
            Weapons Weapon = new Weapons();
            Console.Clear();
            Console.Write("Nieuw account aan het maken... ");
            using (var progress = new ProgressBar())
            {
                Player.MakePublicPlayerKey();
                progress.Report((double)0 / 100);
                Thread.Sleep(50);
                Player.InitPlayer();
                progress.Report((double)33 / 100);
                Thread.Sleep(50);
                Item.Add("money", 100);
                Item.Add("bread", 5);
                Item.Add("water", 5);
                Weapon.Add("mes", 1);
                progress.Report((double)66 / 100);
                Thread.Sleep(50);
                Player.SavePlayer();
                progress.Report((double)99 / 100);
                Thread.Sleep(50);
                progress.Report((double)100 / 100);
                Thread.Sleep(1000);
                Console.ReadKey();
                Console.Clear();

            }

            Console.WriteLine("Public key: " + Player.PublicPlayerKey);
        }

        public static void SavePlayer()
        {
            Inventory Inv = new Inventory();
            Inv.LoadInventory();
            Inv.Save();
        }

        public static string getName(string userid)
        {
            if (Player.PublicPlayerKey != null) return Player.Name;
            return "Error: No public key set";
        }

        public static void MakePublicPlayerKey(string[] options = null)
        {
            string Format = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int FormatLength = 12;
            char[] FormatArray = Format.ToCharArray();
            List<char> KeyArray = new List<char>();
            for (int i = 0; i < FormatLength; i++)
            {
                Random round = new();
                int x = round.Next(0, FormatArray.Length);
                KeyArray.Add(Format[x]);
            }

            Player.PublicPlayerKey = string.Join("", KeyArray);
        }

        public static void InitPlayer()
        {
            if (Player.ValidatePlayer()) return;
            // Save de player objects naar de database
            Database conn = new Database();
            conn.connectdb.Open();

            string query = "INSERT INTO players (playerpublickey, username) VALUES (@ppk, @username)";
            using (var cmd = new MySqlCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn.connectdb;

                cmd.Parameters.AddWithValue("@ppk", Player.PublicPlayerKey);
                cmd.Parameters.AddWithValue("@username", Player.Name);

                cmd.ExecuteNonQuery();
                conn.connectdb.Close();
            }

            Inventory.InitNewPlayerInventory();
        }

        public static bool AddItemToInventory(string Key, int Value)
        {
            Items Item = new Items();
            return Item.Add(Key, Value);
        }

        public static bool ValidatePlayer(string ppk = null)
        {
            if (ppk == null) ppk = Player.PublicPlayerKey;
            bool check = false;
            Database conn = new Database();
            conn.connectdb.Open();
            string sql = "SELECT * FROM players WHERE playerpublickey = @playerpublickey";
            MySqlDataReader rd;
            using (var cmd = new MySqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn.connectdb;
                cmd.Parameters.AddWithValue("@playerpublickey", (string)ppk);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    check = true;
                    Player.PublicPlayerKey = rd.GetString("playerpublickey");
                    Player.Name = rd.GetString("username");
                }
            }

            return check;
        }

    }
}
