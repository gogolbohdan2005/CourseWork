using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using main;

namespace main
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            DynamicText.Text = App.LoggedInUser.Name;
            InitializeDynamicListBox();
            SortListBox();
        }

        private void InitializeDynamicListBox()
        {
            ObservableCollection<Note> notes = new ObservableCollection<Note>();

            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                // Зчитування User з бази даних за допомогою умови WHERE
                User user = connection.Table<User>().FirstOrDefault(u => u.Id == App.LoggedInUser.Id);

                if (user != null)
                {
                    // Поле "Notes" містить рядок JSON з нотатками
                    string notesJson = user.Notes;

                    // Десеріалізація рядка JSON у колекцію нотаток
                    List<Note> userNotes = JsonConvert.DeserializeObject<List<Note>>(notesJson);

                    // Додавання нотаток до ObservableCollection
                    foreach (Note note in userNotes)
                    {
                        notes.Add(note);
                    }
                }
            }
            dynamicListBox.ItemsSource = notes;
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedComboBoxItem = combobox.SelectedItem as ComboBoxItem;

            if (selectedComboBoxItem != null)
            {
                App.selectedIndex = int.Parse(selectedComboBoxItem.Tag.ToString());
                SortListBox();
            }
        }

        private void SortListBox()
        {
            var notes = dynamicListBox.ItemsSource as ObservableCollection<Note>;

            if (notes != null)
            {
                ObservableCollection<Note> sortedNotes;

                if (App.selectedIndex == 1)
                {
                    sortedNotes = new ObservableCollection<Note>(notes.OrderBy(note => note.Title));
                }
                else if (App.selectedIndex == 2)
                {
                    sortedNotes = new ObservableCollection<Note>(notes.OrderByDescending(note => note.Date));
                }
                else if (App.selectedIndex == 3)
                {
                    sortedNotes = new ObservableCollection<Note>(notes.OrderBy(note => note.Date));
                }
                else
                {
                    sortedNotes = new ObservableCollection<Note>(notes); // Залиште початкову колекцію, якщо індекс не відповідає жодному з варіантів
                }

                dynamicListBox.ItemsSource = sortedNotes;
            }
        }


        private void Button_Add(object sender, RoutedEventArgs e)
        {
            Add add = new Add();
            add.Show();
            this.Close();
        }
        private void Button_Out(object sender, RoutedEventArgs e)
        {
            App.LoggedInUser = null;
            Login login = new Login();
            login.Show();
            this.Close();
        }
        private void Button_ToNote(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Note selectedNote = (Note)button.DataContext;
            App.NoteId = selectedNote.Id;
            View view = new View(selectedNote);
            view.Show();
            this.Close();
        }


        // PASS USER NAME HERE, NOT NULL !!!
        private void Button_ToMoneyManager(object sender, RoutedEventArgs e)
        {
            Global.userName = App.LoggedInUser.Name; // UNIQUE ID AS EMAIL
            ExpenceWindow expenceWindow = new ExpenceWindow();
            expenceWindow.Show();
            this.Close();
        }
    }
}
