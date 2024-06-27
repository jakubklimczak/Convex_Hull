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
        results_panel.ReturnButtonClicked += ReturnToEdit!;
    }

    private void CleanCanvasDelegate(object sender, EventArgs args) 
    {
        points_chart_panel.RemoveAllPoints();
        points_chart_panel.Unlock();
    }

    private void RunAlgorithmDelegate(object sender, RunAlgorithmEventArgs args) 
    {
        Stopwatch stopwatch = new Stopwatch();

        var points = points_chart_panel.GetPointsList();
        var result = new Result([],"");
        Func<Point[], Result>? AlgorithmFunction = args.AlgType switch
        {
            AlgorithmType.Jarvis => ConvexHullAlgorithms.JarvisHullAlgorithm,
            AlgorithmType.Graham => ConvexHullAlgorithms.GrahamScan,
            _ => throw new Exception("Unkown Algorithm type!"),
        };

        stopwatch.Start();
        for (int i = 0; i < args.NumberOfIterattions; i++)
        {
            result = AlgorithmFunction(points);
        }
        stopwatch.Stop();


        points_chart_panel.AddHull(result.Points);
        results_panel.SetHullPointsList(result.Points);
        results_panel.SetInputPoints(points);
        results_panel.SetAlgorithmType(args.AlgType);
        results_panel.SetHullShape(result.Shape);

        TimeSpan elapsedTime = stopwatch.Elapsed;
        var avg_time = elapsedTime.TotalMilliseconds / args.NumberOfIterattions;
        results_panel.SetAvgExecTime(avg_time);

        SwitchToResultsMode();
    }
    private void RandomizePointsDelegate(object sender, RandomizePointsEventArgs args)
    {
        var limits = points_chart_panel.GetAxisLimits();

        try
        {
            for(int i = 0;i < args.AmountOfPoints; i++) 
            {
                var point = CoordRandomizer.GetRandomCoordinates(limits.Left, limits.Right, limits.Bottom, limits.Top);
                points_chart_panel.AddNewPoint(point.X, point.Y);
            }
        }
        catch (Exception ex) 
        {
            MessageBox.Show(ex.Message);
        }

    }

    private void ReturnToEdit(object sender, EventArgs args) 
    {
        points_chart_panel.ClearHull();
        SwitchToEditMode();
        results_panel.ClearResults();
    }

    private void SwitchToResultsMode() 
    {
        points_chart_panel.Lock();
        run_control_panel.Hide();
        results_panel.Show();
    }

    private void SwitchToEditMode()
    {
        points_chart_panel.Unlock();
        run_control_panel.Show();
        results_panel.Hide();
    }

}