using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using OxyPlot;
using OxyPlot.Wpf;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace МетодЕйлераРунгеКутта
{
    class SystemEducation
    {
        public static StackPanel Equation(int numberEducation)
        {
            StackPanel stackPanel = new StackPanel { Height = 28, Orientation = Orientation.Horizontal };

            Label label1 = new Label { Width = 70, Content = "dy"+numberEducation+"/dx = ", FontSize = 12 };
            ComboBox comboBox = new ComboBox { Text = "", Margin = new Thickness(3), BorderThickness = new Thickness(0.5), Width = 200, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, FontSize = 12, IsEditable = true };
            comboBox.ItemsSource = SystemEducation.ComboBoxItemsEquation();
            Label label2 = new Label { Height = 60, Content = "   y"+numberEducation+"(x)=" };
            TextBox textBox = new TextBox { Width = 50, Margin = new Thickness(3), BorderThickness = new Thickness(0.5), FontSize = 12, VerticalContentAlignment = System.Windows.VerticalAlignment.Center };
            stackPanel.Children.Add(label1);
            stackPanel.Children.Add(comboBox);
            stackPanel.Children.Add(label2);
            stackPanel.Children.Add(textBox);
            return stackPanel;
        }
        public static StackPanel Equation(int numberEducation, string function, string y)
        {
            StackPanel stackPanel = new StackPanel { Height = 28, Orientation = Orientation.Horizontal };

            Label label1 = new Label { Width = 70, Content = "dy" + numberEducation + "/dx = ", FontSize = 12 };
            ComboBox comboBox = new ComboBox { Text = function, Margin = new Thickness(3), BorderThickness = new Thickness(0.5), Width = 200, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, FontSize = 12, IsEditable = true };
            comboBox.ItemsSource = SystemEducation.ComboBoxItemsEquation();
            Label label2 = new Label { Height = 60, Content = "   y" + numberEducation + "(x)=" };
            TextBox textBox = new TextBox { Text=y, Width = 50, Margin = new Thickness(3), BorderThickness = new Thickness(0.5), FontSize = 12, VerticalContentAlignment = System.Windows.VerticalAlignment.Center };
            stackPanel.Children.Add(label1);
            stackPanel.Children.Add(comboBox);
            stackPanel.Children.Add(label2);
            stackPanel.Children.Add(textBox);
            return stackPanel;
        }
        public static StackPanel ExactEquation(int numberEducation)
        {
            StackPanel stackPanel = new StackPanel { Height = 28, Orientation = Orientation.Horizontal };

            Label label1 = new Label { Width = 70, Content = "Exact: y" + numberEducation, FontSize = 12 };
            ComboBox comboBox = new ComboBox { Text = "", Margin = new Thickness(3), BorderThickness = new Thickness(0.5), Width = 200, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, FontSize = 12, IsEditable = true };
            comboBox.ItemsSource = SystemEducation.ComboBoxItemsExact();
            stackPanel.Children.Add(label1);
            stackPanel.Children.Add(comboBox);
            return stackPanel;
        }
        public static StackPanel ExactEquation(int numberEducation, string function)
        {
            StackPanel stackPanel = new StackPanel { Height = 28, Orientation = Orientation.Horizontal };

            Label label1 = new Label { Width = 70, Content = "Exact: y" + numberEducation, FontSize = 12 };
            ComboBox comboBox = new ComboBox { Text = function, Margin = new Thickness(3), BorderThickness = new Thickness(0.5), Width = 200, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, FontSize = 12, IsEditable = true };
            comboBox.ItemsSource = SystemEducation.ComboBoxItemsExact();
            stackPanel.Children.Add(label1);
            stackPanel.Children.Add(comboBox);
            return stackPanel;
        }
        public static List<ComboBoxItem> ComboBoxItemsEquation()
        {
            List<ComboBoxItem> comboBoxItems = new List<ComboBoxItem>();
            comboBoxItems.Add(new ComboBoxItem { Content = "2*y2-3*y1" });
            comboBoxItems.Add(new ComboBoxItem { Content = "y2-2*y1" });
            comboBoxItems.Add(new ComboBoxItem { Content = "2*y1-y2" });
            comboBoxItems.Add(new ComboBoxItem { Content = "y2-2*y1+18*x" });
            comboBoxItems.Add(new ComboBoxItem { Content = "y1+2*x-3" });
            comboBoxItems.Add(new ComboBoxItem { Content = "y1*(1/x-1)" });
            return comboBoxItems;
        }
        public static List<ComboBoxItem> ComboBoxItemsExact()
        {
            List<ComboBoxItem> comboBoxItems = new List<ComboBoxItem>();
            comboBoxItems.Add(new ComboBoxItem { Content = "2*(1+x)*e^(0-x)" });
            comboBoxItems.Add(new ComboBoxItem { Content = "(3+2*x)*e^(0-x)" });
            comboBoxItems.Add(new ComboBoxItem { Content = "(0-2)*e^(3x)+3*x^2+2*x+3" });
            comboBoxItems.Add(new ComboBoxItem { Content = "2*e^(3*x)+6*x^2-2*x+4" });
            comboBoxItems.Add(new ComboBoxItem { Content = "2*e^x-2*x+1" });
            comboBoxItems.Add(new ComboBoxItem { Content = "e^(1-x)*x" });
            return comboBoxItems;
        }
    }
}
