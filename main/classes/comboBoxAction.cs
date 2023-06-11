using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace main.classes
{
    internal class comboBoxAction
    {
        public static ComboBox GetComboBox(DockPanel parent)
        {
            return (ComboBox)parent.Children[0];
        }

        public static ComboBox CreateComboBox(int width, DockPanel parent)
        {
            // Create the ComboBox instance
            ComboBox comboBox = new ComboBox();
            comboBox.Width = width;
            comboBox.Height = width;
            comboBox.Background = Brushes.AliceBlue;

            // Create a binding for the ItemsSource property
            Binding binding = new Binding("Options");

            // Apply the binding to the ComboBox's ItemsSource property
            comboBox.SetBinding(ComboBox.ItemsSourceProperty, binding);

            // Add the ComboBox to a parent container (e.g., a Grid)
            parent.Children.Add(comboBox);

            return comboBox;
        }

        
    }
}
