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

        public int CustomerOrderNextID(out string status)
        {
            int nextID = -1;
            status = "Getting CustomerOrderNextID failed.";
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString("GGRM")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)connection;
                cmd.CommandText = "SELECT MAX(ID) FROM customer_order";
                SqlDataReader records = cmd.ExecuteReader();
                if (records.Read())
                {
                    nextID = records.GetInt32(0)+1;
                }
            }

            return nextID;
    }
        public CustomerOrder CreateCustomerOrder(CustomerOrder co, out string status)
        {
            status = "CustomerOrder creation failed.";
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString("GGRM")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)connection;
                cmd.CommandText = "EXEC spCustomerOrder_Insert" +
                    " @ordNumber = '" + co.OrdNumber + "',"
                    + " @ordCreationDate = '" + co.OrdCreationDate + "',"
                    + " @ordPaid = '" + co.OrdPaid + "',"
                    + " @paymentID = '" + co.PaymentID + "',"
                    + " @custID = '" + co.CustID + "',"
                    + " @empID = '" + co.EmpID + "'";
                SqlDataReader records = cmd.ExecuteReader();
                if (records.Read())
                {
                    co.ID = records.GetInt32(0);
                }
            }

            return co;
        }

        //CustomerOrderLine
        public CustomerOrderLine CreateCustomerOrderLine(CustomerOrderLine col, out string status)
        {
            status = "CustomerOrderLine insertion failed.";
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)connection;
                cmd.CommandText = "EXEC spCustomerOrderLine_Insert" +
                    " @orlPrice = '" + col.ColPrice + "',"
                    + " @orlQuantity = '" + col.ColQuantity + "',"
                    + " @orlOrderReq = '" + col.ColOrderReq + "',"
                    + " @orlNote = '" + col.ColNote + "',"
                    + " @orlWarranty = '" + col.ColUnderWarranty + "',"
                    + " @serviceID = '" + col.ServiceID + "',"
                    + " @inventoryID = '" + col.InventoryID + "',"
                    + " @custOrdID = '" + col.OrderID + "'";
                SqlDataReader records = cmd.ExecuteReader();
                if (records.Read())
                {
                    col.ID = records.GetInt32(0);
                }
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
                    using SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "EXEC spCustomer_Insert" +
                        " @custFirst = '" + cust.CustFirst + "',"
                        + " @custLast = '" + cust.CustLast + "',"
                        + " @custPhone = '" + cust.CustPhone + "',"
                        + " @custAddress = '" + cust.CustAddress + "',"
                        + " @custCity = '" + cust.CustCity + "',"
                        + " @custPostal = '" + cust.CustPostal + "',"
                        + " @custEmail = '" + cust.CustEmail + "'";
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

        public List<Customer> GetCustomersList(out string status, string searchString = "")
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
                    cmd.CommandText = "SELECT id, custFirst, custLast, custPhone, custAddress, custCity, custPostal, custEmail FROM customer WHERE custFirst LIKE '%" + searchString + "%' OR custLast LIKE '%" + searchString + "%' ORDER BY custLast";
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
                    string sqlCommand = "SELECT id, custLast+', '+custFirst AS Name, custPhone AS Phone, custAddress AS Address, custCity AS City, custPostal AS [Postal Code], custEmail AS Email FROM customer WHERE custFirst LIKE '%"+searchString+ "%' OR custLast LIKE '%" + searchString + "%' ORDER BY custLast";
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

        // Inventory

        public DataTable GetInventoryDataTable(out string status, string searchString = "")
        {
            DataTable dtInventory = new DataTable();

            status = "Getting inventory failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT inventory.id, prodName AS [Name], prodDescription AS [Description], prodBrand AS [Brand], invQuantity AS [Quantity], CONVERT(varchar,invSize) + ' ' + invMeasure AS [Size], invPrice AS [Unit Price] FROM inventory JOIN product ON inventory.productID = product.id WHERE prodName LIKE '%" + searchString + "%'";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtInventory);
                }
                status = "Getting inventory succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtInventory;
        }

        public DataTable GetInventoryDataTableShort(out string status, string searchString = "")
        {
            DataTable dtInventory = new DataTable();

            status = "Getting inventory failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT inventory.id, prodName AS [Name], prodDescription AS [Description], prodBrand AS [Brand], CONVERT(varchar,invSize) + ' ' + invMeasure AS [Size], invPrice AS [Price] FROM inventory JOIN product ON inventory.productID = product.id WHERE prodName LIKE '%" + searchString + "%'";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtInventory);
                }
                status = "Getting inventory succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtInventory;
        }

        //public Inventory GetInventoryByID(int id, out string status)
        //{
        //    Inventory cust = new Inventory();
        //    status = "Getting inventory failed.";
        //    try
        //    {
        //        using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
        //        {
        //            connection.Open();
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.Connection = (SqlConnection)connection;
        //            cmd.CommandText = "SELECT id, invQuantity, WHERE id = " + id.ToString();
        //            SqlDataReader records = cmd.ExecuteReader();
        //            if (records.Read())
        //            {
        //                cust = new Customer(records.GetString(1),
        //                    records.GetString(2),
        //                    records.GetString(3),
        //                    records.GetString(4),
        //                    records.GetString(5),
        //                    records.GetString(6),
        //                    records.GetString(7));
        //                cust.ID = records.GetInt32(0);
        //            }
        //        }
        //        status = "Getting customer succeeded.";
        //    }
        //    catch (Exception ex) { status = ex.Message; }

        //    return cust;
        //}

        //public List<Inventory> GetInventoryList(out string status, string searchString = "")
        //{
        //    List<Inventory> listInv = new List<Inventory>();

        //    status = "Getting inventory failed.";
        //    try
        //    {
        //        using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
        //        {
        //            connection.Open();
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.Connection = (SqlConnection)connection;
        //            cmd.CommandText = "SELECT inventory.id, prodName, prodDescription, prodBrand, invQuantity, invSize, invMeasure, invPrice FROM inventory JOIN product ON inventory.productID = product.id WHERE prodName LIKE '%" + searchString + "%'";
        //            SqlDataReader records = cmd.ExecuteReader();
        //            while (records.Read())
        //            {
        //                Customer cust = new Customer(records.GetString(1),
        //                    records.GetString(2),
        //                    records.GetString(3),
        //                    records.GetString(4),
        //                    records.GetString(5),
        //                    records.GetString(6),
        //                    records.GetString(7));
        //                cust.ID = records.GetInt32(0);
        //                customers.Add(cust);
        //            }
        //        }
        //        status = "Getting customers succeeded.";
        //    }
        //    catch (Exception ex) { status = ex.Message; }

        //    return customers;
        //}

        public Inventory EditInventoryItem(Inventory inv, out string status)
        {
            status = "Inventory update failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "UPDATE inventory SET" +
                        " invQuantity = '" + inv.InvQuantity + "',"
                        + " invSize = '" + inv.InvSize + "',"
                        + " invMeasure = '" + inv.InvMeasure + "',"
                        + " invPrice = '" + inv.InvPrice + "'"
                        + " WHERE id = " + inv.ID;
                    cmd.ExecuteNonQuery();
                }
                status = "Inventory update succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }
            return inv;
        }

        // Repairs

        public DataTable GetPendingServicesDataTable (out string status, string searchString = "")
        {
            DataTable dtPendingServices = new DataTable();

            status = "Getting services failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT receiptID, eqtType, serordDateIn, serordIssue, serordWarranty, serordStatus FROM service_order JOIN receipt ON receipt.id = service_order.receiptID JOIN equipment ON service_order.equipID = equipment.ID JOIN equip_type ON equip_type.id = equipment.equtypeID";
                    using SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtPendingServices);

                }
                status = "Getting services succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtPendingServices;
        }

        //Authentication

        public int AuthenticateLogin(string user, string password, out string status)
        {
            int empID = -1;
            status = "Authentication process failed.";
            try
            {
                using IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM"));
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)connection;
                cmd.CommandText = "SELECT id, empUser, empPassword FROM employee WHERE empUser = '" + user + "'";
                SqlDataReader records = cmd.ExecuteReader();
                if (records.Read())
                {
                    if (password == records.GetString(2))
                    {
                        status = "Authentication successful.";
                        empID = records.GetInt32(0);
                    }
                    else
                    {
                        status = "Password incorrect.";
                    }
                }
                else
                {
                    status = "Username does not exist.";
                }
            }
            catch (Exception ex) { status = ex.Message; }

            return empID;
        }
    }
}
