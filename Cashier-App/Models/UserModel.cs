using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Cashier_App.Models
{
    public class UserModel
    {
        public int AccountID { get; set; }
        //[DisplayName("Product Name")]
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public DateTime RegisterTime { get; set; } = DateTime.Now;
    }
}