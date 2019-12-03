using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class CustomerOrder
    {
        public CustomerOrder()
        {
            ID = -1;
            orderLines = new BindingList<CustomerOrderLine>();
            serviceOrders = new BindingList<ServiceOrder>();
        }

        public int ID { get; set; }

        public int OrdNumber { get; set; }

        public DateTime OrdCreationDate { get; set; }

        public bool OrdPaid { get; set; }

        public int PaymentID { get; set; }

        public int CustID { get; set; }

        public int EmpID { get; set; }

        public BindingList<CustomerOrderLine> orderLines { get; set; }
        public BindingList<ServiceOrder> serviceOrders { get; set; }
    }
}
