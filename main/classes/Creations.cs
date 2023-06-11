using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;

namespace main.classes
{
    class Creations
    {

        public static void CreateTextBlockOfTextBox(DependencyObject parent)
        {
            //var arr = VisualTreeHelperExtensions.FindChildrenOfType<TextBox>(parent);

            IEnumerable<DependencyObject> textBoxEnumerable = VisualTreeHelperExtensions.FindChildrenOfType<TextBox>(parent).ToList();
            List<TextBox> textBoxList = textBoxEnumerable.OfType<TextBox>().ToList();

            foreach (TextBox textBox in textBoxList)
            {
                textBox.IsReadOnly = true;
                textBox.BorderThickness = new Thickness(0);
                textBox.Background = Brushes.Transparent;
            }
            // GETTING COMBOBOX
            ComboBox comboBox = comboBoxAction.GetComboBox((DockPanel)parent);
            // Perform actions based on the selected item
            string comboBoxValue = comboBox.SelectedItem.ToString();

            // SAVING
            int numToSave = int.Parse(textBoxList[2].Text.TrimEnd('₴')); // NUMBER GOT FROM THE TEXTBOX
            sqlManager.InsertDatabase(comboBoxValue, textBoxList[1].Text, numToSave); // GIVE INFORMATION TO DATABASE
        }


        public static TextBox CreateTextBox(string text_info, int width, DockPanel parent)
        {
            TextBox textBox = new TextBox();

            // Set properties for the TextBox
            //var textBoxStyle = Application.Current.FindResource("TextBoxStyle") as Style;
            //textBox.Style = textBoxStyle;
            textBox.Width = width;
            HintAssist.SetHint(textBox, text_info);

            // GOT FOCUS EVENT
            textBox.GotFocus += (sender, e) =>
            {
                Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#44D492"));
                HintAssist.SetBackground((TextBox)sender, brush);
            };

            // Handle the LostFocus event
            textBox.LostFocus += (sender, e) =>
            {
                TextBox focusedTextBox = (TextBox)sender;
                HintAssist.SetBackground(focusedTextBox, Brushes.Transparent);

                // IF PERSON ENETERED SOME VALUE
                if (focusedTextBox.Text != "")
                {
                    HintAssist.SetHint(textBox, "");
                    HintAssist.SetIsFloating(focusedTextBox, false);
                }
            };

            // Add the TextBox to the container
            parent.Children.Add(textBox);
            return textBox;
        }


        public static Button CreateButton(DockPanel parent)
        {
            Button btn = new Button();
            Grid grid = new Grid();
            // Set properties for the TextBox

            var btnStyle = Application.Current.FindResource("_MaterialDesignFloatingActionAccentButton") as Style;
            btn.Style = btnStyle;
            btn.Width = 40;
            btn.Height = 40;

            // Create PackIcon element
            PackIcon packIcon = new PackIcon();
            packIcon.Width = 25;
            packIcon.Height = 25;
            packIcon.Kind = PackIconKind.Check;

            // Add PackIcon to Grid
            grid.Children.Add(packIcon);

            // Set the Grid as the content of the Button
            btn.Content = grid;

            // Add the Btn to the container
            parent.Children.Add(btn);
            return btn;
        }


        public static Button CreateEntryBlock(DockPanel parent, Action<object, TextCompositionEventArgs> eventHandler)
        {
            // PASSING EVENT HANDLER USING THIS DELEGATE FORM
            DockPanel dockPanel = new DockPanel();
            dockPanel.SetValue(DockPanel.DockProperty, Dock.Top);
            dockPanel.VerticalAlignment = VerticalAlignment.Top;

            comboBoxAction.CreateComboBox(20, dockPanel);
            CreateTextBox("Enter expence", 160, dockPanel);

            // CREATING TEXT BOX WITH PREWIEV TEXT EVENT HANDLER
            CreateTextBox("0", 75, dockPanel).PreviewTextInput += new TextCompositionEventHandler(eventHandler);
            Button eventButton = CreateButton(dockPanel);

            eventButton.HorizontalAlignment = HorizontalAlignment.Right; // MAKE BUTTON APPERAR FROM THE RIGHT
            parent.Children.Add(dockPanel);
            return eventButton; // RETURNING TO ADD EVENT HANDLER LATELY
        }

        // ON DIALOOGUE WINDOW OPEN
        public static bool ShowConfirmationDialog()
        {
            // Create an instance of the window
            var dialogWindow = new ConfirmationDialog();
            dialogWindow.Owner = Application.Current.MainWindow;
            dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            // Open the window as a dialog
            if (dialogWindow.ShowDialog() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
