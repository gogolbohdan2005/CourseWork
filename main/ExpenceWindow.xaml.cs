using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using main.classes;
using MaterialDesignThemes.Wpf;
using Microsoft.VisualBasic.FileIO;
using Calendar = System.Windows.Controls.Calendar;

namespace main
{
    public partial class ExpenceWindow : Window
    {

        public ExpenceWindow()
        {
            DataContext = new Global(); // SET THIS APP FILE FOR DATA BINDING, TO LOAD INFO FROM THIS MAIN FILE

            // LOADING JSON FILE WITH CATEGORIES 
            Global.Options = new ObservableCollection<string>()
            {
                "Rent",
                "Travel",
                "Food",
                "Friends",
                "Restraunt",
                "Comfort",
                "Card"
            };

            // INITIALIZE CURRENT DATA
            DateTime todayDate = DateTime.Today;

            Global.folderName += $"\\{Global.userName}";

            // SET DATABASE PATH RELATIVELY TO CURRENT DATA
            Global.dbName = sqlManager.GenerateDatabasePath(todayDate.ToString("yyyy-MM-dd") + ".db");
            Global.jsonPath = ApiTools.GenerateJsonPath();

            InitializeComponent();
            ExpensesListView.ItemsSource = sqlManager.ReadDatabase();

        }


        public void Add_Click(object sender, RoutedEventArgs e)
        {
            Creations.CreateEntryBlock(ExpensesBox, TextBox_PreviewTextInput).Click += Ok_Click;
            // PASS EVENT HANDLER USING ACTION DELEGATE FORM THAT CHAT WROTE FOR ME
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            CalendarPopup.IsOpen = true;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {   
            // GET PARENT AS DEPEDENCY OBJECT
            DependencyObject parent = VisualTreeHelper.GetParent(sender as DependencyObject); 
            // TAKE OUR FUNCTION AND MAKE TEXT BOXES UNEDITABLE
            Creations.CreateTextBlockOfTextBox(parent);
            // REMOVE BUTTON FROM DOCK PANEL
            (parent as DockPanel).Children.Remove(sender as Button); 
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Operations.IsNumericInput(e.Text))
            {
                e.Handled = true; // Ignore the non-numeric input
            }
            else
            {
                TextBox textBox = (sender as TextBox);
                textBox.Text = $"{textBox.Text.TrimEnd('₴')}₴";
            }
        }

        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar dataPicker = sender as Calendar;

            // Handle the date selection event here
            DateTime selectedDate = dataPicker.SelectedDate.Value;
            // Do something with the selected date
            Global.dbName = sqlManager.GenerateDatabasePath(selectedDate.ToString("yyyy-MM-dd") + ".db");

            // Load new data from database
            ExpensesListView.ItemsSource = sqlManager.ReadDatabase();
            VisualTreeHelperExtensions.ClearDockPanelChildren(ExpensesBox);
            VisualTreeHelperExtensions.ClearDockPanelChildren(StatisticsBox);
            //Statistics.CreateStatistics(StatisticsBox);

            // Close Calendar
            CalendarPopup.IsOpen = false;
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            ExpensesListView.ItemsSource = sqlManager.ReadDatabase();
            VisualTreeHelperExtensions.ClearDockPanelChildren(ExpensesBox);
            VisualTreeHelperExtensions.ClearDockPanelChildren(StatisticsBox);
            Statistics.CreateStatistics(StatisticsBox);
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if any item is selected
            if (ExpensesListView.SelectedItem != null)
            {
                // Get the bound class from the selected item
                Expence selectedClass = (Expence)ExpensesListView.SelectedItem;
                int age = selectedClass.Id;

                // Access properties of the selected class
                //int id = selectedClass.Id;
                //string name = selectedClass.Name;
                //MessageBox.Show($"Selected item: {name}, Age: {age}");
                if (age != Global.lastSelectedId)
                {
                    Global.lastSelectedId = age;
                    if (Creations.ShowConfirmationDialog() == true)
                    {
                        sqlManager.DeleteItem(selectedClass);
                        ExpensesListView.ItemsSource = sqlManager.ReadDatabase();
                    }

                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //Statistics.CreateStackPanel(StatisticsBox);
            //Close();
        }

        // ASYNC FUNCTION
        async private void AddWithMono_Click(object sender, RoutedEventArgs e)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (Global.lastTimeParsed + 90 < now)
            {
                Global.lastTimeParsed = now;
                await ApiTools.FetchDataAndSaveToFileAsync();
                ApiTools.InsertFromJsonToTable(Global.jsonPath);
                ExpensesListView.ItemsSource = sqlManager.ReadDatabase();
                VisualTreeHelperExtensions.ClearDockPanelChildren(StatisticsBox);
                Statistics.CreateStatistics(StatisticsBox);
            }
            else
            {
                MessageBox.Show("Too often trying to parse data");
                // YOU CAN REVOKE API EVERY 1.5 MINUTES ONLY
            }
            
        }

        private void AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
