using PaymentGateway_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PaymentGateway_MVC.Controllers
{
    public class EnterPaymentAmountController : Controller
    {
        // GET: EnterPaymentAmount
        public ActionResult Index(int customer_id, string customerName, long cardNumber, int token_id, string token)
        {
            TokenAndCutomerDetails data = new TokenAndCutomerDetails
            {
                Customer_Id = customer_id,
                Customer_Name = customerName,
                Card_Number = cardNumber,
                Token_Id = token_id,
                Token = token
            };
            
            return View(data);
        }

        public ActionResult ConfirmPayment(int _customer_id, int _token_id, int _amount)
        {
            IdandAmount data = new IdandAmount
            {
                customer_id = _customer_id,
                token_id = _token_id,
                amount = _amount
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4074/api/PaymentAuthentication");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<IdandAmount>("http://localhost:4074/api/PaymentAuthentication/PostDetailsForAuthentication", data);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    using (var query = new MonetaEntities())
                    {
                        var _tokenCode = query.TokenDetails
                                    .Where(t => t.CustomerID == _customer_id && t.TokenID == _token_id)
                                    .Select(i => i.TokenCode).FirstOrDefault();

                        int _payment_id = query.PaymentDetails
                                    .Where(t => t.CustomerID == _customer_id && t.TokenID == _token_id)
                                    .Select(i => i.PaymentId).FirstOrDefault();

                        string _email = query.CustomerDetails
                                    .Where(t => t.CustomerID == _customer_id)
                                    .Select(i => i.Email).FirstOrDefault();

                        DataForAuthorization paramsData = new DataForAuthorization
                        {
                            customerId = _customer_id,
                            tokenCode = _tokenCode
                        };

                        //return RedirectToAction("Index", "Authorization", paramsData);
                        return RedirectToAction("Index", "CustomerApproval", new { customer_id_ = _customer_id, payment_id_ = _payment_id, email = _email });
                    }

                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(data);
            
        }

        public void SendCustomerDetails(int customer_id, int payment_id, string email)
        {
            string query = "http://localhost:27826/CustomerApproval?customerid=" + customer_id + "&payment=" + payment_id;

            SendEmail(query, email);

        }

        public void SendEmail(string data,string email)
        {

            //using (SmtpClient smtpClient = new SmtpClient())
            //{
            //    var basicCredential = new NetworkCredential("username", "password");
            //    using (MailMessage message = new MailMessage())
            //    {
            //        MailAddress fromAddress = new MailAddress("from@yourdomain.com");

            //        smtpClient.Host = "mail.mydomain.com";
            //        smtpClient.UseDefaultCredentials = false;
            //        smtpClient.Credentials = basicCredential;

            //        message.From = fromAddress;
            //        message.Subject = "your subject";
            //        // Set IsBodyHtml to true means you can send HTML email.
            //        message.IsBodyHtml = true;
            //        message.Body = "<h1>Click here to approve your payment "+ data + "</h1>";
            //        message.To.Add("to@anydomain.com");

            //        try
            //        {
            //            smtpClient.Send(message);
            //        }
            //        catch (Exception ex)
            //        {
            //            //Error, could not send the message
            //            Response.Write(ex.Message);
            //        }
            //    }
            //}
            
        }
    }
}