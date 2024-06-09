using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
    }

    private void RunAlgorithmDelegate(object sender, RunAlgorithmEventArgs args) 
    {
        MessageBox.Show("Uruchom, argumenty: typ algorytmu: "+args.AlgType+" ilość powtórzeń: "+args.NumberOfIterattions);
    }

    private void RandomizePointsDelegate(object sender, RandomizePointsEventArgs args) 
    {
        MessageBox.Show("Losuj punkty, argumenty: ilość punktów: " + args.AmountOfPoints);
    }
}