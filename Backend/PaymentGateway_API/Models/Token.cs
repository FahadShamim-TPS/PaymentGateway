using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaymentGateway_API.Models
{
    public class Token
    {
        [Key]
        public int TokenID { get; set; }
        public string TokenCode { get; set; }
        public DateTime Date { get; set; }
        public int CustomerID { get; set; }
    }
}