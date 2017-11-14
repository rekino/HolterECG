using HolterECG.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HolterECG.DataStructures
{
    class Statistics
    {
        public double SysMax { get; set; }
        public double DiaMax { get; set; }
        public double MapMax { get; set; }
        public double HrMax { get; set; }
        public DateTime SysMaxTime { get; set; }
        public DateTime DiaMaxTime { get; set; }
        public DateTime MapMaxTime { get; set; }
        public DateTime HrMaxTime { get; set; }
        public double SysMin { get; set; }
        public double DiaMin { get; set; }
        public double MapMin { get; set; }
        public double HrMin { get; set; }
        public DateTime SysMinTime { get; set; }
        public DateTime DiaMinTime { get; set; }
        public DateTime MapMinTime { get; set; }
        public DateTime HrMinTime { get; set; }
        public double SysAvg { get; set; }
        public double DiaAvg { get; set; }
        public double MapAvg { get; set; }
        public double HrAvg { get; set; }
        public double SysStd { get; set; }
        public double DiaStd { get; set; }
        public double MapStd { get; set; }
        public double HrStd { get; set; }
        public int Count { get; set; }
        public int Length { get; set; }
        public double Fraction
        {
            get { return (double)Count / Length; }
        }
    }
    class Report : System.ComponentModel.INotifyPropertyChanged
    {
        State _state;
        public Report()
        {
            _state = Application.Current.FindResource("state") as State;

            TimeSpan NightStart = new TimeSpan(0, 23, 0, 0);
            TimeSpan NightEnd = new TimeSpan(0, 7, 0, 0);
            Overall = GetStatistics(Patient.Readings);
            Awake = GetStatistics(Patient.Readings.Where(x => (x.Date.TimeOfDay > NightEnd) && (x.Date.TimeOfDay < NightStart)));
            Asleep = GetStatistics(Patient.Readings.Where(x => !((x.Date.TimeOfDay > NightEnd) && (x.Date.TimeOfDay < NightStart))));

            Lines = new SeriesCollection();
            LineSeries lineSystolic = new LineSeries { Title = "Systolic Blood Pressure", LineSmoothness = 0, Fill = Brushes.Transparent };
            lineSystolic.Values = new ChartValues<DateTimePoint>();
            LineSeries lineDiastolic = new LineSeries { Title = "Diastolic Blood Pressure", LineSmoothness = 0, Fill = Brushes.Transparent };
            lineDiastolic.Values = new ChartValues<DateTimePoint>();
            LineSeries lineHr = new LineSeries { Title = "Heart Rate", LineSmoothness = 0, Fill = Brushes.Transparent };
            lineHr.Values = new ChartValues<DateTimePoint>();
            foreach (var reading in Patient.Readings)
            {
                lineSystolic.Values.Add(new DateTimePoint(reading.Date, reading.Sys));
                lineDiastolic.Values.Add(new DateTimePoint(reading.Date, reading.Dia));
                lineHr.Values.Add(new DateTimePoint(reading.Date, reading.HR));
            }
            this.Lines.Add(lineSystolic);
            this.Lines.Add(lineDiastolic);
            this.Lines.Add(lineHr);

            PieWho = new SeriesCollection();
            PieAha = new SeriesCollection();
            PieJnc8 = new SeriesCollection();

            // WHO
            this.WhoSlices = new ObservableCollection<Slice>();
            WhoSlices.Add(new Slice
            {
                Title = "Optimal",
                Accepts = reading => reading.Sys < 120 && reading.Dia < 80,
                Fill = Brushes.Green,
                Series = this.PieWho,
                Legend = new string[] { "<120", "AND", "<80" }
            });
            WhoSlices.Add(new Slice
            {
                Title = "Normal",
                Accepts = reading => (reading.Sys >= 120 && reading.Sys < 130) || (reading.Dia >= 80 && reading.Dia < 85),
                Fill = Brushes.Green,
                Series = this.PieWho,
                Legend = new string[] { "120-129", "OR", "80-84" }
            });
            WhoSlices.Add(new Slice
            {
                Title = "High Normal",
                Accepts = reading => (reading.Sys >= 130 && reading.Sys < 140) || (reading.Dia >= 85 && reading.Dia < 90),
                Fill = Brushes.Green,
                Series = this.PieWho,
                Legend = new string[] { "130-139", "OR", "85-89" }
            });
            WhoSlices.Add(new Slice
            {
                Title = "Grade I",
                Accepts = reading => (reading.Sys >= 140 && reading.Sys < 160) || (reading.Dia >= 90 && reading.Dia < 100),
                Fill = Brushes.Yellow,
                Series = this.PieWho,
                Legend = new string[] { "140-159", "OR", "90-99" }
            });
            WhoSlices.Add(new Slice
            {
                Title = "Grade II",
                Accepts = reading => (reading.Sys >= 160 && reading.Sys < 180) || (reading.Dia >= 100 && reading.Dia < 110),
                Fill = Brushes.Orange,
                Series = this.PieWho,
                Legend = new string[] { "160-179", "OR", "100-109" }
            });
            WhoSlices.Add(new Slice
            {
                Title = "Grade III",
                Accepts = reading => reading.Sys >= 180 || reading.Dia >= 110,
                Fill = Brushes.Red,
                Series = this.PieWho,
                Legend = new string[] { ">=180", "OR", ">=110" }
            });
            WhoSlices.Add(new Slice
            {
                Title = "ISH",
                Accepts = reading => reading.Sys > 140 && reading.Dia < 90,
                Fill = Brushes.Purple,
                Series = this.PieWho,
                Legend = new string[] { ">140", "AND", "<90" }
            });

            // AHA
            AhaSlices = new ObservableCollection<Slice>();
            AhaSlices.Add(new Slice
            {
                Title = "Normal",
                Accepts = reading => reading.Sys < 120 && reading.Dia < 80,
                Fill = Brushes.Green,
                Series = this.PieAha,
                Legend = new string[] { "<120", "AND", "<80" }
            });
            AhaSlices.Add(new Slice
            {
                Title = "Prehypertesion",
                Accepts = reading => (reading.Sys >= 120 && reading.Sys < 140) || (reading.Dia >= 80 && reading.Dia < 90),
                Fill = Brushes.Yellow,
                Series = this.PieAha,
                Legend = new string[] { "120-139", "OR", "80-89" }
            });
            AhaSlices.Add(new Slice
            {
                Title = "Stage 1 Hypertension",
                Accepts = reading => (reading.Sys >= 140 && reading.Sys < 160) || (reading.Dia >= 90 && reading.Dia < 100),
                Fill = Brushes.Orange,
                Series = this.PieAha,
                Legend = new string[] { "140-159", "OR", "90-99" }
            });
            AhaSlices.Add(new Slice
            {
                Title = "Stage 2 Hypertension",
                Accepts = reading => (reading.Sys >= 160 && reading.Sys <= 180) || (reading.Dia >= 100 && reading.Dia <= 110),
                Fill = Brushes.OrangeRed,
                Series = this.PieAha,
                Legend = new string[] { "160-179", "OR", "100-109" }
            });
            AhaSlices.Add(new Slice
            {
                Title = "Hypertension Crisis",
                Accepts = reading => reading.Sys > 180 || reading.Dia > 110,
                Fill = Brushes.Red,
                Series = this.PieAha,
                Legend = new string[] { ">180", "OR", ">110" }
            });

            // JNC8
            Jnc8Slices = new ObservableCollection<Slice>();
            Jnc8Slices.Add(new Slice
            {
                Title = "Normal",
                Accepts = reading => (reading.Sys >= 90 && reading.Sys < 120) || (reading.Dia >= 60 && reading.Dia < 80),
                Fill = Brushes.Green,
                Series = this.PieJnc8,
                Legend = new string[] { "90-119", "OR", "60-79" }
            });
            Jnc8Slices.Add(new Slice
            {
                Title = "Prehypertesion",
                Accepts = reading => (reading.Sys >= 120 && reading.Sys < 140) || (reading.Dia >= 80 && reading.Dia < 90),
                Fill = Brushes.Green,
                Series = this.PieJnc8,
                Legend = new string[] { "120-139", "OR", "80-89" }
            });
            Jnc8Slices.Add(new Slice
            {
                Title = "Stage 1 Hypertension",
                Accepts = reading => (reading.Sys >= 140 && reading.Sys < 160) || (reading.Dia >= 90 && reading.Dia < 100),
                Fill = Brushes.Yellow,
                Series = this.PieJnc8,
                Legend = new string[] { "140-159", "OR", "90-99" }
            });
            Jnc8Slices.Add(new Slice
            {
                Title = "Stage 2 Hypertension",
                Accepts = reading => reading.Sys >= 160 || reading.Dia >= 100,
                Fill = Brushes.Red,
                Series = this.PieJnc8,
                Legend = new string[] { ">=160", "OR", ">=100" }
            });
            Jnc8Slices.Add(new Slice
            {
                Title = "ISH",
                Accepts = reading => reading.Sys >= 140 && reading.Dia < 90,
                Fill = Brushes.Purple,
                Series = this.PieJnc8,
                Legend = new string[] { ">=140", "AND", "<90" }
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
                slice.Percentage = slice.Readings.Count / (double)_state.ActivePatient.Readings.Count;
                slice.Series.Add(new PieSeries
                {
                    Title = slice.Title,
                    Values = new ChartValues<double>(new double[] { slice.Percentage }),
                    Fill = slice.Fill,
                    Foreground = Brushes.Black
                });
            }

            BarSystolic = new SeriesCollection();
            BarDiastolic = new SeriesCollection();
            BarHeartRate = new SeriesCollection();
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
            BarSystolic.Add(CreateHistogram(syss, totalBuckets, "Systolic Blood Pressure", Brushes.Blue));
            BarDiastolic.Add(CreateHistogram(dias, totalBuckets, "Diastolic Blood Pressure", Brushes.Blue));
            BarHeartRate.Add(CreateHistogram(hrs, totalBuckets, "Heart Rate", Brushes.Blue));
        }

        Statistics GetStatistics(IEnumerable<Reading> readings)
        {
            Statistics result = new Statistics();

            result.Count = readings.Count();
            result.Length = Patient.Readings.Count;

            var minsys = readings.Aggregate(new Reading{Sys=double.MaxValue}, (acc, e) => acc.Sys < e.Sys ? acc : e);
            var mindia = readings.Aggregate(new Reading { Dia = double.MaxValue }, (acc, e) => acc.Dia < e.Dia ? acc : e);
            var minmap = readings.Aggregate(new Reading { MABP = double.MaxValue }, (acc, e) => acc.MABP < e.MABP ? acc : e);
            var minhr = readings.Aggregate(new Reading { HR = double.MaxValue }, (acc, e) => acc.HR < e.HR ? acc : e);

            var maxsys = readings.Aggregate(new Reading { Sys = double.MinValue }, (acc, e) => acc.Sys > e.Sys ? acc : e);
            var maxdia = readings.Aggregate(new Reading { Dia = double.MinValue }, (acc, e) => acc.Dia > e.Dia ? acc : e);
            var maxmap = readings.Aggregate(new Reading { MABP = double.MinValue }, (acc, e) => acc.MABP > e.MABP ? acc : e);
            var maxhr = readings.Aggregate(new Reading { HR = double.MinValue }, (acc, e) => acc.HR > e.HR ? acc : e);
            

            var syss = readings.Select(x => x.Sys);
            var dias = readings.Select(x => x.Dia);
            var maps = readings.Select(x => x.MABP);
            var hrs = readings.Select(x => x.HR);

            result.SysMax = syss.Count() > 0 ? maxsys.Sys : 0;
            result.DiaMax = dias.Count() > 0 ? maxdia.Dia : 0;
            result.MapMax = maps.Count() > 0 ? maxmap.MABP : 0;
            result.HrMax = hrs.Count() > 0 ? maxhr.HR : 0;
            result.SysMaxTime = syss.Count() > 0 ? maxsys.Date : DateTime.Now;
            result.DiaMaxTime = dias.Count() > 0 ? maxdia.Date : DateTime.Now;
            result.MapMaxTime = maps.Count() > 0 ? maxmap.Date : DateTime.Now;
            result.HrMaxTime = hrs.Count() > 0 ? maxhr.Date : DateTime.Now;

            result.SysMin = syss.Count() > 0 ? minsys.Sys : 0;
            result.DiaMin = dias.Count() > 0 ? mindia.Dia : 0;
            result.MapMin = maps.Count() > 0 ? minmap.MABP : 0;
            result.HrMin = hrs.Count() > 0 ? minhr.HR : 0;
            result.SysMinTime = syss.Count() > 0 ? minsys.Date : DateTime.Now;
            result.DiaMinTime = dias.Count() > 0 ? mindia.Date : DateTime.Now;
            result.MapMinTime = maps.Count() > 0 ? minmap.Date : DateTime.Now;
            result.HrMinTime = hrs.Count() > 0 ? minhr.Date : DateTime.Now;

            result.SysAvg = syss.Count() > 0 ? syss.Average() : 0;
            result.DiaAvg = dias.Count() > 0 ? dias.Average() : 0;
            result.MapAvg = maps.Count() > 0 ? maps.Average() : 0;
            result.HrAvg = hrs.Count() > 0 ? hrs.Average() : 0;

            result.SysStd = syss.Count() > 0 ? Math.Sqrt(syss.Select(x => (x - result.SysAvg) * (x - result.SysAvg)).Sum() / result.Count) : 0;
            result.DiaStd = dias.Count() > 0 ? Math.Sqrt(dias.Select(x => (x - result.DiaAvg) * (x - result.DiaAvg)).Sum() / result.Count) : 0;
            result.MapStd = maps.Count() > 0 ? Math.Sqrt(maps.Select(x => (x - result.MapAvg) * (x - result.MapAvg)).Sum() / result.Count) : 0;
            result.HrStd = hrs.Count() > 0 ? Math.Sqrt(hrs.Select(x => (x - result.HrAvg) * (x - result.HrAvg)).Sum() / result.Count) : 0;

            return result;
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

        public Statistics Overall { get; private set; }
        public Statistics Awake { get; private set; }
        public Statistics Asleep { get; private set; }
        public Patient Patient { get { return _state.ActivePatient; } }
        public SeriesCollection Lines { get; private set; }

        public SeriesCollection BarSystolic { get; private set; }
        public SeriesCollection BarDiastolic { get; private set; }
        public SeriesCollection BarHeartRate { get; private set; }

        public SeriesCollection PieWho { get; private set; }
        public SeriesCollection PieAha { get; private set; }
        public SeriesCollection PieJnc8 { get; private set; }

        public Func<double, string> XFormatterTime { get { return value => new DateTime((long)value).ToString("t"); } }

        public ObservableCollection<Slice> WhoSlices { get; private set; }
        public ObservableCollection<Slice> AhaSlices { get; private set; }
        public ObservableCollection<Slice> Jnc8Slices { get; private set; }

        public Func<double, string> YFormatter { get { return value => String.Format("{0:P}", value); } }
        public string[] XLabels { get; private set; }

        private bool _includeStatistics;

        public bool IncludeStatistics
        {
            get { return _includeStatistics; }
            set { _includeStatistics = value; OnPropertyChanged("IncludeStatistics"); }
        }
        private bool _includeLines;

        public bool IncludeLines
        {
            get { return _includeLines; }
            set { _includeLines = value; OnPropertyChanged("IncludeLines"); }
        }
        private bool _includeDiagnosis;

        public bool IncludeDiagnosis
        {
            get { return _includeDiagnosis; }
            set { _includeDiagnosis = value; OnPropertyChanged("IncludeDiagnosis"); }
        }
        private bool _includeAnalysisWho;

        public bool IncludeAnalysisWho
        {
            get { return _includeAnalysisWho; }
            set { _includeAnalysisWho = value; OnPropertyChanged("IncludeAnalysisWho"); }
        }
        private bool _includeAnalysisAha;

        public bool IncludeAnalysisAha
        {
            get { return _includeAnalysisAha; }
            set { _includeAnalysisAha = value; OnPropertyChanged("IncludeAnalysisAha"); }
        }
        private bool _includeAnalysisJnc8;

        public bool IncludeAnalysisJnc8
        {
            get { return _includeAnalysisJnc8; }
            set { _includeAnalysisJnc8 = value; OnPropertyChanged("IncludeAnalysisJnc8"); }
        }
        private bool _includeHistogramSys;

        public bool IncludeHistogramSys
        {
            get { return _includeHistogramSys; }
            set { _includeHistogramSys = value; OnPropertyChanged("IncludeHistogramSys"); }
        }
        private bool _includeHistogramDia;

        public bool IncludeHistogramDia
        {
            get { return _includeHistogramDia; }
            set { _includeHistogramDia = value; OnPropertyChanged("IncludeHistogramDia"); }
        }
        private bool _includeHistogramHr;

        public bool IncludeHistogramHr
        {
            get { return _includeHistogramHr; }
            set { _includeHistogramHr = value; OnPropertyChanged("IncludeHistogramHr"); }
        }
        private bool _includeReadings;

        public bool IncludeReadings
        {
            get { return _includeReadings; }
            set { _includeReadings = value; OnPropertyChanged("IncludeReadings"); }
        }


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
