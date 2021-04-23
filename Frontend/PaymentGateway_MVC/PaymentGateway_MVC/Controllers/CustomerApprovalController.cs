using PaymentGateway_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PaymentGateway_MVC.Controllers
{
    public class CustomerApprovalController : Controller
    {
        // GET: CustomerApproval
        public ActionResult Index(int customer_id_, int payment_id_, string email)
        {
            string queryString = "http://localhost:27826/CustomerApproval/Index?customer_id_=" + customer_id_ + "&payment_id_=" + payment_id_ + "&email=" + email;

            SendEmail(email, queryString);
            
            using (var query = new MonetaEntities())
            {

                string _token = query.TokenDetails
                                    .Where(t => t.CustomerID == customer_id_)
                                    .Select(i => i.TokenCode).FirstOrDefault();


                CustomerApprovalModel model = new CustomerApprovalModel
                {
                    customer_id = Convert.ToInt32(customer_id_),
                    payment_id = Convert.ToInt32(payment_id_),
                    token = _token
                };

                return View(model);
            }

            
            
        }



        public static void SendEmail(string email, string url)
        {
            var fromAddress = new MailAddress("syedfahadshamim@gmail.com", "Syed Fahad Shamim");
            var toAddress = new MailAddress(email, email);
            const string fromPassword = "mynewaccount2";
            const string subject = "Email Authentication";
            string body = "<a href=" + url + "> Click here to Authenticate</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }

        }

       
    }
}