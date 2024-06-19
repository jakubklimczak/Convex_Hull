using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    public class RandomizePointsEventArgs : EventArgs
    {
        public int AmountOfPoints { get; set; }

        public RandomizePointsEventArgs(int _AmountOfPoints)
        {
            AmountOfPoints = _AmountOfPoints;
        }
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
    /// Logika interakcji dla klasy RunControlPanel.xaml
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
            Debug.WriteLine("clean");
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
        private int GetNumberFromTextBox(TextBox TextBox) 
        {
            int NumberValue = Int32.Parse(TextBox.Text);
            return NumberValue;
        }
        //FIXME: this function is not triggered when spacebar or deletion is performed
        private void PositiveNumbersTextBoxValidation(object sender, TextCompositionEventArgs e)
        {
            TextBox? textbox = (sender as TextBox);
            var TextValue = textbox!.Text.Insert(textbox.CaretIndex,e.Text);
            e.Handled = !IsTextPositiveNumber(TextValue);
        }

        //This function assumes 0 is not positive
        private bool IsTextPositiveNumber(string text)
        {
            Regex regex = new Regex("^[1-9][0-9]*$");
            return regex.IsMatch(text);
        }

        private AlgorithmType GetAlgorithmType()
        {
            var CheckedRadioButton = AlgorithmPanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            if (CheckedRadioButton == null)
                throw new ArgumentNullException(message:"No algorithm has been selected",null);

            string? content = CheckedRadioButton.Content.ToString();
            switch (content) 
            {
                case "Graham":
                    return AlgorithmType.Graham;

                case "Jarvis":
                    return AlgorithmType.Jarvis;

                default:
                    throw new ArgumentException("Undefined type of algorithm has been selected");
            }

        }
    }
}
