using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace main.classes
{
    internal class Global
    {
        static public string dbName = ""; // INITIALIZED WITH TODAY VAUE WHEN PROJECT STARTS
        static public string folderName = "\\EveryWallet"; // INITIALIZED WITH TODAY VAUE WHEN PROJECT STARTS
        static public string userName; // INITIALIZE WHEN LOGIN STARTS


        static public string jsonPath; // INITIALIZED WITH TODAY VAUE WHEN PROJECT STARTS
        static public string jsonFileName = "\\data.json"; // INITIALIZED WITH TODAY VAUE WHEN PROJECT STARTS
        
        static public long lastTimeParsed; // INITIALIZED WITH TODAY VAUE WHEN PROJECT STARTS
        static public int lastSelectedId = 0;

        static public ObservableCollection<string> Options { get; set; } // OPTIONS READ FROM JSON FILE

        static public Dictionary<string, SolidColorBrush> colorDictionary = new Dictionary<string, SolidColorBrush>()
        {
            { "Rent", new SolidColorBrush(Color.FromRgb(237, 125, 49)) },     // Orange
            { "Travel", new SolidColorBrush(Color.FromRgb(0, 120, 215)) },     // Blue
            { "Food", new SolidColorBrush(Color.FromRgb(0, 180, 92)) },        // Green
            { "Friends", new SolidColorBrush(Color.FromRgb(171, 17, 119)) },   // Purple
            { "Restaurant", new SolidColorBrush(Color.FromRgb(240, 150, 9)) },  // Gold
            { "Comfort", new SolidColorBrush(Color.FromRgb(63, 81, 181)) },     // Indigo
            { "Card", new SolidColorBrush(Color.FromRgb(233, 30, 99)) },       // Pink
            // Add more entries as needed
        };
    }
}
