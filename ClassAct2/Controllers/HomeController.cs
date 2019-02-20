using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassAct2.ViewModels;
using ClassAct2.Models;

namespace ClassAct2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            VMs vm = new VMs();

            vm.Employees = GetEmployees(0);

            vm.DateFrom = new DateTime(2014, 12, 1);
            vm.DateTo = new DateTime(2014, 12, 31);

            return View(vm);
        }

        public SelectList GetEmployees(int selected)
        {
            using (HardwareDBEntities db = new HardwareDBEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                var employees = db.lgemployees.Select(x => new SelectListItem
                {
                    Value = x.emp_num.ToString(),
                    Text = x.emp_fname +  x.emp_lname
                }).ToList();

                if (selected == 0)
                    return new SelectList(employees, "Value", "Text");
                else
                    return new SelectList(employees, "Value", "Text", selected);
            }
        }

        [HttpPost]
        public ActionResult Index(VMs vm)
        {
            using (HardwareDBEntities db = new HardwareDBEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                vm.Employees = GetEmployees(vm.SelectedEmployeeID);

                vm.employee = db.lgemployees.Where(x => x.emp_num == vm.SelectedEmployeeID).FirstOrDefault();

                var list = db.lginvoices.Where(x => x.employee_id == vm.SelectedEmployeeID && x.inv_DATETIME >= vm.DateFrom && x.inv_DATETIME <= vm.DateTo).ToList().Select(rr => new ReportRecord
                {
                    OrderDate = Convert.ToDateTime(rr.inv_DATETIME).ToString("MMM. dd, yyyy"),
                    Amount = Convert.ToDouble(rr.inv_total),
                    CustomerID = Convert.ToInt32(rr.cust_code),
                    CustomerName = db.lgcustomers.Where(x => x.cust_code == rr.cust_code).FirstOrDefault().cust_fname,
                    CustomerSurname = db.lgcustomers.Where(x => x.cust_code == rr.cust_code).FirstOrDefault().cust_lname
                }
                );

                vm.results = list.GroupBy(x => x.CustomerID).ToList();

                vm.chartData = list.GroupBy(g => g.OrderDate.ToString()).ToDictionary(g=> g.Key, g=> g.Sum(v => v.Amount));

                TempData["chartdata"] = vm.chartData;
                TempData["records"] = list.ToList();
                TempData["employee"] = vm.employee;

                return View(vm);
            }
        }
        public ActionResult CustomerSalesChart()
        {
            var data = TempData["chartdata"];
            return View(TempData["chartdata"]);
        }
    }
}