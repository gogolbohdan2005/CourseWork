using main.classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int selectedIndex = 0;
        public static User LoggedInUser { get; set; }
        public static int NoteId;
        public static string databaseName = "Users.db";
        public static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string databasePath = System.IO.Path.Combine(folderPath, databaseName);

    }
}
