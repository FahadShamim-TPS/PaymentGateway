using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGateway_API.Models
{
    public class DataForAuthorization
    {
        public int customerId { get; set; }
        public string tokenCode { get; set; }
        
    }
}