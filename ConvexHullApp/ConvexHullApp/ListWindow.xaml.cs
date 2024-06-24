using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ConvexHullApp
{
    /// <summary>
    /// Logika interakcji dla klasy ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        public ListWindow()
        {
            InitializeComponent();
        }

        public void AddToList(ConvexHullApp.Point point) 
        {
            TextBlock newItem = new TextBlock
            {
                Text = ListWindowStackPanel.Children.Count + ") " + (point.X).ToString().PadRight(21) + " " + (point.Y).ToString().PadRight(21),
                FontSize = 18,
                Margin = new Thickness(5)
            };

            ListWindowStackPanel.Children.Add(newItem);
        }

        public void ClearList() 
        {
            ListWindowStackPanel.Children.Clear();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
