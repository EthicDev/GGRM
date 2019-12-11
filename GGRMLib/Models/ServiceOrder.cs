using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class ServiceOrder
    {
        public ServiceOrder()
        {
            ID = -1;
        }

        public int ID { get; set; }

        public string ServiceName { get; set; }

        public DateTime SerOrdDateIn { get; set; }

        public DateTime SerOrdDateOut { get; set; }

        public string SerOrdIssue { get; set; }

        public bool SerOrdWarranty { get; set; }

        public string SerOrdStatus { get; set; }

        public int CustOrdID { get; set; }

        public int ServiceID { get; set; }

        public int EquipID { get; set; }

        public int EmpID { get; set; }
    }
}
