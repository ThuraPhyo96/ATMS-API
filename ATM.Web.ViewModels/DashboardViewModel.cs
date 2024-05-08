using System;
using System.Collections.Generic;
using System.Text;

namespace ATM.Web.ViewModels
{
    public class DashboardViewModel
    {
        public DashboardSummaryViewModel TownshipViewModel { get; set; }
        public DashboardSummaryViewModel BankViewModel { get; set; }
    }

    public class DashboardSummaryViewModel
    {
        public List<string> Labels { get; set; }
        public List<int> Series { get; set; }
    }
}
