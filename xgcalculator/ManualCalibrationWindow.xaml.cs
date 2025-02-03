using System;
using System.Windows;

namespace XGCalculator
{
    public partial class ManualCalibrationWindow : Window
    {
        public double GrandeLeft { get; set; }
        public double GrandeTop { get; set; }
        public double GrandeRight { get; set; }
        public double GrandeBottom { get; set; }

        public ManualCalibrationWindow()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtGrandeLeft.Text, out double left) &&
                double.TryParse(txtGrandeTop.Text, out double top) &&
                double.TryParse(txtGrandeRight.Text, out double right) &&
                double.TryParse(txtGrandeBottom.Text, out double bottom))
            {
                GrandeLeft = left;
                GrandeTop = top;
                GrandeRight = right;
                GrandeBottom = bottom;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Por favor, insira valores numéricos válidos para todas as coordenadas.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
