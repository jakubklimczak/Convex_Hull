using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvexHullApp
{
    public class RandomizePointsEventArgs(int _AmountOfPoints) : EventArgs
    {
        public int AmountOfPoints { get; set; } = _AmountOfPoints;
    }

    public enum AlgorithmType 
    {
        Graham,
        Jarvis
    }

    public class RunAlgorithmEventArgs : EventArgs 
    {
        public AlgorithmType AlgType { get; set; }
        public int NumberOfIterattions { get; set; }

        public RunAlgorithmEventArgs(AlgorithmType _AlgType, int _NumberOfIterattions) 
        {
            AlgType = _AlgType;
            NumberOfIterattions = _NumberOfIterattions;
        }
    }

    /// <summary>
    /// Interaction logic for class RunControlPanel.xaml
    /// </summary>
    public partial class RunControlPanel : UserControl
    {
        public event EventHandler? CleanCavasButtonClicked;
        public event EventHandler<RandomizePointsEventArgs>? RandomizePointsButtonClicked;
        public event EventHandler<RunAlgorithmEventArgs>? RunAlgorithmButtonClicked;

        public RunControlPanel()
        {
            InitializeComponent();
        }

        private void CleanCanvasCallback(object sender, RoutedEventArgs e)
        {
            CleanCavasButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void RandomizePointsCallback(object sender, RoutedEventArgs e) 
        {
            try
            {
                TextBox RandomPointsText = (TextBox)FindName("RandomPointsTextBox");
                int AmountOfPoints = GetNumberFromTextBox(RandomPointsText);
                RandomizePointsButtonClicked?.Invoke(this, new RandomizePointsEventArgs(AmountOfPoints));
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void RunAlgorithmCallback(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox IterrationsText = (TextBox)FindName("IterrationsTextBox");
                int AmountOfIterrations = GetNumberFromTextBox(IterrationsText);


                RunAlgorithmButtonClicked?.Invoke(this, new RunAlgorithmEventArgs(GetAlgorithmType(), AmountOfIterrations));
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            

        }
        //This function can throw if incorrect string is inserted
        private static int GetNumberFromTextBox(TextBox TextBox)
        {
            int NumberValue = Int32.Parse(TextBox.Text);
            return NumberValue;
        }
        private void PositiveNumbersTextBoxValidation(object sender, TextCompositionEventArgs e)
        {
            TextBox? textbox = (sender as TextBox);
            var TextValue = textbox!.Text.Insert(textbox.CaretIndex,e.Text);
            e.Handled = !IsTextPositiveNumber(TextValue);
        }

        //This function assumes 0 is not positive
        private static bool IsTextPositiveNumber(string text)
        {
            Regex regex = new("^[1-9][0-9]*$");
            return regex.IsMatch(text);
        }

        private AlgorithmType GetAlgorithmType()
        {
            var CheckedRadioButton = AlgorithmPanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) ?? throw new ArgumentNullException(message: "No algorithm has been selected", null);
            string? content = CheckedRadioButton.Content.ToString();
            return content switch
            {
                "Graham" => AlgorithmType.Graham,
                "Jarvis" => AlgorithmType.Jarvis,
                _ => throw new ArgumentException("Undefined type of algorithm has been selected"),
            };
        }
        public void Hide()
        {
            ThisPanel.Visibility = Visibility.Collapsed;
        }

        public void Show()
        {
            ThisPanel.Visibility = Visibility.Visible;
        }

    }
}
