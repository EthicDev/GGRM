using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    public class Employee
    {
        public int ID { get; set; }

        public string EmpFirst { get; set; }

        public string EmpLast { get; set; }

        public int PosID { get; set; }

        public string PosName { get; set; }

        public string EmpUser { get; set; }

        public string EmpPassword { internal get; set; }

        public int EmpDisabled { get; set; }
    }
}
