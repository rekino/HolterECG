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

namespace HolterECG
{
    /// <summary>
    /// Interaction logic for PaperReport.xaml
    /// </summary>
    public partial class PaperReport : Page
    {
        public PaperReport()
        {
            InitializeComponent();
            var i = 0;
            foreach (var reading in (this.DataContext as Report).Patient.Readings)
            {
                i++;
                TableRow row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(i.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(reading.Date.ToString("d")))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(reading.Date.ToString("t")))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(reading.Sys.ToString("n2")))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(reading.Dia.ToString("n2")))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(reading.HR.ToString("n2")))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(reading.MABP.ToString("n2")))));
                rows.Rows.Add(row);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
