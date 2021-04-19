using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGateway_MVC.Models
{
    public class PaymentData
    {
        public int? Customer_ID { get; set; }
        public string Token { get; set; }
        public int? Amount { get; set; }
        public int Token_ID { get; set; }


    }
}