using PaymentGateway_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

                        DataForAuthorization paramsData = new DataForAuthorization
                        {
                            customerId = _customer_id,
                            tokenCode = _tokenCode
                        };

                        return RedirectToAction("Index", "Authorization", paramsData);
                    }
                    
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(data);
            
        }
    }
}