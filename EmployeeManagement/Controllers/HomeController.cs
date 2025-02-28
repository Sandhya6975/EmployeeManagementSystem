using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using EmployeeManagement.Models;
using EmployeeManagement.Services;


namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        string conString = ConfigurationManager.ConnectionStrings["data"].ConnectionString;
        public ActionResult Index()
        {
            DBActions obj = new DBActions();
           List<Employee> Getdetails = obj.GetEmployee();
            return View(Getdetails);
        }
        
        public ActionResult Create(Employee emp)
        {
            try
            {
                DBActions obj = new DBActions();
                obj.Addemployee(emp);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }

        public ActionResult Delete(int ID)
        {
            try
            {
                DBActions obj = new DBActions();
                obj.DeleteEmployee(ID);
                return RedirectToAction("Index");
            } 
            catch (Exception ex)
            {
                return View();
            }           
            
        }
        public ActionResult editdetails(int ID)
        {
            try
            {
                DBActions obj = new DBActions();
               List<Employee> emp =obj.Editdata(ID);
                return View(emp);
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }
  
    }
}