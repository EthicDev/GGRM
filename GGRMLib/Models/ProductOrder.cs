using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    class ProductOrder
    {
        public int ID { get; set; }

        public int PordNumber { get; set; }

        public DateTime PordDateOrdered { get; set; }

        public bool PordPaid { get; set; }
    }
}
