using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public class DBActions
    {
        string conString = ConfigurationManager.ConnectionStrings["data"].ConnectionString;
        public bool Addemployee(Employee emp)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    //int Id = emp.Id;
                    string query = "";
                    if (emp.Id == 0)
                    {
                        query = "INSERT INTO Employee_Details (EmpName, Designation, DateOfJoin, Salary, Gender, State) " +
                                "VALUES (@Name, @Designation, @DateOfJoin, @Salary, @Gender, @State)";
                    }
                    else
                    {
                        query = "UPDATE Employee_Details " +
                                "SET EmpName = @Name, Designation = @Designation, DateOfJoin = @DateOfJoin, " +
                                "Gender = @Gender, State = @State, Salary = @Salary " +
                                "WHERE ID = @Id";  // Removed extra parenthesis
                    }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", emp.Name);
                        cmd.Parameters.AddWithValue("@Designation", emp.Designation);
                        cmd.Parameters.AddWithValue("@DateOfJoin", emp.DateOfJoin);
                        cmd.Parameters.AddWithValue("@Salary", emp.Salary);
                        cmd.Parameters.AddWithValue("@Gender", emp.Gender);
                        cmd.Parameters.AddWithValue("@State", emp.State);

                        if (emp.Id != 0)  // Only add @Id parameter if it's an update operation
                        {
                            cmd.Parameters.AddWithValue("@Id", emp.Id);
                        }

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                }
                return true;
            }
            catch(Exception ex) 
            {
                return false;

            }
            
        }

        public List<Employee> GetEmployee()
        {
            try
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
                return employees;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }
        public bool DeleteEmployee(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("delete from Employee_Details where ID=@Id", con);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                }
                 return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
       public List<Employee> Editdata(int ID)
       {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    List<Employee> employees = new List<Employee>();
                    SqlCommand cmd = new SqlCommand("select * from Employee_Details where ID=@ID", con);                   
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
                    return employees;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}