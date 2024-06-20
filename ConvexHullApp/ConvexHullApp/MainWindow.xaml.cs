using System.Diagnostics;
using System.Windows;

namespace ConvexHullApp;

/*
 * <summary>
 * Interaction logic for MainWindow.xaml
 * </summary>
 */
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        run_control_panel.CleanCavasButtonClicked += CleanCanvasDelegate!;
        run_control_panel.RunAlgorithmButtonClicked += RunAlgorithmDelegate!;
        run_control_panel.RandomizePointsButtonClicked += RandomizePointsDelegate!;
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
        var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);
        points_chart_panel.AddHull(result.Points);
        //FIXME: Utilise the result.Shape message - display it somewhere in the GUI
    }
    private void RandomizePointsDelegate(object sender, RandomizePointsEventArgs args)
    {
        MessageBox.Show("Losuj punkty, argumenty: ilość punktów: " + args.AmountOfPoints);
        Debug.WriteLine("clean");
    }

}