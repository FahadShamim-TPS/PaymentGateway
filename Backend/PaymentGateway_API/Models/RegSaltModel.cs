using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGateway_API.Models
{
    public class RegSaltModel
    {
        public long? CardNumber { get; set; }
        public string Salt { get; set; }
        public int CutomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public int? CVV_Code { get; set; }
        public string TokenCode { get; set; }
        public int paymentId { get; set; }
    }
}