using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib.Models
{
    //Coded by: Macklem Curtis
    //Date: Nov/Dec 2019
    class Product
    {
        public int ID { get; set; }

        public string ProdName { get; set; }

        public string ProdDescription { get; set; }

        public string ProdBrand { get; set; }

        public decimal ProdSize { get; set; }

        public decimal ProdPrice { get; set; }

        public string ProdMeasure { get; set; }
    }
}
