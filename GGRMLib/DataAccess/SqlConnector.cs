﻿using System;
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
                    + " @empID = " + co.EmpID;
                SqlDataReader records = cmd.ExecuteReader();
                if (records.Read())
                {
                    co.ID = records.GetInt32(0);
                }
            }

            return co;
        }

        public DataTable GetCustomerOrdersDataTable (out string status)
        {
            DataTable dtCustomerOrders = new DataTable();

            status = "Getting CustomerOrders failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT id, ordNumber, ordCreationDate, ordPaid, paymentID, custID, empID FROM customer_order";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtCustomerOrders);
                }
                status = "Getting CustomerOrders succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtCustomerOrders;
        }

        //OrderLine
        public OrderLine CreateOrderLine(OrderLine col, out string status)
        {
            
            status = "CustomerOrderLine insertion failed.";
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
            {
                string prodOrdID;
                if (col.ProdOrderID == null)
                {
                    prodOrdID = "NULL";
                } else
                {
                    prodOrdID = col.ProdOrderID.ToString();
                }
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)connection;
                cmd.CommandText = "EXEC spOrderLine_Insert" +
                    " @orlPrice = " + col.ColPrice.ToString() + ","
                    + " @orlOrderQuantity = " + col.ColOrderQuantity.ToString() + ","
                    + " @orlOrderReq = " + col.ColOrderReq.ToString() + ","
                    + " @orlNote = '" + col.ColNote + "',"
                    + " @inventoryID = " + col.InventoryID.ToString() + ","
                    + " @custOrdID = " + col.OrderID.ToString() + ","
                    + " @prodOrdID = " + prodOrdID;
                SqlDataReader records = cmd.ExecuteReader();
                if (records.Read())
                {
                    col.ID = Convert.ToInt32(records[0]);
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

        public DataTable GetEmployeeDataTable (out string status, string searchString = "")
        {
            DataTable dtEmployees = new DataTable();

            status = "Getting employees failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT employee.id, empFirst, empLast, posName FROM employee JOIN position ON employee.posID = position.id WHERE empFirst LIKE '%" + searchString + "%' OR empLast LIKE '%" + searchString + "%'";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtEmployees);
                }
                status = "Getting inventory succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtEmployees;
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
                    string sqlCommand = "SELECT inventory.id, prodName AS [Name], prodDescription AS [Description], prodBrand AS [Brand], invQuantity AS [Quantity], CONVERT(varchar,prodSize) + ' ' + prodMeasure AS [Size], invPrice AS [Unit Price] FROM inventory JOIN product ON inventory.productID = product.id WHERE prodName LIKE '%" + searchString + "%'";
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
                    string sqlCommand = "SELECT inventory.id, inventory.invQuantity, prodName AS [Name], prodDescription AS [Description], prodBrand AS [Brand], CONVERT(varchar,prodSize) + ' ' + prodMeasure AS [Size], invPrice AS [Price] FROM inventory JOIN product ON inventory.productID = product.id WHERE prodName LIKE '%" + searchString + "%'";
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
                        + " invPrice = '" + inv.InvPrice + "'"
                        + " WHERE id = " + inv.ID;
                    cmd.ExecuteNonQuery();
                }
                status = "Inventory update succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }
            return inv;
        }

        // Repairs (Services)

        public ServiceOrder CreateServiceOrder (ServiceOrder so, out string status)
        {
            status = "ServiceOrder insertion failed.";
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)connection;
                cmd.CommandText = "EXEC spServiceOrder_Insert" +
                    " @serordDateIn = '" + so.SerOrdDateIn + "',"
                    + " @serordDateOut = '" + so.SerOrdDateOut + "',"
                    + " @serordIssue = '" + so.SerOrdIssue + "',"
                    + " @serordWarranty = '" + so.SerOrdWarranty + "',"
                    + " @serordStatus = '" + so.SerOrdStatus + "',"
                    + " @serviceID = '" + so.ServiceID + "',"
                    + " @equipID = '" + so.EquipID + "',"
                    + " @empID = '" + so.EmpID + "',"
                    + " @custOrdID = '" + so.CustOrdID + "'";
                SqlDataReader records = cmd.ExecuteReader();
                if (records.Read())
                {
                    so.ID = records.GetInt32(0);
                }
            }
            return so;
        }

        public DataTable GetPendingServicesDataTable (out string status, string searchString = "")
        {
            DataTable dtPendingServices = new DataTable();

            status = "Getting services failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT service_order.id, serordDateIn, serordIssue, serordWarranty, serordStatus, custOrdID, empID, serName, eqtType, equModel FROM service_order JOIN service ON service_order.serviceID = service.id JOIN equipment ON service_order.equipID = equipment.id JOIN equip_type ON equip_type.id = equipment.equtypeID WHERE serordStatus = 'Pending'";
                    using SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtPendingServices);

                }
                status = "Getting services succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtPendingServices;
        }

        public DataTable GetServiceTypesDataTable (out string status)
        {
            DataTable dtServiceTypes = new DataTable();

            status = "Getting services failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT id, serName, serDescription, serPrice FROM service";
                    using SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtServiceTypes);

                }
                status = "Getting services succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtServiceTypes;
        }

        // Equipment

        public DataTable GetEquipmentByCustID (int custID, out string status)
        {
            DataTable dtEquipment = new DataTable();
            status = "Getting equipment failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT equipment.id, eqtType + ' - ' + equModel AS [Info], equModel, equSerial, custID, equtypeID, equManuID FROM equipment JOIN equip_type ON equipment.equtypeID = equip_type.id WHERE custID = " + custID.ToString();
                    using SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtEquipment);
                }
                status = "Getting equipment succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtEquipment;
        }

        // ProductOrder

        public ProductOrder CreateProductOrder (ProductOrder po, out string status)
        {
            status = "ProductOrder insertion failed.";
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)connection;
                cmd.CommandText = "EXEC spProductOrder_Insert" +
                    " @pordDateOrdered = '" + po.PordDateOrdered + "',"
                    + " @pordDateReceived = '" + po.PordDateReceived + "',"
                    + " @pordNumber = " + po.PordNumber + ","
                    + " @pordStatus = '" + po.PordStatus + "',"
                    + " @pordPaid = " + po.PordPaid;
                SqlDataReader records = cmd.ExecuteReader();
                if (records.Read())
                {
                    //Not getting id from SQL properly
                    //var test = records[0];
                    //Console.WriteLine("@@DEBUG@@ Test value is equal to: "+test.ToString());
                    po.ID = Convert.ToInt32(records[0]);
                }
            }
            return po;
        }

        // Products

        public DataTable GetProductsDataTable (out string status)
        {
            DataTable dtProducts = new DataTable();

            status = "Getting products failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT id, prodName, prodDescription, prodBrand, prodSize, prodMeasure FROM product";
                    using SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtProducts);

                }
                status = "Getting products succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtProducts;
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
