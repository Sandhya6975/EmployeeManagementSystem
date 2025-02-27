using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeManagement.Models;


namespace EmployeeManagement.Controllers
{
   
    public class EmployeeController : Controller
    {
        string conString = ConfigurationManager.ConnectionStrings["data"].ConnectionString;

        // GET: Employee List
        public ActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Employee_Details", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = Convert.ToInt32(dr["ID"]),
                        Name = dr["EmpName"].ToString(),
                        Designation = dr["Designation"].ToString(),
                        DateOfJoin = Convert.ToDateTime(dr["DateOfJoin"]),
                        Salary = Convert.ToDecimal(dr["Salary"]),
                        Gender = dr["Gender"].ToString(),
                        State = dr["State"].ToString()
                    });
                }
            }
            return View(employees);
        }

        // POST: Add Employee
       // [HttpPost]
        //public ActionResult Create(Employee emp)
        //{
        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        string query = "INSERT INTO Employees (Name, Designation, DateOfJoin, Salary, Gender, State) VALUES (@Name, @Designation, @DateOfJoin, @Salary, @Gender, @State)";
        //        SqlCommand cmd = new SqlCommand(query, con);
        //        cmd.Parameters.AddWithValue("@Name", emp.Name);
        //        cmd.Parameters.AddWithValue("@Designation", emp.Designation);
        //        cmd.Parameters.AddWithValue("@DateOfJoin", emp.DateOfJoin);
        //        cmd.Parameters.AddWithValue("@Salary", emp.Salary);
        //        cmd.Parameters.AddWithValue("@Gender", emp.Gender);
        //        cmd.Parameters.AddWithValue("@State", emp.State);
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    return RedirectToAction("Index");
        //}

        // POST: Delete Employee
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}