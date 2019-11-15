using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class CustomerOrder
    {
        public int id { get; set; }

        public int ordNumber { get; set; }

        public bool ordPaid { get; set; }

        public DateTime ordDate { get; set; }

        public int paymentID { get; set; }

        public int custID { get; set; }

        public int empID { get; set; }
    }
}
