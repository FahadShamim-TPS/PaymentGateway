using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGateway_API.Models
{
    public class PostmanDataForAuthorization
    {
        public PostmanDataForAuthorization()
        {
            customerId = 0;
            tokenCode = string.Empty;
            status = string.Empty;
            tokenId = 0;
            paymentId = 0;
        }
        public int customerId { get; set; }
        public string tokenCode { get; set; }
        public string status { get; set; }
        public int? tokenId { get; set; }
        public int? paymentId { get; set; }
        public string CustomerFirstName { get; set; }
        public long CarNumber { get; set; }
        public string Salt { get; set; }
        public int? CVV_Code { get; set; }
    }
}