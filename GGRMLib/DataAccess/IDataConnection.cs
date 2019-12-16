using GGRMLib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.DataAccess
{
    //Coded by: Macklem Curtis
    //Date: Nov/Dec 2019
    public interface IDataConnection
    {
        OrderLine CreateCOLine(OrderLine col, out string status);

        CustomerOrder CreateCO(CustomerOrder co, out string status);

        Customer CreateCustomer(Customer cust, out string status);

        List<Customer> GetCustomersList(out string status);

        DataTable GetCustomersDataTable(out string status);

        Customer GetCustomerByID(int id, out string status);

        int AuthenticateLogin(string user, string password, out string status);
    }
}
