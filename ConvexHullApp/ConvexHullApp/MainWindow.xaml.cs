using ScottPlot.WPF;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ConvexHullApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        run_control_panel.CleanCavasButtonClicked += CleanCanvasDelegate;
        run_control_panel.RunAlgorithmButtonClicked += RunAlgorithmDelegate;
        run_control_panel.RandomizePointsButtonClicked += RandomizePointsDelegate;
    }

    private void CleanCanvasDelegate(object sender, EventArgs args) 
    {
        MessageBox.Show("Wyczyść");
        //FIXME: Maybe we should add posibility to clear just the hull without points
        points_chart_panel.ClearHull();
        points_chart_panel.RemoveAllPoints();
        points_chart_panel.Unlock();
    }

    private void RunAlgorithmDelegate(object sender, RunAlgorithmEventArgs args) 
    {
        points_chart_panel.Lock();
        MessageBox.Show("Uruchom, argumenty: typ algorytmu: "+args.AlgType+" ilość powtórzeń: "+args.NumberOfIterattions);
        var points = points_chart_panel.GetPointsList();

        //var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);

        foreach (var point in points)
        {
            Debug.WriteLine(point.X.ToString());
            Debug.WriteLine(point.Y.ToString());
        }

        points_chart_panel.AddHull(points);
    }
    private void RandomizePointsDelegate(object sender, RandomizePointsEventArgs args)
    {
        MessageBox.Show("Losuj punkty, argumenty: ilość punktów: " + args.AmountOfPoints);
        Debug.WriteLine("clean");
    }

}