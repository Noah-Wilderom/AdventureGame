using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using AdventureGame;


namespace AdventureGame
{
    class Player
    {
        private static string PublicPlayerKey { get; set; }
        private static string Name { get; set; }
        private static Dictionary<string, int> Inventory;

        public Player(string PlayerName)
        {
            Player.Name = PlayerName;
            InitializeNewPlayer();
        }

        public void InitializeNewPlayer()
        {
            Player.AddToInventory("money", 100);
            Player.MakePublicPlayerKey();
            Player.SavePlayer();
            Console.WriteLine("Public key: " + Player.PublicPlayerKey);
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

        public static void SavePlayer()
        {
            if (Player.ValidatePlayer())
            {
                Player.ValidatePlayerInventory();

            }
            // Save de player objects naar de database
            Database conn = new Database();
            conn.connectdb.Open();
            Console.WriteLine("Connectie Database succesvol!");
                

            string query = "INSERT INTO players (playerpublickey, username) VALUES (@ppk, @username)";
            foreach(KeyValuePair<string, int> entry in Player.Inventory)
            {
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
            }
            
        }

        public static void AddToInventory(string Key, int Value)
        {
            Player.Inventory = new Dictionary<string, int>();
            Player.Inventory.Add(Key, Value);
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
                cmd.Parameters.AddWithValue("@playerpublickey", ppk);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    check = true;
                    Player.PublicPlayerKey = rd.GetString("playerpublickey");
                    Player.Name = rd.GetString("username");
                }
            }

            Player.ValidatePlayerInventory(true);
            return check;
        }

        public static void ValidatePlayerInventory(bool NewPlayer = false)
        {
            // For loop msql player_inventory voor Player.Inventory
            if(NewPlayer)
            {
                // Als de speler nieuw is inventory -> database
            }
            //string query = "INSERT INTO player_inventory (playerpublickey, key, value) VALUES (@ppk, @key, @value)";
            //foreach (KeyValuePair<string, int> entry in Player.Inventory)
            //{
            //    using (var cmd = new MySqlCommand())
            //    {
            //        cmd.CommandText = query;
            //        cmd.CommandType = System.Data.CommandType.Text;
            //        cmd.Connection = conn.connectdb;
            //
            //        cmd.Parameters.Add("@ppk", MySw).Value = Player.PublicPlayerKey;
            //        cmd.Parameters.Add("@key", ).Value = entry.Key;
            //        cmd.Parameters.Add("@value", ).Value = entry.Value;
            //
            //       cmd.ExecuteNonQuery();
            //        conn.connectdb.Close();
            //
            //    }
            //}
        }
    }
}
