using OpenTK.Windowing.Common;
using ScottPlot;
using ScottPlot.AxisLimitManagers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
    public enum ChartPanelMode
    {
        Default,
        Add,
        Remove
    }
    /// <summary>
    /// Logika interakcji dla klasy ChartPnale.xaml
    /// </summary>
    public partial class ChartPanel : UserControl
    {
        private ChartPanelMode current_mode;
        private List<Coordinates> coordinates_list;
        private ScottPlot.Plottables.Scatter point_scatter;
        private ScottPlot.Plottables.Marker highlight_marker;
        private ScottPlot.Plottables.Polygon hull;
        private List<ScottPlot.Plottables.Text> coordinates_text_list;

        private bool is_locked;
        public ChartPanel()
        {
            InitializeComponent();

            coordinates_list = new List<Coordinates>();
            coordinates_text_list = new List<ScottPlot.Plottables.Text>();
            point_scatter = PointChart.Plot.Add.Scatter(coordinates_list);
            point_scatter.LineWidth = 0;
            point_scatter.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Black);
            
            highlight_marker = PointChart.Plot.Add.Marker(0, 0);
            highlight_marker.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Red);
            highlight_marker.MarkerSize = 10;
            highlight_marker.MarkerShape = ScottPlot.MarkerShape.OpenCircle;
            highlight_marker.IsVisible = false;

            PointChart.Refresh();
            Unlock();
        }

        private void ChartMouseClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Point current_position = e.GetPosition(this);
            ScottPlot.Pixel current_pixel = new ScottPlot.Pixel(current_position.X, current_position.Y);
            ScottPlot.Coordinates current_coordinates = PointChart.Plot.GetCoordinates(current_pixel);

            switch (current_mode)
            {
                default:
                case ChartPanelMode.Default:
                break;

                case ChartPanelMode.Add:
                    AddNewPoint(current_coordinates);
                break;

                case ChartPanelMode.Remove:
                    RemoveHighlightedPoint();
                break;
            }

        }

        private void OnKeyDown(object sender, KeyEventArgs e) 
        {
            ModifierKeys modifiers = Keyboard.Modifiers;

            if (is_locked) 
            {
                ChangeCurrentMode(ChartPanelMode.Default);
                return;
            }

            if (modifiers == ModifierKeys.Control)
            {
                ChangeCurrentMode(ChartPanelMode.Add);
            }
            else if (modifiers == ModifierKeys.Shift)
            {
                ChangeCurrentMode(ChartPanelMode.Remove);
            }
            else 
            {
                ChangeCurrentMode(ChartPanelMode.Default);
            }
        }

        private void OnMouseMoved(object sender, MouseEventArgs e) 
        {
            System.Windows.Point current_position = e.GetPosition(this);
            ScottPlot.Pixel current_pixel = new ScottPlot.Pixel(current_position.X, current_position.Y);
            ScottPlot.Coordinates mouse_position = PointChart.Plot.GetCoordinates(current_pixel);

            DataPoint nearest_point = point_scatter.Data.GetNearest(mouse_position, PointChart.Plot.LastRender);

            if (nearest_point.IsReal) 
            {
                MoveHighlightMarker(nearest_point.Coordinates);
            }
            else 
            {
                HideHighLightMarker();
            }
        }

        private void showGridCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ShowGrid();
        }

        private void showGridCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            HideGrid();
        }

        private void showCoordiantesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ShowCoordinatesText();
        }

        private void showCoordiantesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            HideCoordinatesText();
        }

        private void ChangeCurrentMode(ChartPanelMode new_mode)
        {
            current_mode = new_mode;

            switch (current_mode) 
            {
                case ChartPanelMode.Default:
                    PointChart.Cursor = Cursors.Arrow;
                break;

                case ChartPanelMode.Add:
                    PointChart.Cursor = Cursors.Cross;
                break;

                case ChartPanelMode.Remove:
                    PointChart.Cursor = Cursors.No;
                break;

                default:
                    PointChart.Cursor = Cursors.Arrow;
                break;
            }
            
        }

        public void Lock() 
        {
            is_locked = true;
            PointChart.Plot.DataBackground.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Gray);
        }

        public void Unlock() 
        {
            is_locked = false;
            PointChart.Plot.DataBackground.Color = ScottPlot.Color.FromColor(System.Drawing.Color.White);
        }

        public void HideCoordinatesText() 
        {
            if (coordinates_text_list == null)
                return;

            foreach (var text in coordinates_text_list) 
            {
                text.IsVisible = false;
            }
            PointChart.Refresh();
        }

        public void ShowCoordinatesText() 
        {
            if (coordinates_text_list == null)
                return;

            foreach (var text in coordinates_text_list)
            {
                text.IsVisible = true;
            }
            PointChart.Refresh();
        }

        public void ShowGrid() 
        {
            PointChart.Plot.ShowGrid(); 
            PointChart.Refresh();
        }

        public void HideGrid() 
        {
            PointChart.Plot.HideGrid();
            PointChart.Refresh();
        }

        public void AddNewPoint(ScottPlot.Coordinates _coordinates)
        {
            coordinates_list.Add(_coordinates);
            string coordinates_text = GetTextFromCoordinates(_coordinates);
            coordinates_text_list.Add(PointChart.Plot.Add.Text(coordinates_text, _coordinates));
            PointChart.Refresh();
        }

        public void AddNewPoint(double x, double y)
        {
            AddNewPoint(new Coordinates(x, y));
        }

        public ConvexHullApp.Point[] GetPointsList() 
        {
            ConvexHullApp.Point[] points_array = new Point[coordinates_list.Count];
            for (int i = 0; i < coordinates_list.Count; i++) 
            {
                points_array[i] = new Point { X = (int)(coordinates_list[i].X), Y = (int)(coordinates_list[i].Y) };
            }
            return points_array;
        }

        public void RemoveHighlightedPoint() 
        {
            coordinates_list.Remove(highlight_marker.Location);
            RemovePointsText(highlight_marker.Location);
            HideHighLightMarker();
            PointChart.Refresh();
        }


        //This whole method is just very very bad hack but I have no effort to do it better
        private void RemovePointsText(Coordinates coord) 
        {
            string coordinates_text = GetTextFromCoordinates(coord);
            Debug.WriteLine(coordinates_text);

            var text_plottables = PointChart.Plot.GetPlottables<ScottPlot.Plottables.Text>();
            for (int i = 0; i < text_plottables.Count(); i++)
            {
                var item = text_plottables.ElementAt(i);
                Debug.WriteLine(item.LabelText);

                if (item.LabelText == coordinates_text && item.Location == coord)
                {
                    PointChart.Plot.Remove(item);
                }
            }
        }

        private string GetTextFromCoordinates(Coordinates coord) 
        {
            return Math.Round(coord.X, 2) + " " + Math.Round(coord.Y, 2);
        }

        public void RemoveAllPoints() 
        {
            coordinates_list.Clear();
            RemoveAllCoordinatesTexts();
            HideHighLightMarker();
            PointChart.Refresh();
        }

        public void AddHull(ConvexHullApp.Point[] points_array) 
        {
            List<Coordinates> points = new List<Coordinates>();

            foreach (var point in points_array)
            {
                points.Add(new Coordinates(point.X, point.Y));
            }

            hull = PointChart.Plot.Add.Polygon(points.ToArray());
            hull.FillColor = ScottPlot.Color.FromColor(System.Drawing.Color.Transparent);
            hull.LineColor = ScottPlot.Color.FromColor(System.Drawing.Color.Blue);
            PointChart.Refresh();
        }

        public void ClearHull() 
        {
            PointChart.Plot.Remove(hull);
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e) 
        {
            PointChart.Focus();
        }

        private void MoveHighlightMarker(ScottPlot.Coordinates _coordinates) 
        {
            highlight_marker.IsVisible = true;
            highlight_marker.Location = _coordinates;
            PointChart.Refresh();
        }
        private void HideHighLightMarker()
        {
            highlight_marker.IsVisible = false;
            PointChart.Refresh();
        }

        private void RemoveAllCoordinatesTexts() 
        {
            foreach(var text in coordinates_text_list) 
            {
                PointChart.Plot.Remove(text);
            }

            coordinates_text_list = new List<ScottPlot.Plottables.Text>();
        }
    }

}
