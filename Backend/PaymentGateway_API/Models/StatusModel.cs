using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGateway_API.Models
{
    public class StatusModel
    {    
        public string status { get; set; }
        public int? customerId { get; set; }
        public int? tokenId { get; set; }
        public int? paymentId { get; set; }

        public StatusModel(int payID,int custID)
        {
            this.paymentId = payID;
            this.customerId = custID;
        }
        public StatusModel()
        {
            this.paymentId = 0;
            this.customerId = 0;
        }


    }
}
