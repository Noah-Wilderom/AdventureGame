using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AdventureGame
{
    class Database
    {
        private string connStr = "server=localhost;user=root;database=adventuregame;port=3306;password=";
        public MySqlConnection connectdb;

        public Database()
        {
            connectdb = new MySqlConnection(connStr);
        }

    }
}
