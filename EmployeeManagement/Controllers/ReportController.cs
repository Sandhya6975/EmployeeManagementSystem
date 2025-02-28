using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using EmployeeManagement.Models;
using WebGrease.Activities;
using CrystalDecisions.Shared;

namespace EmployeeManagement.Controllers
{
    public class ReportController : Controller
    {
        string conString = ConfigurationManager.ConnectionStrings["data"].ConnectionString;

        public ActionResult GenerateReport(string reportType, string Data)
        {
            string query = "SELECT * FROM Employee_Details";

            if (reportType == "Employee-wise")
            {
                query = "SELECT * FROM Employee_Details WHERE Id = "+ Data; // Modify to get dynamic EmployeeId
            }
            else if (reportType == "Designation")
            {
                query = "SELECT * FROM Employee_Details where Designation=" + Data;
            }
            else if (reportType == "Combination")
            {
                query = "SELECT * FROM Employee_Details WHERE Designation IN ('Software Engineer', 'QA Engineer')";
            }
            else if (reportType == "Hierarchy")
            {
                query = "SELECT e1.ID, e1.EmpName AS Employee, e1.Designation, e1.State, e1.Salary,e2.EmpName AS Reporting_Manager, e2.Designation AS Manager_Role FROM Employee_Details e1 LEFT JOIN Employee_Details e2 ON e1.ReportingTo = e2.ID ORDER BY e2.ID, e1.ID;";
            }

            return GenerateCrystalReport(query);
        }

        public ActionResult GenerateCrystalReport(string query)
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                string reportPath = Server.MapPath("~/CrystalReport1.rpt");
                rptDoc.Load(reportPath);

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataSet ds = new DataSet();                
                    da.Fill(ds, "Employees");

                    if (ds.Tables["Employees"].Rows.Count == 0)
                    {
                        return Content("No data found.");
                    }

                    rptDoc.SetDataSource(ds);
                    rptDoc.Refresh();
                }

                // 🛑 Instead of returning .rpt file, export as PDF
                Stream stream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf", "EmployeeReport.pdf");
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }
    }
}
