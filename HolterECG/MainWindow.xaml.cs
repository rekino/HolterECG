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
            state = Application.Current.FindResource("state") as State;
            this.frmContent.Content = new Report();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            state.Points.Add(new OxyPlot.DataPoint((state.Points.Count)*10, 10));
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            this.frmContent.Content = new AddPatient();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            this.frmContent.Content = new Report();
        }
    }
}
