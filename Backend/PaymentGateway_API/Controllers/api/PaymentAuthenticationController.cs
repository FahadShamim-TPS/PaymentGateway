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
    public class PaymentAuthenticationController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetDetailsForAuthentication(PaymentData data) //POST
        //public IHttpActionResult GetDetailsForAuthentication([FromBody] JObject data) //POST
        {
            //int customer_id = data["customer_id"].ToObject<int>();
            //int token_id = data["token_id"].ToObject<int>();


            IList<Payment> payment = null;

            using (var query = new MonetaEntities())
            {
                payment = query.PaymentDetails
                            .Where(t => t.CustomerID == data.Customer_ID && t.TokenID == data.Token_ID)
                            .Select(p => new Payment()
                            {
                                payment_id = p.PaymentId,
                                token_id = p.TokenID.Value,
                                customer_id = p.CustomerID.Value

                            }).ToList<Payment>();

            }

            if (payment.Count == 0)
            {
                return NotFound();
            }

            return Ok(payment);

        }


        [HttpPost]

        //public IHttpActionResult PostDetailsForAuthentication(int customer_id, int token_id, int amount) //POST
        public IHttpActionResult PostDetailsForAuthentication(IdandAmount data) //POST
        {
            //int customer_id = data["customer_id"].ToObject<int>();
            //int token_id = data["token_id"].ToObject<int>();
            //int amount = data["amount"].ToObject<int>();

            using (var query = new MonetaEntities())
            {
                //query.PaymentDetails.Add(new PaymentDetail()
                //{
                //    PaymentAmount = amount,
                //    TokenID = token_id,
                //    CustomerID = customer_id
                //});

                (from p in query.PaymentDetails
                 where p.CustomerID == data.customer_id && p.TokenID == data.token_id
                 select p).ToList()
                .ForEach(x => x.PaymentAmount = data.amount);

                query.SaveChanges();
            }

            return Ok();

        }

    }
}
