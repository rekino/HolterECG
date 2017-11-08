using LiveCharts;
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
    class Range:System.ComponentModel.INotifyPropertyChanged
    {
        public bool Contains(double value)
        {
            return value >= _low && value < _high;
        }

        private double _low;

        public double Low
        {
            get { return _low; }
            set
            {
                _low = value;
                OnPropertyChanged("Low");
            }
        }
        private double _high;

        public double High
        {
            get { return _high; }
            set
            {
                _high = value;
                OnPropertyChanged("Hight");
            }
        }
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    class PieReportModel : System.ComponentModel.INotifyPropertyChanged
    {
        State _state;
        public PieReportModel()
        {
            this._state = Application.Current.FindResource("state") as State;
            this.SliceLabel = point => string.Format("{0} ({1:P})", point.SeriesView.Title, point.Participation);

            this.Systolic = new SeriesCollection();
            this.Diastolic = new SeriesCollection();
            this.HeartRate = new SeriesCollection();

            this.SystolicRanges = new ObservableCollection<Range>();
            this.SystolicRanges.Add(new Range { Low = 50, High = 80, Title="Under Normal" });
            this.SystolicRanges.Add(new Range { Low = 80, High = 110, Title = "Normal" });
            this.SystolicRanges.Add(new Range { Low = 110, High = 140, Title = "High Normal" });
            this.SystolicRanges.Add(new Range { Low = 140, High = 170, Title = "Grade I" });
            this.SystolicRanges.Add(new Range { Low = 170, High = 190, Title = "Grade II" });
            this.SystolicRanges.Add(new Range { Low = 190, High = 210, Title = "Grade III" });

            this.DiastolicRanges = new ObservableCollection<Range>();
            this.DiastolicRanges.Add(new Range { Low = 50, High = 80, Title = "Under Normal" });
            this.DiastolicRanges.Add(new Range { Low = 80, High = 110, Title = "Normal" });
            this.DiastolicRanges.Add(new Range { Low = 110, High = 140, Title = "High" });

            this.HeartRateRanges = new ObservableCollection<Range>();
            this.HeartRateRanges.Add(new Range { Low = 50, High = 80, Title = "Low" });
            this.HeartRateRanges.Add(new Range { Low = 80, High = 110, Title = "Normal" });
            this.HeartRateRanges.Add(new Range { Low = 110, High = 140, Title = "High Normal" });
            this.HeartRateRanges.Add(new Range { Low = 140, High = 170, Title = "High" });

            double[] syss = _state.ActivePatient.GetSystolicReadings();
            double[] dias = _state.ActivePatient.GetDiastolicReadings();
            double[] hrs = _state.ActivePatient.GetHeartRateReadings();

            this.Systolic.AddRange(CreatePieChart(syss, SystolicRanges));
            this.Diastolic.AddRange(CreatePieChart(dias, DiastolicRanges));
            this.HeartRate.AddRange(CreatePieChart(hrs, HeartRateRanges));
        }

        PieSeries[] CreatePieChart(IEnumerable<double> data, IEnumerable<Range> ranges)
        {
            List<PieSeries> series = new List<PieSeries>();

            int[] counter = new int[1];
            foreach (var range in ranges)
            {
                counter[0] = 0;
                foreach (var reading in data)
                {
                    if (range.Contains(reading))
                    {
                        counter[0]++;
                    }
                }
                series.Add(new PieSeries { Title = range.Title, Values = new ChartValues<int>(counter), DataLabels = true, LabelPoint=this.SliceLabel });
            }
            return series.ToArray();
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
        public Func<ChartPoint, string> SliceLabel { get; private set; }

        public ObservableCollection<Range> SystolicRanges { get; private set; }
        public ObservableCollection<Range> DiastolicRanges { get; private set; }
        public ObservableCollection<Range> HeartRateRanges { get; private set; }
    }
    /// <summary>
    /// Interaction logic for PieReport.xaml
    /// </summary>
    public partial class PieReport : UserControl
    {
        public PieReport()
        {
            InitializeComponent();
        }
    }
}
