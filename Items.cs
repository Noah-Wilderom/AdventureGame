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

    }
}
