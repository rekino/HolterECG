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
        private Brush _fill;

        public Brush Fill
        {
            get { return _fill; }
            set { _fill = value; }
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

            this._sysMin = 80;
            this._sysMax = 130;
            this._diaMin = 70;
            this._diaMax = 90;
            this._hrMin = 60;
            this._hrMax = 90;

            this.PropertyChanged += PieReportModel_PropertyChanged;

            this.DrawSysSeries();
            this.DrawDiaSeries();
            this.DrawHrSeries();
        }

        public void SetSysDefaults()
        {
            this.SysMin = 80;
            this.SysMax = 130;
        }
        public void SetDiaDefaults()
        {
            this.DiaMin = 70;
            this.DiaMax = 90;
        }
        public void SetHrDefaults()
        {
            this.HrMin = 60;
            this.HrMax = 90;
        }
        void PieReportModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SysMax" || e.PropertyName == "SysMin")
                this.DrawSysSeries();
            if (e.PropertyName == "DiaMax" || e.PropertyName == "DiaMin")
                this.DrawDiaSeries();
            if (e.PropertyName == "HrMax" || e.PropertyName == "HrMin")
                this.DrawHrSeries();
        }

        void DrawSysSeries()
        {
            List<Range> ranges = new List<Range>();
            ranges.Add(new Range { Title = "Low", Low = 0, High = this.SysMin, Fill = Brushes.Blue });
            ranges.Add(new Range { Title = "Normal", Low = this.SysMin, High = this.SysMax, Fill = Brushes.Green });
            ranges.Add(new Range { Title = "High", Low = this.SysMax, High = double.MaxValue, Fill = Brushes.Red });
            double[] syss = _state.ActivePatient.GetSystolicReadings();
            var series = CreatePieChart(syss, ranges);
            this.Systolic.Clear();
            this.Systolic.AddRange(series);
        }
        void DrawDiaSeries()
        {
            this.Diastolic.Clear();
            List<Range> ranges = new List<Range>();
            ranges.Add(new Range { Title = "Low", Low = 0, High = this.DiaMin, Fill = Brushes.Blue });
            ranges.Add(new Range { Title = "Normal", Low = this.DiaMin, High = this.DiaMax, Fill = Brushes.Green });
            ranges.Add(new Range { Title = "High", Low = this.DiaMax, High = double.MaxValue, Fill = Brushes.Red });
            double[] dias = _state.ActivePatient.GetDiastolicReadings();
            this.Diastolic.AddRange(CreatePieChart(dias, ranges));
        }
        void DrawHrSeries()
        {
            this.HeartRate.Clear();
            List<Range> ranges = new List<Range>();
            ranges.Add(new Range { Title = "Low", Low = 0, High = this.HrMin, Fill = Brushes.Blue });
            ranges.Add(new Range { Title = "Normal", Low = this.HrMin, High = this.HrMax, Fill = Brushes.Green });
            ranges.Add(new Range { Title = "High", Low = this.HrMax, High = double.MaxValue, Fill = Brushes.Red });
            double[] hrs = _state.ActivePatient.GetHeartRateReadings();
            this.HeartRate.AddRange(CreatePieChart(hrs, ranges));
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
                series.Add(new PieSeries { Title = range.Title, Values = new ChartValues<int>(counter), DataLabels = true, LabelPoint=this.SliceLabel, Fill=range.Fill });
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
        private double _sysMax;

        public double SysMax
        {
            get { return _sysMax; }
            set { _sysMax = value; OnPropertyChanged("SysMax"); }
        }
        private double _sysMin;

        public double SysMin
        {
            get { return _sysMin; }
            set { _sysMin = value; OnPropertyChanged("SysMin"); }
        }
        private double _diaMax;

        public double DiaMax
        {
            get { return _diaMax; }
            set { _diaMax = value; OnPropertyChanged("DiaMax"); }
        }
        private double _diaMin;

        public double DiaMin
        {
            get { return _diaMin; }
            set { _diaMin = value; OnPropertyChanged("DiaMin"); }
        }
        private double _hrMax;

        public double HrMax
        {
            get { return _hrMax; }
            set { _hrMax = value; OnPropertyChanged("HrMax"); }
        }
        private double _hrMin;

        public double HrMin
        {
            get { return _hrMin; }
            set { _hrMin = value; OnPropertyChanged("HrMin"); }
        }

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

        private void setSysDefaults(object sender, RoutedEventArgs e)
        {
            (grdTop.DataContext as PieReportModel).SetSysDefaults();
        }

        private void setDiaDefaults(object sender, RoutedEventArgs e)
        {
            (grdTop.DataContext as PieReportModel).SetDiaDefaults();
        }

        private void setHrDefaults(object sender, RoutedEventArgs e)
        {
            (grdTop.DataContext as PieReportModel).SetHrDefaults();
        }
    }
}
