using System.ComponentModel.Design;
using System.Diagnostics;
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
    }

    private void CleanCanvasCallback(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("clean");
    }
}