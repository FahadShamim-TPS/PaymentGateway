using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaymentGateway_MVC.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public string Email { get; set; }
        public long CardNumber { get; set; }
        public int CVV_Code { get; set; }
        public DateTime Expiration { get; set; }
        public string Salt { get; set; }
    }
}