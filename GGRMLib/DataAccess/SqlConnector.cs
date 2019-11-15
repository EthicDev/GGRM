using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGRMLib.Models;

namespace GGRMLib.DataAccess
{
    class SqlConnector : IDataConnection
    {
        public CustomerOrder CreateCO(CustomerOrder co)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConString("GGRM")))
            {

            }
            //co.id = 4;

            return co;
        }

        public CustomerOrderLine CreateCOLine(CustomerOrderLine col)
        {
            throw new NotImplementedException();
        }

        public Customer CreateCustomer(Customer cust)
        {
            cust.ID = 10;

            return cust;
        }
    }
}
