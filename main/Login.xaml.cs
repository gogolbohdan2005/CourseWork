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
using System.Xml.Linq;
using main.classes;
using main;


namespace main
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        bool dynamicErrorEmpty = false;
        bool dynamicErrorFailed = false;
        Style baseTextBoxStyle = (Style)Application.Current.Resources["BaseTextBlock"];
        private void Button_SingIn(object sender, RoutedEventArgs e)
        {
            List<User> users = DB.ReadAllUsers();
            dynamicContainerEmpty.Children.Clear();
            dynamicContainerFailed.Children.Clear();
            dynamicErrorEmpty = false;
            dynamicErrorFailed = false;

            if (string.IsNullOrWhiteSpace(login.Text) || string.IsNullOrWhiteSpace(pass.Password))
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
            }
            else
            {
                bool correctData = users.Exists(u => (u.Name == login.Text || u.Email == login.Text) && u.Password == pass.Password);
                if(correctData)
                {
                    User loggedInUser = users.Find(u => (u.Name == login.Text || u.Email == login.Text) && u.Password == pass.Password);
                    App.LoggedInUser = loggedInUser; // Збереження інформації про користувача
                    Menu menu = new Menu();
                    menu.Show();
                    this.Close();
                } else
                {
                    TextBlock dynamicText = new TextBlock();
                    dynamicText.Text = "Uncorrect Data!";
                    dynamicText.Style = baseTextBoxStyle;
                    dynamicText.FontSize = 14;
                    dynamicText.HorizontalAlignment = HorizontalAlignment.Center;
                    if (!dynamicErrorFailed)
                    {
                        dynamicContainerFailed.Children.Add(dynamicText);
                        dynamicErrorFailed = true;
                    }
                }
            }
        }
        private void Button_ToSingUp(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Close();
        }
    }
}
