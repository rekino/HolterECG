using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using System.Collections.ObjectModel;

namespace HolterECG
{
    enum ReportType { Text, Line, Bar, Pie }
    class State : System.ComponentModel.INotifyPropertyChanged
    {
        string _route = "ReportPage.xaml";
        ReportType _activeReport = ReportType.Text;
        bool _isConnected = false;
        public State()
        {
            this.Points = new ObservableCollection<DataPoint>
                              {
                                  new DataPoint(0, 4),
                                  new DataPoint(10, 13),
                                  new DataPoint(20, 15),
                                  new DataPoint(30, 16),
                                  new DataPoint(40, 12),
                                  new DataPoint(50, 12)
                              };
            this.ActiveFile = new System.Windows.Data.XmlDataProvider();
        }

        public ObservableCollection<DataPoint> Points { get; private set; }
        public string Route
        {
            get { return _route; }
            set
            {
                _route = value;
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("Route"));
            }
        }
        public ReportType ActiveReport
        {
            get { return _activeReport; }
            set { 
                _activeReport = value;
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("ActiveReport"));
            }
        }
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("IsConnected"));
            }
        }
        public System.Windows.Data.XmlDataProvider ActiveFile { get; private set; }

        

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
