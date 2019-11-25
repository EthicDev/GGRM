using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGRMLib.Models;

namespace GGRMLib.DataAccess
{
    public class SqlConnector /*: IDataConnection*/
    {
        //CustomerOrder
        public CustomerOrder CreateCO(CustomerOrder co, out string status)
        {
            status = "CustomerOrder creation failed.";
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString("GGRM")))
            {

            }
            //co.id = 4;

            return co;
        }

        //CustomerOrderLine
        public CustomerOrderLine CreateCOLine(CustomerOrderLine col, out string status)
        {
            status = "COLine creation failed.";
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString("GGRM")))
            {

            }
            return col;
        }

        //Customer
        public Customer CreateCustomer(Customer cust, out string status)
        {
            status = "Customer insertion failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "EXEC spCustomer_Insert" +
                        " @custFirst = '" + cust.CustFirst + "',"
                        + " @custLast = '" + cust.CustLast + "',"
                        + " @custPhone = '" + cust.CustPhone + "',"
                        + " @custAddress = '" + cust.CustAddress + "',"
                        + " @custCity = '" + cust.CustCity + "',"
                        + " @custPostal = '" + cust.CustPostal + "',"
                        + " @custEmail = '" + cust.CustEmail +"'";
                    SqlDataReader records = cmd.ExecuteReader();
                    if (records.Read())
                    {
                        cust.ID = records.GetInt32(0);
                    }
                }
                status = "Customer insertion succeeded.";
            } catch (Exception ex) { status = ex.Message; }
            return cust;
        }

        public Customer EditCustomer(Customer cust, out string status)
        {
            status = "Customer update failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "UPDATE customer SET" +
                        " custFirst = '" + cust.CustFirst + "',"
                        + " custLast = '" + cust.CustLast + "',"
                        + " custPhone = '" + cust.CustPhone + "',"
                        + " custAddress = '" + cust.CustAddress + "',"
                        + " custCity = '" + cust.CustCity + "',"
                        + " custPostal = '" + cust.CustPostal + "',"
                        + " custEmail = '" + cust.CustEmail + "'"
                        + " WHERE id = " + cust.ID;
                    cmd.ExecuteNonQuery();
                }
                status = "Customer update succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }
            return cust;
        }

        public List<Customer> GetCustomersList(out string status)
        {
            List<Customer> customers = new List<Customer>();

            status = "Getting customers failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "SELECT id, custFirst, custLast, custPhone, custAddress, custCity, custPostal, custEmail FROM customer ORDER BY custLast";
                    SqlDataReader records = cmd.ExecuteReader();
                    while (records.Read())
                    {
                        Customer cust = new Customer(records.GetString(1),
                            records.GetString(2),
                            records.GetString(3),
                            records.GetString(4),
                            records.GetString(5),
                            records.GetString(6),
                            records.GetString(7));
                        cust.ID = records.GetInt32(0);
                        customers.Add(cust);
                    }
                }
                status = "Getting customers succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return customers;
        }

        public DataTable GetCustomersDataTable(out string status, string searchString = "")
        {
            DataTable dtCustomers = new DataTable();

            status = "Getting customers failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT id, custLast+', '+custFirst AS Name, custPhone AS Phone, custAddress AS Address, custCity AS City, custPostal AS [Postal Code], custEmail AS Email FROM customer WHERE custFirst LIKE '%"+searchString+ "%' OR custLast LIKE '%" + searchString + "%' ORDER BY custLast ";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtCustomers);
                }
                status = "Getting customers succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtCustomers;
        }
        public Customer GetCustomerByID(int id, out string status)
        {
            Customer cust = new Customer();
            status = "Getting customer failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "SELECT id, custFirst, custLast, custPhone, custAddress, custCity, custPostal, custEmail FROM customer WHERE id = " + id.ToString();
                    SqlDataReader records = cmd.ExecuteReader();
                    if (records.Read())
                    {
                        cust = new Customer(records.GetString(1),
                            records.GetString(2),
                            records.GetString(3),
                            records.GetString(4),
                            records.GetString(5),
                            records.GetString(6),
                            records.GetString(7));
                        cust.ID = records.GetInt32(0);
                    }
                }
                status = "Getting customer succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return cust;
        }

        // Employee

        public Employee CreateEmployee(Employee emp, out string status)
        {
            status = "Employee insertion failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "EXEC spEmployee_Insert" +
                        " @empFirst = '" + emp.EmpFirst + "',"
                        + " @empLast = '" + emp.EmpLast + "',"
                        + " @posID = " + emp.PosID + ","
                        + " @empUser = '" + emp.EmpUser + "',"
                        + " @empPassword = '" + emp.EmpPassword + "'";
                    SqlDataReader records = cmd.ExecuteReader();
                    if (records.Read())
                    {
                        emp.ID = records.GetInt32(0);
                    }
                }
                status = "Employee insertion succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }
            return emp;
        }

        //Authentication

        public int AuthenticateLogin(string user, string password, out string status)
        {
            int empID = -1;
            status = "Authentication process failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "SELECT id, empUser, empPassword FROM employee WHERE empUser = '" + user +"'";
                    SqlDataReader records = cmd.ExecuteReader();
                    if (records.Read())
                    {
                       if (password == records.GetString(2))
                        {
                            status = "Authentication successful.";
                            empID = records.GetInt32(0);
                        } else
                        {
                            status = "Password incorrect.";
                        }
                    } else
                    {
                        status = "Username does not exist.";
                    }
                }
            }
            catch (Exception ex) { status = ex.Message; }

            return empID;
        }
    }
}
