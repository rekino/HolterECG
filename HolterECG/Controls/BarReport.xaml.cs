using HolterECG.DataStructures;
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

namespace HolterECG.Controls
{
    /// <summary>
    /// Interaction logic for BarReport.xaml
    /// </summary>
    public partial class BarReport : UserControl
    {
        State _state;
        SeriesCollection _seriesSystolic;
        SeriesCollection _seriesDiastolic;
        SeriesCollection _seriesHeartRate;
        public BarReport()
        {
            InitializeComponent();
            _state = Application.Current.FindResource("state") as State;
            _seriesSystolic = new SeriesCollection();
            _seriesDiastolic = new SeriesCollection();
            _seriesHeartRate = new SeriesCollection();
            chartSys.Series = _seriesSystolic;
            chartDia.Series = _seriesDiastolic;
            chartHr.Series = _seriesHeartRate;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var totalBuckets = 10;
            List<double> syss = new List<double>();
            List<double> dias = new List<double>();
            List<double> hrs = new List<double>();

            foreach (Reading reading in _state.ActivePatient.Readings)
            {
                syss.Add(reading.Sys);
                dias.Add(reading.Dia);
                hrs.Add(reading.HR);
            }

            _seriesSystolic.Add(CreateHistogram(syss, totalBuckets, "Systolic Blood Pressure"));
            _seriesDiastolic.Add(CreateHistogram(dias, totalBuckets, "Diastolic Blood Pressure"));
            _seriesHeartRate.Add(CreateHistogram(hrs, totalBuckets, "Heart Rate"));
        }

        ColumnSeries CreateHistogram(IEnumerable<double> data, int totalBuckets, string title)
        {
            var min = data.Min();
            var max = data.Max();
            var buckets = new double[totalBuckets];
            var unit = 100.0 / data.Count();

            var bucketSize = (max - min) / totalBuckets;
            foreach (var value in data)
            {
                int bucketIndex = 0;
                if (bucketSize > 0.0)
                {
                    bucketIndex = (int)((value - min) / bucketSize);
                    if (bucketIndex == totalBuckets)
                    {
                        bucketIndex--;
                    }
                }
                buckets[bucketIndex] += unit ;
            }
            ColumnSeries series = new ColumnSeries { Title = title };
            series.Values = new ChartValues<double>(buckets);
            return series;
        }
    }
}
