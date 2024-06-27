using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ConvexHullApp
{
    /// <summary>
    /// Interaction logic for class ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        public ListWindow()
        {
            InitializeComponent();
        }

        public void AddToList(ConvexHullApp.Point point) 
        {
            TextBlock newItem = new()
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
