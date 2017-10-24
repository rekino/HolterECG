using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using HolterECG.DataStructures;

namespace HolterECG
{
    enum ReportType { Text, Line, Bar, Pie }
    class HomeState : System.ComponentModel.INotifyPropertyChanged
    {
        ReportType _activeReport = ReportType.Text;
        public ReportType ActiveReport
        {
            get { return _activeReport; }
            set
            {
                _activeReport = value;
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("ActiveReport"));
            }
        }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
    class State : System.ComponentModel.INotifyPropertyChanged
    {
        string _route = "ReportPage.xaml";
        bool _isConnected = false;
        public State()
        {
            this._activePatient = new Patient();
            this.Home = new HomeState();
        }
        public HomeState Home { get; private set; }
        public string Route
        {
            get { return _route; }
            set
            {
                _route = value;
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("Route"));
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
        private Patient _activePatient;

        public Patient ActivePatient
        {
            get { return _activePatient; }
            set { 
                _activePatient = value;
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("ActivePatient"));
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
