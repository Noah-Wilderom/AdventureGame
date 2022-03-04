using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace AdventureGame
{
    class Inventory
    {
        private static SortedList<string, int> Items = new SortedList<string, int>();
        private static SortedList<string, int> Weapons = new SortedList<string, int>();

        public static void AddInventory(string Key, int Value, bool IsWeapon = false)
        {
            if(IsWeapon)
            {
                if (Weapons.ContainsKey(Key))
                {
                    Weapons[Key] += Value;
                    return;
                }
                Weapons.Add(Key, Value);
                return;
            }
            if (Items.ContainsKey(Key))
            {
                Items[Key] += Value;
                return;
            }
            Items.Add(Key, Value);
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
            }

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
            foreach (var entry in Items)
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
            string queryWeapons = "INSERT INTO player_weapons (playerpublickey, name, value) VALUES (@ppk, @name, @value)";
            foreach (var entry in Weapons)
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
            foreach (var entry in Items)
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
