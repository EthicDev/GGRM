using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class CustomerOrderLine
    {
        public int ID { get; set; }

        public double ColPrice { get; set; }

        public int ColQuantity { get; set; }

        public string ColNote { get; set; }

        public bool? UnderWarranty { get; set; }

        public int? InventoryID { get; set; }

        public int? ServiceID { get; set; }

        public int OrderID { get; set; }
    }
}
