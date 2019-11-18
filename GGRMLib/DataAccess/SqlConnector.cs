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
    class SqlConnector : IDataConnection
    {
        public CustomerOrder CreateCO(CustomerOrder co, out string status)
        {
            status = "CustomerOrder creation failed.";
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString("GGRM")))
            {

            }
            //co.id = 4;

            return co;
        }

        public CustomerOrderLine CreateCOLine(CustomerOrderLine col, out string status)
        {
            throw new NotImplementedException();
        }

        public Customer CreateCustomer(Customer cust, out string status)
        {
            status = "Customer insertion failed.";
            //try
            //{
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
                status = "Customer insertion successful.";
            //} catch (Exception ex) { status = ex.Message; }
            return cust;
        }
    }
}
