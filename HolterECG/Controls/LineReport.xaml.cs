using LiveCharts;
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

namespace HolterECG.Controls
{
    /// <summary>
    /// Interaction logic for LineReport.xaml
    /// </summary>
    public partial class LineReport : UserControl
    {
        State _state;
        SeriesCollection _seriesSystolic;
        public LineReport()
        {
            InitializeComponent();
            _state = Application.Current.FindResource("state") as State;
            _seriesSystolic = new SeriesCollection();
            chartSys.Series = _seriesSystolic;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
