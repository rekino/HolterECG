using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HolterECG.DataStructures
{
    class Report
    {
        State _state;
        public Report()
        {
            _state = Application.Current.FindResource("state") as State;
        }
    }
}
