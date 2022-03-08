using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureGame;
using MySql.Data.MySqlClient;

namespace AdventureGame
{
    class Items : Inventory
    {
        public bool Add(string Key, int Value)
        {
            if (!this.ValidItem(Key)) return false;
            Inventory.AddInventory(Key, Value);
            return true;
        }

        public bool ValidItem(string Key)
        {
            Database conn = new Database();
            conn.connectdb.Open();
            string sql = "SELECT * FROM items WHERE name = @name";
            MySqlDataReader rd;
            using (var cmd = new MySqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn.connectdb;
                cmd.Parameters.AddWithValue("@name", (string) Key);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    return true;
                }
                return false;
            }

        }
        /// <summary>
        ///  Searches trough the items table to get the associated label.
        /// </summary>
        /// <param name="name">name code of the item</param>
        /// <returns>Returns the label that is associated with the name code.</returns>
        public static string GetLabel(string name)
        {
            Database conn = new Database();
            conn.connectdb.Open();
            string sql = "SELECT * FROM items WHERE name = @name";
            MySqlDataReader rd;
            using (var cmd = new MySqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn.connectdb;
                cmd.Parameters.AddWithValue("@name", (string)name);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    return rd.GetString("label");
                }
            }
            return "Error geen label kunnen vinden voor " + name;
        }

        public static void Print()
        {
            Console.WriteLine("Player Inventory Items");
            foreach(var item in Inventory.ItemsInventory)
            {
                Console.Write("Key: ");
                Console.Write(Items.GetLabel(item.Key));
                Console.Write(" | Value: ");
                Console.Write(item.Value);
                Console.Write("\n");
            }
        }

    }
}
