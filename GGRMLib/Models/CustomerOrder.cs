using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    class CustomerOrder
    {
        public int ID { get; set; }

        public int OrdNumber { get; set; }

        public DateTime OrdCreationDate { get; set; }

        public bool OrdPaid { get; set; }

        public int PaymentID { get; set; }

        public int CustID { get; set; }

        public int EmpID { get; set; }
    }
}
