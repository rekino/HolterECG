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

namespace HolterECG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        State state;
        public MainWindow()
        {
            InitializeComponent();
            System.IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + @"\files");
            state = Application.Current.FindResource("state") as State;
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            state.Route = "AddPatientPage.xaml";
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            state.Route = "PaperReport.xaml";
        }

        private void btnManualBlood_Click(object sender, RoutedEventArgs e)
        {
            state.Route = "ManualPressurePage.xaml";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            state.Route = "CommentPage.xaml";
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            state.Route = "SettingsPage.xaml";
        }
    }
}
