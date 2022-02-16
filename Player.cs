using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using AdventureGame;


namespace AdventureGame
{
    class Player
    {
        public static string PublicPlayerKey { get; private set; }
        public static string Name { get; private set; }
        private static SortedList<string, int> Inventory = new SortedList<string, int>();
        private static SortedList<string, int> Weapons = new SortedList<string, int>();
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
            Console.Clear();
            Console.Write("Nieuw account aan het maken... ");
            using (var progress = new ProgressBar())
            {
                progress.Report((double) 0 / 100);
                Thread.Sleep(50);
                Player.Inventory.Add("money", 100);
                progress.Report((double) 33 / 100);
                Thread.Sleep(50);
                Player.MakePublicPlayerKey();
                progress.Report((double) 66 / 100);
                Thread.Sleep(50);
                Player.SavePlayer(true, true);
                progress.Report((double) 99 / 100);
                Thread.Sleep(50);
                progress.Report((double)100 / 100);
                Thread.Sleep(1000);
                Console.ReadKey();
                Console.Clear();

            }
            
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

        public static void SavePlayer(bool FirstSave = false, bool NewPlayer = false)
        {
            if (Player.ValidatePlayer())
            {
                Player.ValidatePlayerInventory();
                return;
            }
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
            if(FirstSave) Player.ValidatePlayerInventory(NewPlayer);

            
        }

        public static void AddToInventory(string Key, int Value, bool ToDB = true)
        {
            Player.Inventory.Add(Key, Value);
            if (!ToDB) return;
            // Save de inventory naar de database
            Database conn = new Database();
            conn.connectdb.Open();

            string query = "INSERT INTO player_inventory (playerpublickey, name, value) VALUES (@ppk, @name, @value)";
            using (var cmd = new MySqlCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn.connectdb;

                cmd.Parameters.AddWithValue("@ppk", Player.PublicPlayerKey);
                cmd.Parameters.AddWithValue("@name", Key);
                cmd.Parameters.AddWithValue("@value", Value);


                cmd.ExecuteNonQuery();
                conn.connectdb.Close();
            }
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
                cmd.Parameters.AddWithValue("@playerpublickey", (string) ppk);
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

        public static void ValidatePlayerInventory(bool NewPlayer = false)
        {
            Database conn = new Database();
            // For loop msql player_inventory voor Player.Inventory
            if(NewPlayer)
            {
                // Als de speler nieuw is inventory -> database
                string query = "INSERT INTO player_inventory (playerpublickey, name, value) VALUES (@ppk, @name, @value)";
                foreach (var entry in Player.Inventory)
                {
                    conn.connectdb.Open();
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Connection = conn.connectdb;
            
                        cmd.Parameters.AddWithValue("@ppk", (string) Player.PublicPlayerKey);
                        cmd.Parameters.AddWithValue("@name", (string) entry.Key);
                        cmd.Parameters.AddWithValue("@value", (int) entry.Value);
                        
            
                        cmd.ExecuteNonQuery();
                        conn.connectdb.Close();
            
                    }
                }
            }
            else
            {
                conn.connectdb.Open();
                string sql = "SELECT * FROM player_inventory WHERE playerpublickey = @playerpublickey";
                MySqlDataReader rd;
                using (var cmd = new MySqlCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = conn.connectdb;
                    cmd.Parameters.AddWithValue("@playerpublickey", Player.PublicPlayerKey);
                    rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        Player.AddToInventory(rd.GetString("name"), (int) rd.GetInt16("value"), false);
                    }
                }
            }
            
        }
    }
}
