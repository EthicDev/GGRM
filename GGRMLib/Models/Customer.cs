using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class Customer
    {
        public int id { get; set; }

        public Customer()
        {

        }

        public Customer(string custFirst, string custLast, string custPhone, string custAddress, string custCity, string custPostal, string custEmail)
        {

        }

        public string custFirst { get; set; }

        public string custLast { get; set; }

        public string custPhone { get; set; }

        public string custAddress { get; set; }

        public string custCity { get; set; }

        public string custPostal { get; set; }

        public string custEmail { get; set; }
    }
}
