using HolterECG.DataStructures;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            this.YFormatter = value => String.Format("{0:P}", value);

            var bucketSize = 10;
            double[] syss = _state.ActivePatient.GetSystolicReadings();
            double[] dias = _state.ActivePatient.GetDiastolicReadings();
            double[] hrs = _state.ActivePatient.GetHeartRateReadings();
            List<string> xlabels = new List<string>();
            int xStart = 50;
            while (xStart <= 210)
            {
                xlabels.Add(xStart.ToString());
                xStart += bucketSize;
            }
            this.XLabels = xlabels.ToArray();

            var totalBuckets = 17;
            Systolic.Add(CreateHistogram(syss, totalBuckets, "Systolic Blood Pressure", Brushes.Red));
            Diastolic.Add(CreateHistogram(dias, totalBuckets, "Diastolic Blood Pressure", Brushes.Green));
            HeartRate.Add(CreateHistogram(hrs, totalBuckets, "Heart Rate", Brushes.Blue));
        }
        ColumnSeries CreateHistogram(IEnumerable<double> data, int totalBuckets, string title, Brush fill)
        {
            var min = 45;
            var max = 215;
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


            ColumnSeries series = new ColumnSeries
            {
                Title = title,
                Fill = fill,
                DataLabels = true,
                LabelPoint = point => point.Y != 0 ? String.Format("{0:P}", point.Y) : "",
                MaxColumnWidth = double.PositiveInfinity,
                ColumnPadding = 1
            };
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
        public string[] XLabels { get; private set; }

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
