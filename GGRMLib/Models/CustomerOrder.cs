using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    //Coded by: Macklem Curtis
    //Date: Nov/Dec 2019
    public class CustomerOrder
    {
        public CustomerOrder()
        {
            ID = -1;
            PaymentID = -1;
            OrdPaid = false;
            OrdTotal = 0.00m;
            OrdPartyPlan = 0.00m;
            orderLines = new BindingList<OrderLine>();
            serviceOrders = new BindingList<ServiceOrder>();
        }

        public int ID { get; set; }

        public int OrdNumber { get; set; }

        public DateTime OrdCreationDate { get; set; }

        public decimal OrdTotal { get; set; }

        public decimal OrdPartyPlan { get; set; }

        public bool OrdPaid { get; set; }

        public int PaymentID { get; set; }

        public int CustID { get; set; }

        public int EmpID { get; set; }

        public BindingList<OrderLine> orderLines { get; set; }
        public BindingList<ServiceOrder> serviceOrders { get; set; }
    }
}
