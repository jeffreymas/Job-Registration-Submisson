using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.ViewModel
{
    public class ChartVM
    {
       
    }
    public class stsChartVM
    {
        public string status { get; set; }
        public int total { get; set; }
    }
    public class PieChartVM
    {
        public string status { get; set; }
        public string JoblistName { get; set; }
        public int total { get; set; }
    }
    public class BarChartVM
    {
        public string date { get; set; }
        public string car { get; set; }
        public int days { get; set; }
    }
}
