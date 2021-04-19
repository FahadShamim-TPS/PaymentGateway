using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGateway_MVC.Models
{
    public class Payment
    {
        public int payment_id { get; set; }
        public int amount { get; set; }
        public int customer_id { get; set; }
        public int token_id { get; set; }
    }
}