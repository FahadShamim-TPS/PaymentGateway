using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentGateway_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PaymentGateway_MVC.Controllers
{
    public class GetCustomerDetailsController : Controller
    {
        public ActionResult create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult create(Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4074/api/CustomerDetails");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Customer>("http://localhost:4074/api/CustomerDetails/EnterCustomerData", customer);
                postTask.Wait();

                var readTask = postTask.Result.Content.ReadAsStringAsync();
                readTask.Wait();


                string ResponseValue = readTask.Result;

                var data = (JObject)JsonConvert.DeserializeObject(ResponseValue);
                string token = data["token"].Value<string>();
                int customerId = data["customerId"].Value<int>();
                string customerName = data["name"].Value<string>();
                string emailId = data["emailId"].Value<string>();
                long cardNumber = data["cardNumber"].Value<long>();
                int tokenId = data["token_id"].Value<int>();

                TokenAndCutomerDetails tokenAndCustomer = new TokenAndCutomerDetails();
                tokenAndCustomer.Customer_Id = customerId;
                tokenAndCustomer.Customer_Name = customerName;
                tokenAndCustomer.Customer_Email = emailId;
                tokenAndCustomer.Card_Number = cardNumber;
                tokenAndCustomer.Token = token;
                tokenAndCustomer.Token_Id = tokenId;


                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("ShowToken", "GetToken", tokenAndCustomer);
                }
                // Pointers
                //else
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(customer);
        }

        
    }
}