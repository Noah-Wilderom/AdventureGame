using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace AdventureGame
{
    class Inventory
    {
        public static SortedList<string, int> ItemsInventory { get; private set; } = new SortedList<string, int>();
        public static SortedList<string, int> WeaponsInventory { get; private set; } = new SortedList<string, int>();

        public static void AddInventory(string Key, int Value, bool IsWeapon = false)
        {
            if(IsWeapon)
            {
                if (WeaponsInventory.ContainsKey(Key))
                {
                    WeaponsInventory[Key] += Value;
                    return;
                }
                WeaponsInventory.Add(Key, Value);
                return;
            }
            if (ItemsInventory.ContainsKey(Key))
            {
                ItemsInventory[Key] += Value;
                return;
            }
            ItemsInventory.Add(Key, Value);
            return;

        }

        public void LoadInventory()
        {
            Database conn = new Database();
            conn.connectdb.Open();
            string sqlItems = "SELECT * FROM player_inventory WHERE playerpublickey = @playerpublickey";
            MySqlDataReader rdItems;
            using (var cmd = new MySqlCommand())
            {
                cmd.CommandText = sqlItems;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn.connectdb;
                cmd.Parameters.AddWithValue("@playerpublickey", Player.PublicPlayerKey);
                rdItems = cmd.ExecuteReader();
                while (rdItems.Read())
                {
                    Inventory.AddInventory(rdItems.GetString("name"), (int)rdItems.GetInt16("value"));
                }
                conn.connectdb.Close();

            }
            conn.connectdb.Open();
            string sqlWeapons = "SELECT * FROM player_weapons WHERE playerpublickey = @playerpublickey";
            MySqlDataReader rdWeapons;
            using (var cmd = new MySqlCommand())
            {
                cmd.CommandText = sqlWeapons;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn.connectdb;
                cmd.Parameters.AddWithValue("@playerpublickey", Player.PublicPlayerKey);
                rdWeapons = cmd.ExecuteReader();
                while (rdWeapons.Read())
                {
                    Inventory.AddInventory(rdWeapons.GetString("name"), (int) rdWeapons.GetInt16("value"), true);
                }
                conn.connectdb.Close();

            }
        }
        /// <summary>
        /// Saves the inventory to the database
        /// return void 
        /// </summary>
        public void Save()
        {
            Database conn = new Database();
            string queryItems = "INSERT INTO player_inventory (playerpublickey, name, value) VALUES (@ppk, @name, @value)";
            string queryItemsUpdate = "UPDATE player_inventory SET value = @val WHERE playerpublickey = @ppk";
            foreach (var entry in ItemsInventory)
            {
                bool check = false;
                conn.connectdb.Open();
                string sql = "SELECT * FROM player_inventory WHERE playerpublickey = @ppk AND name = @name";
                MySqlDataReader rd;
                using (var cmd = new MySqlCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = conn.connectdb;
                    cmd.Parameters.AddWithValue("@ppk", (string)Player.PublicPlayerKey);
                    cmd.Parameters.AddWithValue("@name", (string)entry.Key);
                    rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        check = true;
                    }
                    check = false;
                    conn.connectdb.Close();
                }
                if (check)
                {
                    conn.connectdb.Open();
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandText = queryItemsUpdate;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Connection = conn.connectdb;

                        cmd.Parameters.AddWithValue("@ppk", (string)Player.PublicPlayerKey);
                        cmd.Parameters.AddWithValue("@name", (string)entry.Key);
                        cmd.Parameters.AddWithValue("@value", (int)entry.Value);


                        cmd.ExecuteNonQuery();
                        conn.connectdb.Close();

                    }
                }
                else
                {
                    conn.connectdb.Open();
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandText = queryItems;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Connection = conn.connectdb;

                        cmd.Parameters.AddWithValue("@ppk", (string)Player.PublicPlayerKey);
                        cmd.Parameters.AddWithValue("@name", (string)entry.Key);
                        cmd.Parameters.AddWithValue("@value", (int)entry.Value);


                        cmd.ExecuteNonQuery();
                        conn.connectdb.Close();

                    }
                }

            }
            string queryWeapons = "INSERT INTO player_weapons (playerpublickey, name, value) VALUES (@ppk, @name, @value)";
            string queryWeaponsUpdate = "UPDATE player_weapons SET value = @val WHERE playerpublickey = @ppk AND name = @name";
            foreach (var entry in WeaponsInventory)
            {
                bool check = false;
                conn.connectdb.Open();
                string sql = "SELECT * FROM player_weapons WHERE playerpublickey = @ppk AND name = @name";
                MySqlDataReader rd;
                using (var cmd = new MySqlCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = conn.connectdb;
                    cmd.Parameters.AddWithValue("@ppk", (string)Player.PublicPlayerKey);
                    cmd.Parameters.AddWithValue("@name", (string)entry.Key);
                    rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        check = true;
                    }
                    check = false;
                    conn.connectdb.Close();
                }
                if(check)
                {
                    conn.connectdb.Open();
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandText = queryWeaponsUpdate;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Connection = conn.connectdb;

                        cmd.Parameters.AddWithValue("@ppk", (string)Player.PublicPlayerKey);
                        cmd.Parameters.AddWithValue("@name", (string)entry.Key);
                        cmd.Parameters.AddWithValue("@value", (int)entry.Value);


                        cmd.ExecuteNonQuery();
                        conn.connectdb.Close();

                    }
                } else
                {
                    conn.connectdb.Open();
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandText = queryWeapons;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Connection = conn.connectdb;

                        cmd.Parameters.AddWithValue("@ppk", (string)Player.PublicPlayerKey);
                        cmd.Parameters.AddWithValue("@name", (string)entry.Key);
                        cmd.Parameters.AddWithValue("@value", (int)entry.Value);


                        cmd.ExecuteNonQuery();
                        conn.connectdb.Close();

                    }
                }
                
            }
        }
        /// <summary>
        /// Initializes a new player
        /// return void
        /// </summary>
        public static void InitNewPlayerInventory()
        {
            Database conn = new Database();
            if (!Player.ValidatePlayer())
            {
                Player.InitPlayer();
            }
            else return;

            // Als de speler nieuw is inventory -> database
            string query = "INSERT INTO player_inventory (playerpublickey, name, value) VALUES (@ppk, @name, @value)";
            foreach (var entry in ItemsInventory)
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
    }
}
