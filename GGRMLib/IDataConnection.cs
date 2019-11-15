using GGRMLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib
{
    public interface IDataConnection
    {
        CustomerOrderLine CreateCOLine(CustomerOrderLine col);

        CustomerOrder CreateCO(CustomerOrder co);

        Customer CreateCustomer(Customer cust);
    }
}
