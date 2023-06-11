using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;


namespace main.classes
{
    internal class Statistics
    {
        public static void CreateRectangle(double height, double width, Panel parent, string category)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Height = height;
            rectangle.Width = width;
            rectangle.Fill = Global.colorDictionary[category]; // Example fill color, you can modify as needed

            parent.Children.Add(rectangle); // Add the rectangle to the parent container
        }
        
        public static StackPanel CreateStackPanel(string text, int height, Panel parent)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            stackPanel.Margin = new Thickness(2);
            // Add child elements to the stackPanel

            // Example: Add a TextBox to the stackPanel
            CreateRectangle(height, 40, stackPanel, text);
            TextBlock textBox = new TextBlock();
            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.Text = text;
            textBox.FontSize = 10;
            stackPanel.Children.Add(textBox);


            // Add the stackPanel to the parent container or window
            parent.Children.Add(stackPanel);
            return stackPanel;
        }

        public static void CreateStatistics(DockPanel parent)
        {
            VisualTreeHelperExtensions.ClearDockPanelChildren(parent);

            Dictionary<string, int> categoryAndSum = new Dictionary<string, int>();
            int global_sum = 0;
            foreach (string option in Global.Options)
            {
                // Perform the desired action for each element in the collection
                List<Expence> arr = sqlManager.ReadDatabaseWithCertainField(option, Global.dbName);
                int total = 0;
                foreach (Expence item in arr)
                {
                    total += item.Price;
                }
                // CALCULATE SUM, BUILD RECT
                if (total != 0)
                {
                    global_sum += total;
                    categoryAndSum[option] = total;
                }
            }

            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            int height = (int)currentWindow.ActualHeight - 150;
            foreach (KeyValuePair<string, int> kvp in categoryAndSum)
            {
                Statistics.CreateStackPanel(kvp.Key, kvp.Value * height / global_sum, parent);
            }
        }
    }   

}
