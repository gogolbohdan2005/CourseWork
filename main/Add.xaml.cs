using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using main.classes;

namespace main
{
    /// <summary>
    /// Interaction logic for Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        public Add()
        {
            InitializeComponent();
        }


        public bool dynamicErrorCheck = false;
        public bool dynamicCompleteCheck = false;
        Style baseTextBoxStyle = (Style)Application.Current.Resources["BaseTextBlock"];
        private void Button_Add(object sender, RoutedEventArgs e)
        {
            dynamicErrorCheck = false;
            dynamicCompleteCheck = false;
            dynamicError.Children.Clear();
            dynamicComplete.Children.Clear();
            if (string.IsNullOrWhiteSpace(title.Text) || string.IsNullOrWhiteSpace(descr.Text))
            {
                TextBlock dynamicText = new TextBlock();
                dynamicText.Text = "Input empty!";
                dynamicText.Style = baseTextBoxStyle;
                dynamicText.FontSize = 14;
                dynamicText.HorizontalAlignment = HorizontalAlignment.Center;
                if (!dynamicErrorCheck)
                {
                    dynamicError.Children.Add(dynamicText);
                    dynamicErrorCheck = true;
                }
            } 
            else
            {
                List<Note> userNotes = new List<Note>();
                using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                {
                    connection.CreateTable<User>();
                    User user = connection.Table<User>().FirstOrDefault(u => u.Id == App.LoggedInUser.Id);
                    if (user != null)
                    {
                        string notesJson = user.Notes;
                        userNotes = JsonConvert.DeserializeObject<List<Note>>(notesJson);
                    }
                }
                int nextId = 1;
                if (userNotes.Count > 0)
                {
                    nextId = userNotes.Max(n => n.Id) + 1;
                }

                Note notes = new Note()
                {
                    Id = nextId,
                    Title = title.Text,
                    Description = descr.Text,
                    Date = DateTime.Now
                };
                userNotes.Add(notes);
                string updatedNotesJson = JsonConvert.SerializeObject(userNotes);
                using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                {
                    connection.CreateTable<User>();
                    User user = connection.Table<User>().FirstOrDefault(u => u.Id == App.LoggedInUser.Id);
                    if (user != null)
                    {
                        user.Notes = updatedNotesJson;
                        connection.Update(user);
                    }
                }
                TextBlock dynamicText = new TextBlock();
                dynamicText.Text = "Note with title " + title.Text + " was created!";
                dynamicText.Style = baseTextBoxStyle;
                dynamicText.FontSize = 14;
                dynamicText.HorizontalAlignment = HorizontalAlignment.Center;
                if (!dynamicCompleteCheck)
                {
                    dynamicComplete.Children.Add(dynamicText);
                    dynamicCompleteCheck = true;
                }
            }
        }

        private void Button_Out(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void descr_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
