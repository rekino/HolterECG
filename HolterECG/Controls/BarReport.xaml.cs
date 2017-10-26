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
    class BarReportModel:System.ComponentModel.INotifyPropertyChanged
    {
        State _state;
        public BarReportModel()
        {
            this._state = Application.Current.FindResource("state") as State;

            this.Systolic = new SeriesCollection();
            this.Diastolic = new SeriesCollection();
            this.HeartRate = new SeriesCollection();

            this.YFormatter = value => String.Format("%{0}", value * 100);

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

            Systolic.Add(CreateHistogram(syss, totalBuckets, "Systolic Blood Pressure", Brushes.Red));
            Diastolic.Add(CreateHistogram(dias, totalBuckets, "Diastolic Blood Pressure", Brushes.Green));
            HeartRate.Add(CreateHistogram(hrs, totalBuckets, "Heart Rate", Brushes.Blue));
        }
        ColumnSeries CreateHistogram(IEnumerable<double> data, int totalBuckets, string title, Brush fill)
        {
            var min = data.Min();
            var max = data.Max();
            var buckets = new double[totalBuckets];
            var unit = 1.0 / data.Count();

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
                buckets[bucketIndex] += unit;
            }
            ColumnSeries series = new ColumnSeries { Title = title, Fill = fill };
            series.Values = new ChartValues<double>(buckets);
            return series;
        }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        public SeriesCollection Systolic { get; private set; }
        public SeriesCollection Diastolic { get; private set; }
        public SeriesCollection HeartRate { get; private set; }
        public Func<double, string> YFormatter { get; private set; }
    }
    /// <summary>
    /// Interaction logic for BarReport.xaml
    /// </summary>
    public partial class BarReport : UserControl
    {
        public BarReport()
        {
            InitializeComponent();
        }
    }
}
