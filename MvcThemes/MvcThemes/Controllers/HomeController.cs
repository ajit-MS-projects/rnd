using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcThemes.Models;

namespace MvcThemes.Controllers
{
    public class HomeController : Controller
    {
        static List<Employee> employees = new List<Employee>();

        static HomeController()
        {
            employees = new List<Employee>();
            for (int i = 0; i < 10; i++)
            {
                var e = new Employee()
                            {
                                Id = new Random().Next(),
                                Name = "Nm_" + new Random().Next().ToString(),
                                Salary = new Random().Next()
                            };

                employees.Add(e);
            }
        }
        public ActionResult Index()
        {
            return View(employees);
        }
        public ActionResult Details(int id)
        {
            Employee e = employees.Where(x => x.Id == id).FirstOrDefault();
            return View(e);
        }
        public ActionResult Edit(int id)
        {
            Employee e = employees.Where(x => x.Id == id).FirstOrDefault();
            return View(e);
        }
    }
}
