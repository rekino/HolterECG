using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using HolterECG.DataStructures;

namespace HolterECG
{
    enum ReportType { Text, Line, Bar, Pie, Analysis }
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
            this._activePatient = new Patient
            {
                FirstName = "Ramin",
                LastName = "Barati",
                Age = 29,
                Height = 179,
                Comments = "Lorem ipsum dolor sit amet, vel hinc porro eligendi et, vituperata sadipscing ne qui. Feugait corrumpit scribentur an nam, error intellegat has cu. Est at hinc consequuntur, ei mel diam mediocrem petentium. Eos at expetendis voluptatibus, sapientem conceptam eum te, causae salutatus et vim. Justo utinam bonorum ne nam, ancillae accusata id usu. Cibo posse ius no.",
                Insurance = "Parsian",
                Medications = "Lorem ipsum dolor sit amet, vel hinc porro eligendi et, vituperata sadipscing ne qui.",
                MoreInformation = "Lorem ipsum dolor sit amet, vel hinc porro eligendi et, vituperata sadipscing ne qui. Feugait corrumpit scribentur an nam, error intellegat has cu. Est at hinc consequuntur, ei mel diam mediocrem petentium. Eos at expetendis voluptatibus, sapientem conceptam eum te, causae salutatus et vim. Justo utinam bonorum ne nam, ancillae accusata id usu. Cibo posse ius no.",
                PhoneNumber = "09131091517",
                Physician = "Dr. Hesabi",
                Sex = Sex.Male,
                Symptoms = "Lorem ipsum dolor sit amet, vel hinc porro eligendi et, vituperata sadipscing ne qui. Feugait corrumpit scribentur an nam, error intellegat has cu.",
                Weight = 75
            };
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
