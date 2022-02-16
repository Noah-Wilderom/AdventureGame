using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using AdventureGame;


namespace AdventureGame
{
    class Weapons
    {
        public static SortedList<string, string> getWeaponStats(string name, string ppk = null)
        {
            if (ppk == null) ppk = Player.PublicPlayerKey;
            SortedList<string, string> WeaponStats = new SortedList<string, string>();

            Database conn = new Database();
            conn.connectdb.Open();

            string query = "SELECT * FROM player_weapons WHERE playerpublickey = @ppk";
            MySqlDataReader rd;
            using (var cmd = new MySqlCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn.connectdb;

                cmd.Parameters.AddWithValue("@ppk", ppk);

                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    for (var f = 0; f < rd.FieldCount; f++)
                    {
                        WeaponStats.Add((string) rd.GetName(f), (string) rd.GetValue(f).ToString());
                    }
                }
            }


            return WeaponStats;
        }

        public static bool AddWeaponToPlayer(string ppk = null) // PlayerPublicKey
        {
            if (ppk == null) ppk = Player.PublicPlayerKey;
            return true;

        }

        public static void PrintWeaponStats(string name, string ppk = null)
        {
            if (ppk == null) ppk = Player.PublicPlayerKey;
            SortedList<string, string> WeaponStats = Weapons.getWeaponStats(name, ppk);
            if (WeaponStats.Count <= 0) return;
            Console.Write("Weapon stats voor ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Weapons.GetLabel(name) + "\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach ( var kv in WeaponStats )
            {
                if (kv.Key != "timestamp" && kv.Key != "name" && kv.Key != "playerpublickey" && kv.Key != "ID" && kv.Key != "label" && kv.Key != "value") Console.WriteLine(kv.Key.Substring(0, 1).ToUpper() + kv.Key.Substring(1) + " = " + kv.Value);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        public static string GetLabel(string name)
        {
            Database conn = new Database();
            conn.connectdb.Open();
            string sql = "SELECT * FROM weapons WHERE name = @name";
            MySqlDataReader rd;
            using (var cmd = new MySqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn.connectdb;
                cmd.Parameters.AddWithValue("@name", (string) name);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    return rd.GetString("label");
                }
            }
            return "Error geen label kunnen vinden voor " + name;
        }
    }
}
