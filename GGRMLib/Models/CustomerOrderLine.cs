using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class CustomerOrderLine
    {
        public int id { get; set; }

        public double colPrice { get; set; }

        public int colQuantity { get; set; }

        public string colNote { get; set; }

        public bool? underWarranty { get; set; }

        public int? inventoryID { get; set; }

        public int? serviceID { get; set; }

        public int orderID { get; set; }
    }
}
