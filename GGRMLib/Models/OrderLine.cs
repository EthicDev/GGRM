using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class OrderLine
    {
        public OrderLine()
        {
            ProdOrderID = null;
            OrderID = null;
            ServiceOrderID = null;
        }
        public int ID { get; set; }

        public string ColItemName { get; set; }

        public string ColItemBrand { get; set; }

        public string ColItemDesc { get; set; }

        public decimal ColItemSize { get; set; }

        public string ColItemMeasure { get; set; }

        public decimal ColPrice { get; set; }

        public int ColStockQuantity { get; set; }

        public int ColOrderQuantity { get; set; }

        public bool ColOrderReq { get; set; }

        public string ColNote { get; set; }

        public int InventoryID { get; set; }

        public int? OrderID { get; set; }

        public int? ProdOrderID { get; set; }

        public int? ServiceOrderID { get; set; }


    }
}
