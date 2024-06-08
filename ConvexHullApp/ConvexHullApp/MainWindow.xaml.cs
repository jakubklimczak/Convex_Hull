using System.ComponentModel.Design;
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

    private void TestClick(object sender, RoutedEventArgs e)
    {
        Button test = (Button)sender;
        test.Background = Brushes.Maroon;
    }
}