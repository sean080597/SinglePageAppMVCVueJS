using SinglePageAppBusiness;
using SinglePageAppBusiness.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinglePageApp.Controllers
{
    public class TestController : Controller
    {
        IEmployeeBusiness _employeeBusiness;
        public TestController(IEmployeeBusiness employeeBusiness)
        {
            _employeeBusiness = employeeBusiness;
        }

        // GET: Test
        public ActionResult Index()
        {
            ViewBag.EmpList = _employeeBusiness.GetAllEmployee();
            ViewBag.DepartList = _employeeBusiness.GetAllDepartment();
            ViewBag.DepartSingle = _employeeBusiness.GetSingleDepartment(1);
            return View();
        }
    }
}