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
    public class ServiceOrder
    {
        public ServiceOrder()
        {
            ID = -1;
            serviceParts = new BindingList<OrderLine>();
        }

        public int ID { get; set; }

        public string SerOrdNumber { get; set; }

        public string ServiceName { get; set; }

        public DateTime SerOrdDateIn { get; set; }

        public DateTime SerOrdDateOut { get; set; }

        public string SerOrdIssue { get; set; }

        public bool SerOrdWarranty { get; set; }

        public string SerOrdStatus { get; set; }

        public string SerOrdDiagnosis { get; set; }

        public int CustOrdID { get; set; }

        public int ServiceID { get; set; }

        public int EquipID { get; set; }

        public string EquipName { get; set; }

        public int RequestingEmpID { get; set; }

        public int TechnicianID { get; set; }

        public BindingList<OrderLine> serviceParts { get; set; }
        public string SerOrdRepairNote { get; set; }
    }
}
