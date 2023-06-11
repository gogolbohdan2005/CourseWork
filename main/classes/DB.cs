using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using main.classes;
using main;

namespace main.classes

{
    public static class DB
    {
        public static List<User> ReadAllUsers()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<User>();
                List<User> users = conn.Table<User>().ToList();
                return users;
            }
        }


    }
}
