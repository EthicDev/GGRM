using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class Customer
    {
        public int ID { get; set; }

        public Customer()
        {

        }

        public Customer(string custFirst, string custLast, string custPhone, string custAddress, string custCity, string custPostal, string custEmail)
        {
            CustFirst = custFirst;
            CustLast = custLast;
            CustPhone = custPhone;
            CustAddress = custAddress;
            CustCity = custCity;
            CustPostal = custPostal;
            CustEmail = custEmail;
        }

        public string CustFirst { get; set; }

        public string CustLast { get; set; }

        public string CustPhone { get; set; }

        public string CustAddress { get; set; }

        public string CustCity { get; set; }

        public string CustPostal { get; set; }

        public string CustEmail { get; set; }
    }
}
