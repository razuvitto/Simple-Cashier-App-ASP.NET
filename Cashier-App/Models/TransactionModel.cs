using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Cashier_App.Models
{
    public class TransactionModel
    {
        public int TransactionID { get; set; }
        //[DisplayName("Product Name")]
        public int ProductID{ get; set; }
        public int Quantity { get; set; }
        public string TransactionNumber { get; set; }
        public int AccountID { get; set; }
        public int Total { get; set; }
    }
}