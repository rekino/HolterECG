using LiveCharts;
using LiveCharts.Defaults;
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
    class LineReportModel : System.ComponentModel.INotifyPropertyChanged
    {
        State _state;
        public LineReportModel()
        {
            this._state = Application.Current.FindResource("state") as State;
            this.XFormatterTime = value => new DateTime((long)value).ToString("t");
            this.XFormatterDate = value => new DateTime((long)value).ToString("MMM dd");
            this.PressureSeries = new SeriesCollection();
            this.PressureSeriesSummary = new SeriesCollection();

            LineSeries lineSystolic = new LineSeries { Title = "Systolic Blood Pressure", LineSmoothness = 0 };
            lineSystolic.Values = new ChartValues<DateTimePoint>();
            LineSeries lineDiastolic = new LineSeries { Title = "Diastolic Blood Pressure", LineSmoothness = 0 };
            lineDiastolic.Values = new ChartValues<DateTimePoint>();
            LineSeries lineMabp = new LineSeries { Title = "MABP", LineSmoothness = 0 };
            lineMabp.Values = new ChartValues<DateTimePoint>();

            LineSeries lineSystolicSummary = new LineSeries { Title = "Systolic Blood Pressure", PointGeometry = null, LineSmoothness = 0 };
            lineSystolicSummary.Values = new ChartValues<DateTimePoint>();
            LineSeries lineDiastolicSummary = new LineSeries { Title = "Diastolic Blood Pressure", PointGeometry = null, LineSmoothness = 0 };
            lineDiastolicSummary.Values = new ChartValues<DateTimePoint>();
            LineSeries lineMabpSummary = new LineSeries { Title = "MABP", PointGeometry = null, LineSmoothness = 0 };
            lineMabpSummary.Values = new ChartValues<DateTimePoint>();
            
            if(_state.ActivePatient.Readings.Count > 0)
            {
                this.From = _state.ActivePatient.Readings[0].Date.Ticks;
                this.To = _state.ActivePatient.Readings[0].Date.AddHours(3).Ticks;
            }
            else
            {
                this.From = 0;
                this.To = 1;
            }
            foreach (var reading in _state.ActivePatient.Readings)
            {
                lineSystolic.Values.Add(new DateTimePoint(reading.Date, reading.Sys));
                lineDiastolic.Values.Add(new DateTimePoint(reading.Date, reading.Dia));
                lineMabp.Values.Add(new DateTimePoint(reading.Date, reading.MABP));

                lineSystolicSummary.Values.Add(new DateTimePoint(reading.Date, reading.Sys));
                lineDiastolicSummary.Values.Add(new DateTimePoint(reading.Date, reading.Dia));
                lineMabpSummary.Values.Add(new DateTimePoint(reading.Date, reading.MABP));
            }
            this.PressureSeries.Add(lineSystolic);
            this.PressureSeries.Add(lineDiastolic);
            this.PressureSeries.Add(lineMabp);

            this.PressureSeriesSummary.Add(lineSystolicSummary);
            this.PressureSeriesSummary.Add(lineDiastolicSummary);
            this.PressureSeriesSummary.Add(lineMabpSummary);
        }
        public SeriesCollection PressureSeries { get; private set; }
        public SeriesCollection PressureSeriesSummary { get; private set; }
        public Func<double, string> XFormatterTime { get; set; }
        public Func<double, string> XFormatterDate { get; set; }
        private double _from;

        public double From
        {
            get { return _from; }
            set { 
                _from = value;
                this.OnPropertyChanged("From");
            }
        }
        private double _to;

        public double To
        {
            get { return _to; }
            set { 
                _to = value;
                this.OnPropertyChanged("To");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// Interaction logic for LineReport.xaml
    /// </summary>
    public partial class LineReport : UserControl
    {
        public LineReport()
        {
            InitializeComponent();
        }
    }
}
