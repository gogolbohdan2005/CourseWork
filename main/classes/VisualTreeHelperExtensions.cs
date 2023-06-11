using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace main.classes
{
    public static class VisualTreeHelperExtensions
    {
        // GETS US CHILDRENS OF A BLOCK IN AN ARRAY
        public static IEnumerable<DependencyObject> FindChildrenOfType<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T)
                {
                    yield return child;
                }

                foreach (var nestedChild in FindChildrenOfType<T>(child))
                {
                    yield return nestedChild;
                }
            }
        }

        static public void ClearDockPanelChildren(DockPanel dockPanel)
        {
            while (dockPanel.Children.Count > 0)
            {
                UIElement child = dockPanel.Children[0];
                dockPanel.Children.Remove(child);

                // Check if the child is a container (e.g., another DockPanel) and recursively clear its children
                if (child is DockPanel childDockPanel)
                {
                    ClearDockPanelChildren(childDockPanel);
                }
            }
        }
    }
}
