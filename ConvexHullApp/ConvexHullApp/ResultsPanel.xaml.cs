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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConvexHullApp
{
    /// <summary>
    /// Logika interakcji dla klasy ResultsPanel.xaml
    /// </summary>
    public partial class ResultsPanel : UserControl
    {
        private ListWindow input_points_list_window;

        public event EventHandler? ReturnButtonClicked;


        public ResultsPanel()
        {
            InitializeComponent();

            input_points_list_window = new ListWindow();
            input_points_list_window.Owner = this.Parent as Window;
            input_points_list_window.Title = "Input points list";
        }

        private void DisplayExecutionTimeDetails(object sender, RoutedEventArgs e) 
        {

        }

        private void DisplayInputPointsList(object sender, RoutedEventArgs e)
        {
            input_points_list_window.Show();
        }

        private void ReturnToEditing(object sender, RoutedEventArgs e) 
        {
            ReturnButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        public void SetHullPointsList(ConvexHullApp.Point[] points)  
        {
            foreach(var point in points) 
            {
                TextBlock newItem = new TextBlock
                {
                    Text = (point.X).ToString().PadRight(21) + " " + (point.Y).ToString().PadRight(21),
                    FontSize = 18,
                    Margin = new Thickness(5)
                };

                HullPointsListStackPanel.Children.Add(newItem);
            }
        }

        public void SetInputPoints(ConvexHullApp.Point[] points) 
        {
            foreach (var point in points) 
            {
                input_points_list_window.AddToList(point);
            }
        }

        public void SetAlgorithmType(AlgorithmType alg) 
        {
            AlgorithmTypeDisplay.Text = alg.ToString();
        }

        public void SetHullShape(string shape) 
        {
            HullShapeDisplay.Text = shape;
        }
        public void SetAvgExecTime(double time) 
        {
            ExecAvgTime.Text = time.ToString()+"[ms]";
        }

        public void ClearResults() 
        {
            HullPointsListStackPanel.Children.Clear();
            input_points_list_window.ClearList();
        }

        public void Hide()
        {
            ThisPanel.Visibility = Visibility.Collapsed;
            input_points_list_window.Hide();
        }

        public void Show()
        {
            ThisPanel.Visibility = Visibility.Visible;
        }
    }
}
