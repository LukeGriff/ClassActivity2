using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassAct2.Models;

namespace ClassAct2.ViewModels
{
    public class VMs
    {
        public IEnumerable<SelectListItem> Employees { get; set; }
        public int SelectedEmployeeID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public lgemployee employee { get; set; }
        public List<IGrouping<int, ReportRecord>> results { get; set; }
        public Dictionary<string, double> chartData { get; set; }
    }

    public class ReportRecord
    {
        public string OrderDate { get; set; }
        public double Amount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public int CustomerID { get; set; }
    }
}