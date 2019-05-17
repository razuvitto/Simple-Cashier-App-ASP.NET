using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Cashier_App.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int Stock{ get; set; }
    }
}