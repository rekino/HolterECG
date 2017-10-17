using HolterECG.DataStructures;
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
using System.Xml;
using System.Xml.Linq;

namespace HolterECG
{
    /// <summary>
    /// Interaction logic for AddPatient.xaml
    /// </summary>
    public partial class AddPatientPage : Page
    {
        State state;
        public AddPatientPage()
        {
            InitializeComponent();
            state = Application.Current.FindResource("state") as State;
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient()
            {
                Age = int.Parse(txtAge.Text),
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Height = float.Parse(txtHeight.Text),
                Weight = float.Parse(txtWeight.Text),
                Insurance = txtInsurance.Text,
                Medications = txtMedications.Text,
                MoreInformation = txtInfo.Text,
                PhoneNumber = txtPhone.Text,
                Physician = txtPhysician.Text,
                Sex = cmbSex.SelectedIndex == 1 ? Sex.Male : Sex.Female,
                Symptoms = txtSymptoms.Text,
            };

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(patient.Serialize());
            string fileName = System.AppDomain.CurrentDomain.BaseDirectory + string.Format(@"\files\{0}-{1}-{2}.xml", patient.FirstName, patient.LastName, DateTime.Now.ToString("yyyy-MM-dd"));

            xml.Save(fileName);
            state.ActiveFile.Source = new Uri(fileName);
        }
    }
}
