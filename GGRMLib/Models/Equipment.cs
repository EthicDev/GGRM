using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    class Equipment
    {
        public int ID { get; set; }

        public string EquModel { get; set; }

        public string EquSerial { get; set; }

        public int CustID { get; set; }

        public int EquTypeID { get; set; }

        public int EquManuID { get; set; }
    }
}
