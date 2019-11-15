using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGRMLib.Models;

namespace GGRMLib
{
    class SqlConnector : IDataConnection
    {
        public CustomerOrder CreateCO(CustomerOrder co)
        {
            co.id = 4;

            return co;
        }

        public CustomerOrderLine CreateCOLine(CustomerOrderLine col)
        {
            throw new NotImplementedException();
        }

        public Customer CreateCustomer(Customer cust)
        {
            cust.id = 10;

            return cust;
        }
    }
}
