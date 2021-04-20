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

        [Required]
        [DataType(DataType.PostalCode)]
        public int ZipCode { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+)).([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Required]
        [DataType(DataType.EmailAddress,ErrorMessage ="Enter correct postal code")]
        [Display(Name = "Email address*")]
        public string Email { get; set; }

        [RegularExpression(@"^4[0-9]{12}(?:[0-9]{3})?$", ErrorMessage = "Please enter a valid credit card number")]
        [Required]
        [DataType(DataType.CreditCard)]
        [Display(Name = "Credit Card*")]
        public long CardNumber { get; set; }


        [RegularExpression(@"[0-9]{1,3}", ErrorMessage ="Numbers Exceding the limit")]
        [Required]
        public int CVV_Code { get; set; }

        [Required]
        //    [RegularExpression(@" ^(0[1-9]|[12] [0-9]|3[01])[- /.] (0[1-9]|1[012])[- /.] (19|20)\d\d$")]
        [DataType(DataType.Date)]
        [Display(Name = "Expiry Date")]
        public DateTime Expiration { get; set; }
        public string Salt { get; set; }
    }
}