using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaymentGateway_API.Models
{
    public class Payment
    {
        [Key]
        public int payment_id { get; set; }
        public int amount { get; set; }
        public int customer_id { get; set; }
        public int token_id { get; set; }
        public string status { get; set; } 
    }
}