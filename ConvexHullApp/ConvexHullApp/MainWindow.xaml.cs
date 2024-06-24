﻿using System.Diagnostics;
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
        Func<Point[],Result> AlgorithmFunction = null;

        switch (args.AlgType) 
        {
            case AlgorithmType.Jarvis:
                AlgorithmFunction = ConvexHullAlgorithms.JarvisHullAlgorithm;
                break;

            case AlgorithmType.Graham:
                throw new NotImplementedException("Jeśli to widzisz to znaczy, że Miłosz nie zaimplementował tego alogorytmu");
                AlgorithmFunction = ConvexHullAlgorithms.JarvisHullAlgorithm;
                break;

            default:
                throw new Exception("Unkown Algorithm type!");
        }

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
        MessageBox.Show("Losuj punkty, argumenty: ilość punktów: " + args.AmountOfPoints);
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