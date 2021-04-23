using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGateway_MVC.Models
{
    public class CustomerApprovalModel
    {
        public int customer_id { get; set; }
        public int payment_id { get; set; }
        public string email { get; set; }

        public string token { get; set; }
    }
}