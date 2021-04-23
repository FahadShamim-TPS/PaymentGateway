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
        public IHttpActionResult Authorize(DataForAuthorization data) //POST
        {
            DateTime d2 = DateTime.Now;
            string _clientSideTime = d2.ToString("hh:mm tt");
            
            int customerId = data.customerId;
            string tokenCode = data.tokenCode;
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


                if (customer_id == Convert.ToInt32(customerId) && token == Convert.ToString(tokenCode) && TimeDifference < 1)
                {

                    var response = new
                    {
                        status = "Authorized",
                        message = "Payment has been Authorized!"
                    };

                    return Json(response);
                }

                else if (customer_id == Convert.ToInt32(customerId) && token == Convert.ToString(tokenCode) && TimeDifference > 1)
                {
                    var response = new
                    {
                        status = "Unauthorized",
                        message = "Your Token has expired!"
                    };

                    return Json(response);
                }


                else if (customer_id == Convert.ToInt32(customerId) && token != Convert.ToString(tokenCode))
                {
                    var response = new
                    {
                        status = "Unauthorized",
                        message = "Token has been Altered!"
                    };

                    return Json(response);
                }

                

            }
            return Ok();
        }


        [HttpPost]
        public bool RegHash(DataForAuthorization data)
        {
           
      
            using (var query = new MonetaEntities())
            {
                //regSalt = query.CustomerDetails.Join(
                //    query.TokenDetails,
                //    cust => cust.CustomerID,
                //    token => token.CustomerID,
                //    (cust, token) => new RegSaltModel()
                //    {
                //        CutomerId = cust.CustomerID,
                //        CardNumber = cust.CardNumber,
                //        Salt = cust.Salt,
                //        CVV_Code = cust.CVV_Code,
                //        CustomerFirstName = cust.FirstName,
                //        TokenCode = token.TokenCode
                //    }).Where(p=>p.CutomerId == ).ToList<RegSaltModel>();

                IList<RegSaltModel> regSalt = new List<RegSaltModel>();
                 regSalt = (from cust in query.CustomerDetails
                                      join token in query.TokenDetails on cust.CustomerID equals token.CustomerID
                                      where cust.CustomerID == data.customerId
                                      select new RegSaltModel()
                                      {
                                          CutomerId = cust.CustomerID,
                                          CardNumber = cust.CardNumber,
                                          Salt = cust.Salt,
                                          CVV_Code = cust.CVV_Code,
                                          CustomerFirstName = cust.FirstName,
                                          TokenCode = token.TokenCode
                                      }).ToList<RegSaltModel>();

                int payment_id = query.PaymentDetails
                                    .Where(x => x.CustomerID == data.customerId)
                                    .Select(p => p.PaymentId).FirstOrDefault();

                

                DataForAuthorization dataForAuthorization = new DataForAuthorization();
                //  RegenerateHash()
                foreach (var fetch_regSalt in regSalt)
                {
                    dataForAuthorization.customerId = fetch_regSalt.CutomerId;
                    dataForAuthorization.CarNumber = Convert.ToInt64(fetch_regSalt.CardNumber);
                    dataForAuthorization.Salt = fetch_regSalt.Salt;
                    dataForAuthorization.CustomerFirstName = fetch_regSalt.CustomerFirstName;
                    dataForAuthorization.tokenCode = fetch_regSalt.TokenCode;
                    dataForAuthorization.CVV_Code = fetch_regSalt.CVV_Code;
                    dataForAuthorization.paymentId = payment_id;
                }
                StatusModel statusModel = new StatusModel();
                statusModel.customerId = dataForAuthorization.customerId;
                statusModel.paymentId = payment_id;
                int id = dataForAuthorization.customerId;


                string name = string.Empty, cvv, cardNumber;

                using (var query3 = new MonetaEntities())
                {
                    string _salt = query3.CustomerDetails
                                        .Where(x => x.CustomerID == data.customerId)
                                        .Select(x => x.Salt).FirstOrDefault();

                    string _hash = query3.TokenDetails
                                        .Where(x => x.CustomerID == data.customerId)
                                        .Select(x => x.TokenCode).FirstOrDefault();

                    DecrpytandSplitToken(_hash, _salt, out name, out cardNumber, out cvv);

                }

                if (name == data.CustomerFirstName && cardNumber == data.CarNumber.ToString() && cvv == data.CVV_Code.ToString())
                {
                    Authorize(dataForAuthorization);
                    ApprovedStatus(statusModel);
                    return true;
                }
                else
                {
                    Authorize(dataForAuthorization);
                    RejectedStatus(statusModel);
                    return false;
                }
            }
            

            

            

          
            
        }

        //private string RegenerateHash(/*string cust_name,*/ long card_num, int cvv, string salt)
        private string RegenerateToken(DataForAuthorization dataForAuthorization)
        {
            #region Instruction to follow
            //first decrypt token then match CustomerFirstName, CarNumber,CVV_Code from data if match true else false

            //token expiry implement as ui by fahad
            #endregion
            string Customerdetails = dataForAuthorization.CustomerFirstName + "," + dataForAuthorization.CarNumber + "," + dataForAuthorization.CVV_Code;
            string encryptedstring = StringCipher.Encrypt(Customerdetails, dataForAuthorization.Salt);

            return encryptedstring;
        }



        private string DecrpytToken(string hash, string salt)
        {
            string decryptedstring = StringCipher.Decrypt(hash, salt);
            return decryptedstring;
        }

        private string DecrpytandSplitToken(string hash, string salt, out string name, out string cardNo, out string CVV)
        {
            string decryptedstring = StringCipher.Decrypt(hash, salt);
            string[] splited = decryptedstring.Split(',');
            name = splited[0];
            cardNo = splited[1];
            CVV = splited[2];

            return decryptedstring;
        }

        [HttpGet]
        public IHttpActionResult CheckTokenStatusWithPending(int customerId)
        {
            //pending transaction into list
            var dataAuthorization = new DataForAuthorization();
            IList<StatusModel> _statusModel;


            using (var query = new MonetaEntities())
            {
             
                _statusModel = query.PaymentDetails
                                        .Where(p => p.status == "pending" && p.CustomerID == customerId)
                                        .Select(x => new StatusModel()
                                        {
                                            tokenId = x.TokenID,
                                            customerId = x.CustomerID,
                                            paymentId = x.PaymentId,
                                            status = x.status

                                        }).ToList<StatusModel>();

            }

            foreach (var fetch in _statusModel)
            {
                dataAuthorization.customerId = Convert.ToInt32(fetch.customerId.Value);
                dataAuthorization.paymentId = fetch.paymentId.Value;
                dataAuthorization.tokenId = fetch.tokenId.Value;
                dataAuthorization.paymentId = fetch.paymentId.Value;
                // Authorize(dataAuthorization);

                //ApprovedStatus(fetch);

                Authorize(dataAuthorization);

            }

            if (_statusModel.Count == 0)
            {
                return NotFound();
            }

            return Ok(_statusModel);
        }


        public void ApprovedStatus(StatusModel statusModel)
        {
            using (var ctx = new MonetaEntities())
            {
                var existingDetails = ctx.PaymentDetails.Where(p => p.PaymentId == statusModel.paymentId &&
                                      p.CustomerID == statusModel.customerId).FirstOrDefault(); 

                if (existingDetails != null)
                {
                    existingDetails.status = "Approved";

                    ctx.SaveChanges();
                }
                else
                {
                    // return NotFound();
                }
            }
            
        }



        public void RejectedStatus(StatusModel statusModel)
        {
            using (var ctx = new MonetaEntities())
            {
                var existingDetails = ctx.PaymentDetails.Where(p => p.PaymentId == statusModel.paymentId &&
                                      p.CustomerID == statusModel.customerId).FirstOrDefault(); ;

                if (existingDetails != null)
                {
                    existingDetails.status = "Rejected";

                    ctx.SaveChanges();
                }
                else
                {
                    // return NotFound();
                }
            }

            // return Ok();
        }


    }
}
#region commented code
//CheckTokenStatusWithPending 
//   if (payment.status == "pending") { 
//_statusModel = query.PaymentDetails
//                    .Where(p => p.TokenID == payment.token_id
//                    && p.CustomerID == payment.customer_id && p.status == payment.status)
//                    .Select(x => new StatusModel()
//                    {
//                        tokenId = x.TokenID,
//                        customerId = x.CustomerID,
//                        paymentId = x.PaymentId

//                        //tokenId = Convert.ToInt32(x.TokenID),
//                        //customerId = Convert.ToInt32(x.CustomerID),
//                        //paymentId = Convert.ToInt32(x.PaymentId)

//                        //                                        customerId 
//                    }).ToList<StatusModel>();
// }





//RegHash

    // This query is running but with one table
    //regSalt = query.CustomerDetails
    //    .Where(c => c.CustomerID == customerId)
    //    .Select(s => new RegSaltModel()
    //    {
    //        CardNumber = s.CardNumber,
    //        Salt = s.Salt,
    //        CVV_Code = s.CVV_Code,
    //        CustomerFirstName = s.FirstName
    //    }).ToList<RegSaltModel>();



    //string originalData = RegenerateToken(dataForAuthorization).ToString();
    //string dataForMaching = dataForAuthorization.tokenCode.ToString();
    //// decrypt
    //if (originalData.Equals(dataForMaching))
    //{
    //    CheckTokenStatusWithPending(id);
    //    ApprovedStatus(statusModel);
    //    //authorize()
    //}
    //else
    //{

    //    RejectedStatus(statusModel);
    //}
    //return Ok();
#endregion


