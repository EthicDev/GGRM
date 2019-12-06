using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class ProductOrder
    {
        public ProductOrder()
        {
            ID = -1;
            orderLines = new BindingList<OrderLine>();
            PordStatus = "Not Ordered";
            PordNumber = 0;
            PordPaid = false;
        }
        public int ID { get; set; }

        public int PordNumber { get; set; }

        public string PordStatus { get; set; }

        public DateTime PordDateOrdered { get; set; }

        public DateTime PordDateReceived { get; set; }

        public bool PordPaid { get; set; }

        public BindingList<OrderLine> orderLines { get; set; }
    }
}
