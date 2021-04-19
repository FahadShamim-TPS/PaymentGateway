using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGateway_MVC.Models
{
    public class TokenAndCutomerDetails
    {
        public int Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Email { get; set; }
        public long Card_Number { get; set; }
        public int amount { get; set; }
        public string Token { get; set; }

        public int Token_Id { get; set; }

    }
}