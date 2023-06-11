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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using main.classes;

namespace main
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : Window
    {
        private Note note;
        public View(Note note)
        {
            InitializeComponent();
            this.note = note;
            Title = "Note: " + note.Title;
            InitializeDynamicText();
        }

        private void InitializeDynamicText()
        {
            dynamicTitle.Text = note.Title;
            dynamicDescr.Text = note.Description;
            dynamicDate.Text = note.Date.ToString();
        }
        private void Button_Out(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            dynamicTitle.IsReadOnly = false;
            dynamicDescr.IsReadOnly = false;
            dynamicTitle.AcceptsReturn = true;
            dynamicDescr.AcceptsReturn = true;
            save.Visibility = Visibility.Visible;
            delete.Visibility = Visibility.Visible;
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            dynamicTitle.IsReadOnly = true;
            dynamicDescr.IsReadOnly = true;
            save.Visibility = Visibility.Hidden;
            delete.Visibility = Visibility.Hidden;


            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<User>();
                User user = connection.Table<User>().FirstOrDefault(u => u.Id == App.LoggedInUser.Id);
                if (user != null)
                {
                    List<Note> userNotes = JsonConvert.DeserializeObject<List<Note>>(user.Notes);
                    Note noteToUpdate = userNotes.FirstOrDefault(n => n.Id == App.NoteId);
                    if (noteToUpdate != null)
                    {
                        // Оновлення значення нотатки
                        noteToUpdate.Title = dynamicTitle.Text;
                        noteToUpdate.Description = dynamicDescr.Text;

                        // Оновлення JSON-представлення списку нотаток користувача
                        user.Notes = JsonConvert.SerializeObject(userNotes);

                        // Збереження змін до бази даних
                        connection.Update(user);
                    }
                }
            }

        }
        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<User>();
                User user = connection.Table<User>().FirstOrDefault(u => u.Id == App.LoggedInUser.Id);
                if (user != null)
                {
                    List<Note> userNotes = JsonConvert.DeserializeObject<List<Note>>(user.Notes);
                    Note noteToUpdate = userNotes.FirstOrDefault(n => n.Id == App.NoteId);
                    if (noteToUpdate != null)
                    {
                        // Видалення зі списку
                        userNotes.Remove(noteToUpdate);
                        foreach (var users in userNotes) {
                            if(users.Id > App.NoteId) users.Id -= 1;
                        }
                        // Оновлення JSON-представлення списку нотаток користувача
                        user.Notes = JsonConvert.SerializeObject(userNotes);

                        // Збереження змін до бази даних
                        connection.Update(user);
                    }
                }
            }
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
    }
}
