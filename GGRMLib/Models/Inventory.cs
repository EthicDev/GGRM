using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class Inventory
    {
        public int ID { get; set; }

        public string DisplayName { get; set; }

        public int InvQuantity { get; set; }

        public double InvSize { get; set; }

        public string InvMeasure { get; set; }

        public decimal InvPrice { get; set; }

        public int ProductID { get; set; }
    }
}
