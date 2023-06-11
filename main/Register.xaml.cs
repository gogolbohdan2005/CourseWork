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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using main.classes;
using main;


namespace main
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        bool dynamicErrorEmpty = false;
        bool dynamicErrorRegistered = false;
        Style baseTextBoxStyle = (Style)Application.Current.Resources["BaseTextBlock"];
        private void Button_SingUp(object sender, RoutedEventArgs e)
        {
            List<User> users = DB.ReadAllUsers(); 
            dynamicContainerEmpty.Children.Clear();
            dynamicContainerRegistered.Children.Clear();
            dynamicErrorEmpty = false;
            dynamicErrorRegistered = false;
            bool isValidEmail = Regex.IsMatch(email.Text, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$");
            if (string.IsNullOrWhiteSpace(login.Text) || string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrWhiteSpace(pass.Password))
            {
                TextBlock dynamicText = new TextBlock();
                dynamicText.Text = "Input is empty";
                dynamicText.Style = baseTextBoxStyle;
                dynamicText.FontSize = 14;
                dynamicText.HorizontalAlignment = HorizontalAlignment.Center;
                if (!dynamicErrorEmpty)
                {
                    dynamicContainerEmpty.Children.Add(dynamicText);
                    dynamicErrorEmpty = true;
                }
            } else
            {
                bool nameExists = users.Exists(u => u.Name == login.Text);
                bool emailExist = users.Exists(u => u.Email == email.Text);
                if (nameExists || emailExist)
                {

                    TextBlock dynamicText = new TextBlock();
                    dynamicText.Text = "User with this name or email is already registered";
                    dynamicText.Style = baseTextBoxStyle;
                    dynamicText.FontSize = 14;
                    dynamicText.HorizontalAlignment = HorizontalAlignment.Center;
                    if (!dynamicErrorRegistered)
                    {
                        dynamicContainerRegistered.Children.Add(dynamicText);
                        dynamicErrorRegistered = true;
                    }
                } else if(!isValidEmail)
                {
                    TextBlock dynamicText = new TextBlock();
                    dynamicText.Text = "Uncorrect email!";
                    dynamicText.Style = baseTextBoxStyle;
                    dynamicText.FontSize = 14;
                    dynamicText.HorizontalAlignment = HorizontalAlignment.Center;
                    if (!isValidEmail)
                    {
                        dynamicContainerEmpty.Children.Add(dynamicText);
                    }
                }
                else
                {
                    List<Note> notes = new List<Note>();
                    string notesJson = JsonConvert.SerializeObject(notes);
                    User user = new User()
                    {
                        Name = login.Text,
                        Email = email.Text,
                        Password = pass.Password,
                        Notes = notesJson,
                        Date = DateTime.Now
                    };
                    using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                    {
                        connection.CreateTable<User>();
                        connection.Insert(user);
                    }
                    TextBlock dynamicText = new TextBlock();
                    dynamicText.Text = "Completed!";
                    dynamicText.Style = baseTextBoxStyle;
                    dynamicText.FontSize = 14;
                    dynamicText.HorizontalAlignment = HorizontalAlignment.Center;
                    dynamicContainerEmpty.Children.Add(dynamicText);
                }
            }
        }

        private void Button_ToSingIn(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
