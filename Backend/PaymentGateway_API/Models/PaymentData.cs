using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGateway_API.Models
{
    public class PaymentData
    {
        public int? Customer_ID { get; set; }
        public int? Token_ID { get; set; }
        public int? Amount { get; set; }
    }
}