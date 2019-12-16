using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGRMLib.Models;

namespace GGRMLib.DataAccess
{
    //Coded by: Macklem Curtis & Cooper Keddy
    //Date: Nov/Dec 2019

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
                    + " @ordTotal = '" + co.OrdTotal + "',"
                    + " @ordPartyPlan = '" + co.OrdPartyPlan + "',"
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
                    string sqlCommand = "SELECT id, ordNumber, ordCreationDate, ordTotal, ordPartyPlan, ordPaid, paymentID, custID, empID FROM customer_order";
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
                string prodOrdID, custOrdID, serOrdID;
                if (col.ProdOrderID == null)
                {
                    prodOrdID = "NULL";
                } else
                {
                    prodOrdID = col.ProdOrderID.ToString();
                }
                if (col.OrderID == null)
                {
                    custOrdID = "NULL";
                }
                else
                {
                    custOrdID = col.OrderID.ToString();
                }
                if (col.ServiceOrderID == null)
                {
                    serOrdID = "NULL";
                }
                else
                {
                    serOrdID = col.ServiceOrderID.ToString();
                }
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)connection;
                cmd.CommandText = "EXEC spOrderLine_Insert" +
                    " @orlPrice = " + col.ColPrice.ToString() + ","
                    + " @orlOrderQuantity = " + col.ColOrderQuantity.ToString() + ","
                    + " @orlOrderReq = " + col.ColOrderReq.ToString() + ","
                    + " @orlNote = '" + col.ColNote + "',"
                    + " @orlWarranty = '" + col.ColWarranty + "',"
                    + " @inventoryID = " + col.InventoryID.ToString() + ","
                    + " @custOrdID = " + custOrdID + ","
                    + " @prodOrdID = " + prodOrdID + ","
                    + " @serOrdID = " + serOrdID;
                SqlDataReader records = cmd.ExecuteReader();
                if (records.Read())
                {
                    col.ID = Convert.ToInt32(records[0]);
                }
            }
            return col;
        }

        public DataTable GetOrderLinesByProdOrdIDs (int[] ids, out string status)
        {
            DataTable dtOrderLines = new DataTable();
            status = "Getting Order Lines failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string idSelector = "";
                    for (int i = 0; i < ids.Length; i++)
                    {
                        idSelector += ids[i].ToString();
                        if (i != ids.Length - 1) idSelector += " OR order_line.prodOrdID = ";
                    }
                    string sqlCommand = "SELECT order_line.id, pordNumber, prodName, prodDescription, prodBrand, CONVERT(varchar,prodSize) + ' ' + prodMeasure AS [Size], orlPrice, invQuantity, orlOrderQuantity, orlNote, pordRequestSource, empFirst + ' ' + empLast AS [Requesting Employee], inventoryID, custOrdID, prodOrdID, serOrdID, orlSupplierQuantityOrdered FROM order_line JOIN inventory ON order_line.inventoryID = inventory.id JOIN product ON inventory.productID = product.id JOIN prod_order ON order_line.prodOrdID = prod_order.id JOIN employee ON employee.ID = prod_order.requestingEmpID WHERE order_line.prodOrdID = " + idSelector;
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtOrderLines);
                }
                status = "Getting Order Lines succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtOrderLines;
        }

        public BindingList<OrderLine> GetServicePartsListByServOrdID (int id, out string status)
        {
            BindingList<OrderLine> blServiceParts = new BindingList<OrderLine>();
            status = "Getting Order Lines failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT order_line.id, orlPrice, orlOrderQuantity, invQuantity, invPrice, orlOrderReq, orlNote, orlWarranty, inventoryID, custOrdID, prodOrdID, serOrdID FROM order_line JOIN inventory ON order_line.InventoryID = inventory.ID WHERE order_line.serOrdID = " + id.ToString();

                    SqlDataReader records = cmd.ExecuteReader();
                    while (records.Read())
                    {
                        OrderLine part = new OrderLine();
                        part.ID = records.GetInt32(0);
                        part.ColPrice = records.GetDecimal(1);
                        part.ColOrderQuantity = records.GetInt32(2);
                        part.ColStockQuantity = records.GetInt32(3);
                        part.ColPrice = records.GetDecimal(4);
                        part.ColOrderReq = records.GetBoolean(5);
                        part.ColNote = records.GetString(6);
                        part.ColWarranty = records.GetBoolean(7);
                        part.InventoryID = records.GetInt32(8);
                        if (records.IsDBNull(9)) part.OrderID = null;
                        else part.OrderID = (int)records[9];
                        if (records.IsDBNull(10)) part.ProdOrderID = null;
                        else part.ProdOrderID = (int)records[10];
                        if (records.IsDBNull(11)) part.ServiceOrderID = null;
                        else part.ServiceOrderID = (int)records[11];
                        blServiceParts.Add(part);
                    }      
                }
                status = "Getting Order Lines succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return blServiceParts;
        }

        public int EditOrderLineSupplierQuantity (int id, int quantity, out string status)
        {
            status = "Order Line update failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "UPDATE order_line SET" +
                        " orlSupplierQuantityOrdered = " + quantity.ToString()
                        + " WHERE id = " + id.ToString();
                    cmd.ExecuteNonQuery();
                }
                status = "Order Line update succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }
            return id;
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
                        + " @empPassword = '" + emp.EmpPassword + "'"
                        + " @empDisabled = '" + emp.EmpDisabled;
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
                    string sqlCommand = "SELECT employee.id, empUser, empFirst, empLast, posName FROM employee JOIN position ON employee.posID = position.id WHERE empFirst LIKE '%" + searchString + "%' OR empLast LIKE '%" + searchString + "%'";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtEmployees);
                }
                status = "Getting inventory succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtEmployees;
        }

        public Employee GetEmployeeByID (int id, out string status)
        {
            Employee emp = new Employee();
            status = "Getting employee failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "SELECT employee.id, empFirst, empLast, posID, posName, empUser, empPassword FROM employee JOIN position on employee.posID = position.id WHERE employee.id = " + id.ToString();
                    SqlDataReader records = cmd.ExecuteReader();
                    if (records.Read())
                    {
                        emp.ID = (int)records[0];
                        emp.EmpFirst = (string)records[1];
                        emp.EmpLast = (string)records[2];
                        emp.PosID = (int)records[3];
                        emp.PosName = (string)records[4];
                        emp.EmpUser = (string)records[5];
                        emp.EmpPassword = (string)records[6];

                    }
                }
                status = "Getting employee succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return emp;
        }

        public Employee EditEmployee (Employee emp, out string status)
        {
            status = "Employee update failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "UPDATE employee SET" +
                        " empFirst = '" + emp.EmpFirst + "',"
                        + " empLast = '" + emp.EmpLast + "',"
                        + " posID = '" + emp.PosID + "',"
                        + " empUser = '" + emp.EmpUser + "',"
                        + " empPassword = '" + emp.EmpPassword + "',"
                        + " empDisabled = '" + emp.EmpDisabled + "'"
                        + " WHERE id = " + emp.ID;
                    cmd.ExecuteNonQuery();
                }
                status = "Employee update succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }
            return emp;
        }

        public Employee ToggleEmployeeDisabled (Employee emp, out string status)
        {
            status = "Employee update failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    if(emp.EmpDisabled)
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = (SqlConnection)connection;
                        cmd.CommandText = "UPDATE employee SET empDisabled = 0 WHERE id = " + emp.ID;
                        cmd.ExecuteNonQuery();
                        status = "Employee enabled.";
                    } else
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = (SqlConnection)connection;
                        cmd.CommandText = "UPDATE employee SET empDisabled = 1 WHERE id = " + emp.ID;
                        cmd.ExecuteNonQuery();
                        status = "Employee disabled.";
                    }
                    
                }
                
            } catch (Exception ex)
            {
                status = ex.Message;
            }
            return emp;
        }

        //Position

        public DataTable GetPositionDataTable (out string status, string searchString = "")
        {
            DataTable dtPosition = new DataTable();

            status = "Getting positions failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT id, posName FROM position WHERE posName LIKE '%" + searchString + "%' AND id <= 5";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtPosition);
                }
                status = "Getting positions succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtPosition;
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
                    string sqlCommand = "SELECT inventory.id, inventory.invQuantity, prodName AS [Name], prodDescription AS [Description], prodBrand AS [Brand], prodSize, prodMeasure, CONVERT(varchar,prodSize) + ' ' + prodMeasure AS [Size], invPrice AS [Price] FROM inventory JOIN product ON inventory.productID = product.id WHERE prodName LIKE '%" + searchString + "%'";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtInventory);
                }
                status = "Getting inventory succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtInventory;
        }

        public Inventory GetInventoryByID(int id, out string status)
        {
            Inventory inv = new Inventory();
            status = "Getting inventory failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "SELECT inventory.id, invQuantity, invPrice, productID, prodName, prodDescription, prodBrand FROM inventory JOIN product ON inventory.productID = product.id WHERE inventory.id = " + id.ToString();
                    SqlDataReader records = cmd.ExecuteReader();
                    if (records.Read())
                    {
                        inv.ID = (int)records[0];
                        inv.InvQuantity = (int)records[1];
                        inv.InvPrice = (decimal)records[2];
                        inv.ProductID = (int)records[3];
                        inv.DisplayName = records[4].ToString() + "\n" + records[5].ToString() + "\n" + records[6].ToString() + "\n";
                    }
                }
                status = "Getting inventory succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return inv;
        }

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

        public int UpdateStockByID(int idToUpdate, int quantityChanged, out string status)
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
                        " invQuantity = invQuantity + " + quantityChanged.ToString()
                        + " WHERE id = " + idToUpdate;
                    cmd.ExecuteNonQuery();
                }
                status = "Inventory update succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }
            return idToUpdate;
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
                    + " @requestingEmpID = '" + so.RequestingEmpID + "',"
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
                    string sqlCommand = "SELECT service_order.id, serordDateIn AS [Intake Date], serordIssue AS [Issue], serordWarranty AS [Warranty?], serordStatus AS [Status], custFirst + ' ' + custLast AS [Customer], e1.empFirst + ' ' + e1.empLast AS [Requesting Employee], e2.empFirst + ' ' + e2.empLast AS [Assigned Technician], serName AS [Service Name], eqtType AS [Equip Type], equModel AS [Equip Model] FROM service_order JOIN [dbo].[service] ON service_order.serviceID = [dbo].[service].[id] JOIN equipment ON service_order.equipID = equipment.id JOIN equip_type ON equip_type.id = equipment.equtypeID JOIN employee e1 ON service_order.requestingEmpID = e1.id LEFT JOIN employee e2 ON service_order.technicianID = e2.id JOIN customer_order ON service_order.custOrdID = customer_order.id JOIN customer ON customer_order.custID = customer.id";
                    using SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtPendingServices);

                }
                status = "Getting services succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtPendingServices;
        }

        public ServiceOrder GetServiceOrderByID(int id, out string status)
        {
            ServiceOrder sord = new ServiceOrder();
            status = "Getting ServiceOrder failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "SELECT service_order.id, serordNumber, serordDateIn, serordDateOut, serordIssue, serordWarranty, serordStatus, custOrdID, serviceID, equipID, equModel, equSerial, eqtType, manName, requestingEmpID, technicianID FROM service_order JOIN equipment ON equipment.id = service_order.equipID JOIN equip_type ON equipment.equtypeID = equip_type.id JOIN manufacturer ON manufacturer.id = equipment.equManuID WHERE service_order.id = " + id.ToString();
                    SqlDataReader records = cmd.ExecuteReader();
                    if (records.Read())
                    {
                        sord.ID = (int)records[0];
                        sord.SerOrdNumber = (string)records[1];
                        sord.SerOrdDateIn = (DateTime)records[2];
                        sord.SerOrdDateOut = (DateTime)records[3];
                        sord.SerOrdIssue = (string)records[4];
                        sord.SerOrdWarranty = (bool)records[5];
                        sord.SerOrdStatus = (string)records[6];
                        sord.CustOrdID = (int)records[7];
                        sord.ServiceID = (int)records[8];
                        sord.EquipID = (int)records[9];
                        sord.EquipName = (string)records[12] + " - " + (string)records[13] + '\n' + (string)records[10] + '\n' + (string)records[11];
                        sord.RequestingEmpID = (int)records[14];
                        sord.TechnicianID = (int)records[15];
                    }
                }
                status = "Getting ServiceOrder succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return sord;
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

        public ServiceOrder EditServiceOrder (ServiceOrder so, out string status)
        {
            status = "Service Order update failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "UPDATE service_order SET" +
                        " serordDateOut = '" + so.SerOrdDateOut + "',"
                        + " serordStatus = '" + so.SerOrdStatus + "',"
                        + " serordDiagnosis = '" + so.SerOrdDiagnosis + "',"
                        + " technicianID = '" + so.TechnicianID + "'"
                        + " WHERE id = " + so.ID;
                    cmd.ExecuteNonQuery();
                }
                status = "Service Order update succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }
            return so;
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
                    + " @pordDateCreated = '" + DateTime.UtcNow + "',"
                    + " @pordDateReceived = '" + po.PordDateReceived + "',"
                    + " @pordStatus = '" + po.PordStatus + "',"
                    + " @pordPaid = " + po.PordPaid + ","
                    + " @pordRequestSource = '" + po.PordRequestSource + "',"
                    + " @requestingEmpID = " + po.RequestingEmpID;
                SqlDataReader records = cmd.ExecuteReader();
                if (records.Read())
                {
                    po.ID = Convert.ToInt32(records[0]);
                }
            }
            return po;
        }

        public DataTable GetProductOrderRequestsDataTable (out string status)
        {
            DataTable dtProductOrderRequests = new DataTable();

            status = "Getting ProductOrders failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT prod_order.id, pordNumber AS [PO #], pordDateCreated AS [Date Created], empFirst + ' ' + empLast AS [Requesting Employee], pordRequestSource AS [Request Source], pordStatus AS [Status] FROM prod_order JOIN employee ON prod_order.requestingEmpID = employee.id WHERE pordStatus = 'Requested'";
                    using SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtProductOrderRequests);

                }
                status = "Getting ProductOrders succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtProductOrderRequests;
        }

        public DataTable GetPendingProductOrdersDataTable(out string status)
        {
            DataTable dtPendingProductOrders = new DataTable();

            status = "Getting ProductOrders failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT prod_order.id, pordNumber AS [PO #], pordSupplierOrdNum AS [Supplier Order #], pordDateOrdered as [Date Ordered], empFirst + ' ' + empLast AS [Ordering Employee], pordStatus AS [Status] FROM prod_order LEFT JOIN employee ON prod_order.orderingEmpID = employee.id WHERE pordStatus = 'Ordered' OR pordStatus = 'Delivered'";
                    using SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand, conn);
                    sqlDa.Fill(dtPendingProductOrders);

                }
                status = "Getting ProductOrders succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return dtPendingProductOrders;
        }

        public ProductOrder GetProductOrderByID(int id, out string status)
        {
            ProductOrder pord = new ProductOrder();
            status = "Getting Product Order failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "SELECT id, pordNumber, pordSupplierOrdNum, pordStatus, pordDateCreated, pordDateOrdered, pordDateReceived, pordPaid, requestingEmpID, orderingEmpID FROM prod_order WHERE id = " + id.ToString();
                    SqlDataReader records = cmd.ExecuteReader();
                    if (records.Read())
                    {
                        pord.ID = (int)records[0];
                        pord.PordNumber = (string)records[1];
                        pord.PordSupplierOrderNum = (string)records[2];
                        pord.PordStatus = (string)records[3];
                        pord.PordDateCreated = Convert.ToDateTime(records[4]);
                        pord.PordDateOrdered = Convert.ToDateTime(records[5]);
                        pord.PordDateReceived = Convert.ToDateTime(records[6]);
                        pord.PordPaid = (bool)records[7];
                        pord.RequestingEmpID = (int)records[8];
                        pord.OrderingEmpID = (int)records[9];
                    }
                }
                status = "Getting Product Order succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }

            return pord;
        }

        public ProductOrder EditProductOrder(ProductOrder po, out string status)
        {
            status = "Product Order update failed.";
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)connection;
                    cmd.CommandText = "UPDATE prod_order SET" +
                        " pordSupplierOrdNum = '" + po.PordSupplierOrderNum + "',"
                        + " pordStatus = '" + po.PordStatus + "',"
                        + " pordDateOrdered = '" + po.PordDateOrdered.ToString() + "',"
                        + " orderingEmpID = '" + po.OrderingEmpID + "'"
                        + " WHERE id = " + po.ID;
                    cmd.ExecuteNonQuery();
                }
                status = "Product Order update succeeded.";
            }
            catch (Exception ex) { status = ex.Message; }
            return po;
        }

        // Products

        public DataTable GetProductsDataTable (out string status, string searchString = "")
        {
            DataTable dtProducts = new DataTable();

            status = "Getting products failed.";
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalConfig.ConString("GGRM")))
                {
                    conn.Open();
                    string sqlCommand = "SELECT id, prodName, prodDescription, prodBrand, prodSize, prodMeasure, prodPrice, CONVERT(varchar, prodSize) + ' ' + prodMeasure AS [Size] FROM product WHERE prodName LIKE '%" + searchString + "%' OR prodDescription LIKE '%" + searchString + "%' OR prodBrand LIKE '%" + searchString + "%' OR prodSize LIKE '%" + searchString + "%' OR prodMeasure LIKE '%" + searchString + "%'";
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
