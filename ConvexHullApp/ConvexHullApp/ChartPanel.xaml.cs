using OpenTK.Windowing.Common;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ChartPanel()
        {
            InitializeComponent();

            coordinates_list = new List<Coordinates>();
            point_scatter = PointChart.Plot.Add.Scatter(coordinates_list);
            point_scatter.LineWidth = 0;
            point_scatter.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Black);
            
            highlight_marker = PointChart.Plot.Add.Marker(0, 0);
            highlight_marker.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Red);
            highlight_marker.MarkerSize = 10;
            highlight_marker.MarkerShape = ScottPlot.MarkerShape.OpenCircle;
            highlight_marker.IsVisible = false;

            PointChart.Refresh();
        }

        private void ChartMouseClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point current_position = e.GetPosition(this);
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
            Point current_position = e.GetPosition(this);
            ScottPlot.Pixel current_pixel = new ScottPlot.Pixel(current_position.X, current_position.Y);
            ScottPlot.Coordinates mouse_position = PointChart.Plot.GetCoordinates(current_pixel);

            DataPoint nearest_point = point_scatter.Data.GetNearest(mouse_position, PointChart.Plot.LastRender);

            Debug.WriteLine(nearest_point.X + " " + nearest_point.Y);
            Debug.WriteLine(point_scatter.Data.GetScatterPoints().Count);

            if (nearest_point.IsReal) 
            {
                MoveHighlightMarker(nearest_point.Coordinates);
            }
            else 
            {
                HideHighLightMarker();
            }
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

        public void AddNewPoint(ScottPlot.Coordinates _coordinates)
        {
            coordinates_list.Add(_coordinates);
            PointChart.Refresh();
        }

        public void AddNewPoint(double x, double y)
        {
            coordinates_list.Add(new Coordinates(x, y));
            PointChart.Refresh();
        }

        public void RemoveHighlightedPoint() 
        {
            coordinates_list.Remove(highlight_marker.Location);
            HideHighLightMarker();
            PointChart.Refresh();
        }

        public void RemoveAllPoints() 
        {
            coordinates_list.Clear();
            HideHighLightMarker();
            PointChart.Refresh();
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
    }

}
