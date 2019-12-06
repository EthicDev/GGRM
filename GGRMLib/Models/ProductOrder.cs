using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    class ProductOrder
    {
        public ProductOrder()
        {
            orderLines = new BindingList<OrderLine>();
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
