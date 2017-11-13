using HolterECG.DataStructures;
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
    class Slice
    {
        public Slice()
        {
            this.Readings = new List<Reading>();
        }
        public Func<Reading, bool> Accepts { get; set; }
        public string Title { get; set; }
        public Brush Fill { get; set; }
        public SeriesCollection Series { get; set; }
        public Func<ChartPoint, string> Formatter
        {
            get { return point => string.Format("{0} ({1:P})", point.SeriesView.Title, point.Y); }
        }
        public string[] Legend { get; set; }
        public List<Reading> Readings { get; private set; }
    }
    class AnalysisReportModel
    {
        State _state;
        public AnalysisReportModel()
        {
            _state = Application.Current.FindResource("state") as State;

            this.WHO = new SeriesCollection();
            this.AHA = new SeriesCollection();
            this.JNC8 = new SeriesCollection();

            // WHO
            this.WhoSlices = new ObservableCollection<Slice>();
            WhoSlices.Add(new Slice
            {
                Title = "Optimal",
                Accepts = reading => reading.Sys < 120 && reading.Dia < 80,
                Fill = Brushes.Green,
                Series = this.WHO,
                Legend = new string[] {"<120", "AND","<80"}
            });
            WhoSlices.Add(new Slice
            {
                Title = "Normal",
                Accepts = reading => (reading.Sys >= 120 && reading.Sys < 130) || (reading.Dia >= 80 && reading.Dia < 85),
                Fill = Brushes.Green,
                Series = this.WHO,
                Legend = new string[] {"120-129", "OR", "80-84"}
            });
            WhoSlices.Add(new Slice
            {
                Title = "High Normal",
                Accepts = reading => (reading.Sys >= 130 && reading.Sys < 140) || (reading.Dia >= 85 && reading.Dia < 90),
                Fill = Brushes.Green,
                Series = this.WHO,
                Legend = new string[] {"130-139", "OR", "85-89"}
            });
            WhoSlices.Add(new Slice
            {
                Title = "Grade I",
                Accepts = reading => (reading.Sys >= 140 && reading.Sys < 160) || (reading.Dia >= 90 && reading.Dia < 100),
                Fill = Brushes.Yellow,
                Series = this.WHO,
                Legend = new string[] {"140-159", "OR", "90-99"}
            });
            WhoSlices.Add(new Slice
            {
                Title = "Grade II",
                Accepts = reading => (reading.Sys >= 160 && reading.Sys < 180) || (reading.Dia >= 100 && reading.Dia < 110),
                Fill = Brushes.Orange,
                Series = this.WHO,
                Legend = new string[] {"160-179", "OR", "100-109"}
            });
            WhoSlices.Add(new Slice
            {
                Title = "Grade III",
                Accepts = reading => reading.Sys >= 180 || reading.Dia >= 110,
                Fill = Brushes.Red,
                Series = this.WHO,
                Legend = new string[] {">=180", "OR", ">=110"}
            });
            WhoSlices.Add(new Slice
            {
                Title = "ISH",
                Accepts = reading => reading.Sys > 140 && reading.Dia < 90,
                Fill = Brushes.Purple,
                Series = this.WHO,
                Legend = new string[] {">140", "AND", "<90"}
            });

            // AHA
            AhaSlices = new ObservableCollection<Slice>();
            AhaSlices.Add(new Slice
            {
                Title = "Normal",
                Accepts = reading => reading.Sys < 120 && reading.Dia < 80,
                Fill = Brushes.Green,
                Series = this.AHA,
                Legend = new string[] {"<120", "AND", "<80"}
            });
            AhaSlices.Add(new Slice
            {
                Title = "Prehypertesion",
                Accepts = reading => (reading.Sys >= 120 && reading.Sys < 140) || (reading.Dia >= 80 && reading.Dia < 90),
                Fill = Brushes.Yellow,
                Series = this.AHA,
                Legend = new string[] {"120-139", "OR", "80-89"}
            });
            AhaSlices.Add(new Slice
            {
                Title = "Stage 1 Hypertension",
                Accepts = reading => (reading.Sys >= 140 && reading.Sys < 160) || (reading.Dia >= 90 && reading.Dia < 100),
                Fill = Brushes.Orange,
                Series = this.AHA,
                Legend = new string[] {"140-159", "OR", "90-99"}
            });
            AhaSlices.Add(new Slice
            {
                Title = "Stage 2 Hypertension",
                Accepts = reading => (reading.Sys >= 160 && reading.Sys <= 180) || (reading.Dia >= 100 && reading.Dia <= 110),
                Fill = Brushes.OrangeRed,
                Series = this.AHA,
                Legend = new string[] {"160-179", "OR", "100-109"}
            });
            AhaSlices.Add(new Slice
            {
                Title = "Hypertension Crisis",
                Accepts = reading => reading.Sys > 180 || reading.Dia > 110,
                Fill = Brushes.Red,
                Series = this.AHA,
                Legend = new string[] {">180", "OR", ">110"}
            });

            // JNC8
            Jnc8Slices = new ObservableCollection<Slice>();
            Jnc8Slices.Add(new Slice
            {
                Title = "Normal",
                Accepts = reading => (reading.Sys >= 90 && reading.Sys < 120) || (reading.Dia >= 60 && reading.Dia < 80),
                Fill = Brushes.Green,
                Series = this.JNC8,
                Legend = new string[] {"90-119", "OR", "60-79"}
            });
            Jnc8Slices.Add(new Slice
            {
                Title = "Prehypertesion",
                Accepts = reading => (reading.Sys >= 120 && reading.Sys < 140) || (reading.Dia >= 80 && reading.Dia < 90),
                Fill = Brushes.Green,
                Series = this.JNC8,
                Legend = new string[] {"120-139", "OR", "80-89"}
            });
            Jnc8Slices.Add(new Slice
            {
                Title = "Stage 1 Hypertension",
                Accepts = reading => (reading.Sys >= 140 && reading.Sys < 160) || (reading.Dia >= 90 && reading.Dia < 100),
                Fill = Brushes.Yellow,
                Series = this.JNC8,
                Legend = new string[] {"140-159", "OR", "90-99"}
            });
            Jnc8Slices.Add(new Slice
            {
                Title = "Stage 2 Hypertension",
                Accepts = reading => reading.Sys >= 160 || reading.Dia >= 100,
                Fill = Brushes.Red,
                Series = this.JNC8,
                Legend = new string[] {">=160", "OR", ">=100"}
            });
            Jnc8Slices.Add(new Slice
            {
                Title = "ISH",
                Accepts = reading => reading.Sys >= 140 && reading.Dia < 90,
                Fill = Brushes.Purple,
                Series = this.JNC8,
                Legend = new string[] {">=140", "AND", "<90"}
            });

            List<Slice> slices = new List<Slice>();
            slices.AddRange(WhoSlices);
            slices.AddRange(AhaSlices);
            slices.AddRange(Jnc8Slices);

            foreach (var reading in _state.ActivePatient.Readings)
            {
                foreach (var slice in slices)
                    if (slice.Accepts(reading))
                        slice.Readings.Add(reading);
            }

            foreach (var slice in slices)
            {
                slice.Series.Add(new PieSeries
                {
                    Title = slice.Title,
                    Values = new ChartValues<double>(new double[] { slice.Readings.Count / (double)_state.ActivePatient.Readings.Count }),
                    DataLabels = true,
                    LabelPoint = slice.Formatter,
                    Fill = slice.Fill
                });
            }
        }
        public SeriesCollection WHO { get; private set; }
        public SeriesCollection AHA { get; private set; }
        public SeriesCollection JNC8 { get; private set; }
        public ObservableCollection<Slice> WhoSlices { get; private set; }
        public ObservableCollection<Slice> AhaSlices { get; private set; }
        public ObservableCollection<Slice> Jnc8Slices { get; private set; }
    }
    /// <summary>
    /// Interaction logic for AnalysisReport.xaml
    /// </summary>
    public partial class AnalysisReport : UserControl
    {
        public AnalysisReport()
        {
            InitializeComponent();
        }
    }
}
