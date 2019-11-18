using GGRMLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.DataAccess
{
    public interface IDataConnection
    {
        CustomerOrderLine CreateCOLine(CustomerOrderLine col, out string status);

        CustomerOrder CreateCO(CustomerOrder co, out string status);

        Customer CreateCustomer(Customer cust, out string status);
    }
}
