using Newtonsoft.Json.Linq;
using PaymentGateway_API.DAL;
using PaymentGateway_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PaymentGateway_API.Controllers.api
{
    public class AuthorizationController : ApiController
    {

        [HttpPost]
        //public IHttpActionResult Authorize(int customerId, string tokenCode, DateTime tokentime) //POST
        public IHttpActionResult Authorize(DataForAuthorization data) //POST
        {
            DateTime d2 = DateTime.Now;
            string _clientSideTime = d2.ToString("hh:mm tt");

            //int customerId = data["customerId"].ToObject<int>();
            //string tokenCode = data["tokenCode"].ToObject<string>();
            //DateTime tokentime = data["tokentime"].ToObject<DateTime>();

            int customerId = data.customerId;
            string tokenCode = data.tokenCode;
            //DateTime tokentime = data.tokentime;

            using (var query = new MonetaEntities())
            {
                var customer_id = query.TokenDetails
                                    .Where(t => t.CustomerID == customerId && t.TokenCode == tokenCode)
                                    .Select(i => i.CustomerID).FirstOrDefault();

                var token = query.TokenDetails
                                    .Where(t => t.CustomerID == customerId && t.TokenCode == tokenCode)
                                    .Select(i => i.TokenCode).FirstOrDefault();

                var DbTokentime = query.TokenDetails
                                    .Where(t => t.CustomerID == customerId && t.TokenCode == tokenCode)
                                    .Select(i => i.Date).FirstOrDefault();

                DateTime d1 = Convert.ToDateTime(DbTokentime);
                string _tokenCreatedTime = d1.ToString("hh:mm tt");


                

                DateTime tokenCreatedTime = Convert.ToDateTime(_tokenCreatedTime);
                DateTime clientSideTime = Convert.ToDateTime(_clientSideTime);

                double TimeDifference = tokenCreatedTime.Subtract(clientSideTime).TotalMinutes * -1;


                if (customer_id == Convert.ToInt32(customerId) && token == Convert.ToString(tokenCode) && TimeDifference < 15)
                {
                    var response = new
                    {
                        status = "Authorized",
                        message = "Payment has been Authorized!"
                    };

                    return Json(response);
                }

                else if (customer_id == Convert.ToInt32(customerId) && token == Convert.ToString(tokenCode) && TimeDifference > 15)
                {
                    var response = new
                    {
                        status = "Unauthorized",
                        message = "Your Token has expired!"
                    };

                    return Json(response);
                }
            }

            return Ok();
        }

    }
}


