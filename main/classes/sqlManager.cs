using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.classes
{
    class sqlManager
    {
        public static string GenerateDatabasePath(string databaseName)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            folderPath += Global.folderName;
            Directory.CreateDirectory(folderPath);
            //string databasePath = System.IO.Path.Combine(folderPath, databaseName);
            string databasePath = folderPath + "\\" + databaseName;
            return databasePath;
        }

        static public List<Expence> ReadDatabase()
        {
            // READING INFO FROM DB
            List<Expence> contacts;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(Global.dbName))
            {
                // FIRSTLY CONNECTING, CREATING IF IT IS NO ACTUAL DB
                conn.CreateTable<Expence>();
                // GET LIST OF ALL CLASSES, ALL INFO
                contacts = conn.Table<Expence>().ToList();
            }
            return contacts;

        }

        static public List<Expence> ReadDatabaseWithCertainField(string fieldValue, string name)
        {
            List<Expence> expenses;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(name))
            {
                // FIRSTLY CONNECTING, CREATING IF IT IS NO ACTUAL DB
                conn.CreateTable<Expence>();

                // SELECTING CLASSES WITH A CERTAIN FIELD
                expenses = conn.Table<Expence>().Where(expense => expense.Category == fieldValue).ToList();
            }
            return expenses;
        }


        static public void InsertDatabase(string categoryName, string nameFromTextBox, int numberFromTextBox)
        {
            // CREATING CLASS TO SAVE IN DB
            Expence exp = new Expence()
            {
                // SET CLASS WITH CERTAIN VALUES
                Category = categoryName,
                Name = nameFromTextBox,
                Price = numberFromTextBox
            };

            using (SQLiteConnection connection = new SQLiteConnection(Global.dbName))
            {
                // CONECTED AND ADDED SOME INFORMATION WIA INSERT
                connection.CreateTable<Expence>();
                connection.Insert(exp);
            }
        }

        public static void DeleteItem(Expence expence)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Global.dbName))
            {
                connection.Delete(expence);
            }
        }
    }
}
