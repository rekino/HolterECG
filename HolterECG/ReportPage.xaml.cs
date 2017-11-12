using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace HolterECG
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        State _state;
        SeriesCollection _line;
        public ReportPage()
        {
            InitializeComponent();
            _state = Application.Current.FindResource("state") as State;
            _state.PropertyChanged += _state_PropertyChanged;
            _line = new SeriesCollection();

        }

        void _state_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActivePatient")
            {

            }
        }

        private void btnTextReport_Click(object sender, RoutedEventArgs e)
        {
            _state.Home.ActiveReport = ReportType.Text;
        }

        private void btnLineReport_Click(object sender, RoutedEventArgs e)
        {
            _state.Home.ActiveReport = ReportType.Line;
        }

        private void btnBarReport_Click(object sender, RoutedEventArgs e)
        {
            _state.Home.ActiveReport = ReportType.Bar;
        }

        private void btnPieReport_Click(object sender, RoutedEventArgs e)
        {
            _state.Home.ActiveReport = ReportType.Pie;
        }

        private void btnAnalysisReport_Click(object sender, RoutedEventArgs e)
        {
            _state.Home.ActiveReport = ReportType.Analysis;
        }
    }
}
