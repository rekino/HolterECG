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
            this.XFormatter = value => new DateTime((long)value).ToString("t");
            this.PressureSeries = new SeriesCollection();

            LineSeries lineSystolic = new LineSeries { Title = "Systolic Blood Pressure" };
            lineSystolic.Values = new ChartValues<DateTimePoint>();
            LineSeries lineDiastolic = new LineSeries { Title = "Diastolic Blood Pressure" };
            lineDiastolic.Values = new ChartValues<DateTimePoint>();
            LineSeries lineMabp = new LineSeries { Title = "MABP" };
            lineMabp.Values = new ChartValues<DateTimePoint>();

            foreach (var reading in _state.ActivePatient.Readings)
            {
                lineSystolic.Values.Add(new DateTimePoint(reading.Date, reading.Sys));
                lineDiastolic.Values.Add(new DateTimePoint(reading.Date, reading.Dia));
                lineMabp.Values.Add(new DateTimePoint(reading.Date, reading.MABP));
            }
            PressureSeries.Add(lineSystolic);
            PressureSeries.Add(lineDiastolic);
            PressureSeries.Add(lineMabp);
        }
        public SeriesCollection PressureSeries { get; private set; }
        public Func<double, string> XFormatter { get; private set; }
        private double _from;

        public double From
        {
            get { return _from; }
            set { 
                _from = value;
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("From"));
            }
        }
        private double _to;

        public double To
        {
            get { return _to; }
            set { 
                _to = value;
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("To"));
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
    /// <summary>
    /// Interaction logic for LineReport.xaml
    /// </summary>
    public partial class LineReport : UserControl
    {
        State _state;
        SeriesCollection _seriesPressure;
        public Func<double, string> Formatter { get; private set; }
        public LineReport()
        {
            InitializeComponent();
            _state = Application.Current.FindResource("state") as State;
            this.Formatter = value => new DateTime((long) value).ToString("t");
            _seriesPressure = new SeriesCollection();
            chartSys.Series = _seriesPressure;
            axis_x.LabelFormatter = this.Formatter;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LineSeries lineSystolic = new LineSeries { Title = "Systolic Blood Pressure" };
            lineSystolic.Values = new ChartValues<DateTimePoint>();
            LineSeries lineDiastolic = new LineSeries { Title = "Diastolic Blood Pressure" };
            lineDiastolic.Values = new ChartValues<DateTimePoint>();
            LineSeries lineMabp = new LineSeries { Title = "MABP" };
            lineMabp.Values = new ChartValues<DateTimePoint>();

            foreach (var reading in _state.ActivePatient.Readings)
            {
                lineSystolic.Values.Add(new DateTimePoint(reading.Date, reading.Sys));
                lineDiastolic.Values.Add(new DateTimePoint(reading.Date, reading.Dia));
                lineMabp.Values.Add(new DateTimePoint(reading.Date, reading.MABP));
            }
            _seriesPressure.Add(lineSystolic);
            _seriesPressure.Add(lineDiastolic);
            _seriesPressure.Add(lineMabp);
        }
    }
}
