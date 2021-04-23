using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentGateway_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PaymentGateway_MVC.Controllers
{
    public class AuthorizationController : Controller
    {
        // GET: Authorization
        public ActionResult Index(DataForAuthorization data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4074/api/Authorization");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<DataForAuthorization>("http://localhost:4074/api/Authorization/Authorize", data);
                postTask.Wait();

                var readTask = postTask.Result.Content.ReadAsStringAsync();
                readTask.Wait();


                string ResponseValue = readTask.Result;

                try
                {
                    var responseData = (JObject)JsonConvert.DeserializeObject(ResponseValue);
                    string status = responseData["status"].Value<string>();
                    string message = responseData["message"].Value<string>();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Status = status;
                        ViewBag.Messgae = message;
                        //return RedirectToAction("Index", new { token = tokenValue });
                        //return RedirectToAction("ShowToken", verify);
                    }
                    
                }
                catch (Exception ex)
                {

                }

                if (ResponseValue == "")
                {
                    ViewBag.Status = "Unauthorized";
                    ViewBag.Messgae = "Token has been altered";
                }

            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(data);
            
        }
    }
}