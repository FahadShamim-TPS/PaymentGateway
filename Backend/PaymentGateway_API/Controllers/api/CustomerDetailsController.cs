using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using PaymentGateway_API.DAL;
using PaymentGateway_API.Managers;
using PaymentGateway_API.ModelForJWT;
using PaymentGateway_API.Models;

namespace PaymentGateway_API.Controllers.api
{
    public class CustomerDetailsController : ApiController
    {
        [HttpPost]
        public IHttpActionResult EnterCustomerData(Customer customer) //POST
        {
            //if (!ModelState.IsValid)
            //    return BadRequest("Invalid data.");

            //Randomly generating salt for hash encryption
            Guid saltGuid = Guid.NewGuid();


            //Add customer data in database along with salt value
            using (var query = new MonetaEntities())
            {
                query.CustomerDetails.Add(new CustomerDetail()
                {
                    //CustomerID = customer.CustomerID,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Address = customer.Address,
                    City = customer.City,
                    ZipCode = customer.ZipCode,
                    Email = customer.Email,
                    CardNumber = customer.CardNumber,
                    CVV_Code = customer.CVV_Code,
                    Expiration = customer.Expiration,
                    Salt = saltGuid.ToString()
                });

                query.SaveChanges();
            }

            string hash = GenerateToken(customer.FirstName,
                                        customer.CardNumber, customer.CVV_Code,
                                        saltGuid.ToString());

            //Adding Token to Token Table in Token DB
            using (var query2 = new MonetaEntities())
            {
                var customer_id = query2.CustomerDetails
                                    .Where(c => c.CardNumber == customer.CardNumber)
                                    .Select(x => x.CustomerID).FirstOrDefault();


                query2.TokenDetails.Add(new TokenDetail()
                {
                    TokenCode = hash,
                    Date = DateTime.Now,
                    CustomerID = Convert.ToInt32(customer_id)
                });
                query2.SaveChanges();
            }


            //filling payment table
            using (var query = new MonetaEntities())
            {

                query.PaymentDetails.Add(new PaymentDetail()
                {
                    TokenID = getTokenId(hash),
                    CustomerID = getCustomerId(hash)

                });
                query.SaveChanges();
            }


            //return Ok(hash); //200
            var multipleValues = new
            {
                token = hash,
                customerId = getCustomerId(hash),
                name = customer.FirstName,
                emailId = customer.Email,
                cardNumber = customer.CardNumber,
                token_id = getTokenId(hash)

            };

            return Json(multipleValues);
        }


        //Getting token generating functions from path: Hashing/StringCipher
        private string GenerateToken(string cus_name, long card_num, int cvv, string salt)
        {
            string Customerdetails = cus_name + "," + card_num + "," + cvv; //This comma seperated should be coming from webConfig file
            string encryptedstring = StringCipher.Encrypt(Customerdetails, salt);

            return encryptedstring;
        }


        private string DecrpytToken(string hash, string salt)
        {
            string decryptedstring = StringCipher.Decrypt(hash, salt);
            return decryptedstring;
        }


        private int getCustomerId(string hash)
        {
            using (var idQuery = new MonetaEntities())
            {
                var customer_id = idQuery.TokenDetails
                                    .Where(t => t.TokenCode == hash)
                                    .Select(x => x.CustomerID).Single();

                return Convert.ToInt32(customer_id);
            }

        }

        private int getTokenId(string hash)
        {
            using (var idQuery = new MonetaEntities())
            {
                var token_id = idQuery.TokenDetails
                                    .Where(t => t.TokenCode == hash)
                                    .Select(x => x.TokenID).Single();

                return Convert.ToInt32(token_id);
            }

        }


        #region JWT Tokenization
        private static JWTContainerModel GetJWTContainerModel(string name, string email)
        {
            return new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, email)
                }
            };
        }
        #endregion

        #region Passing Parameters of Token
        //static void Main(string[] args)
        //{
        //    IAuthContainerModel model = GetJWTContainerModel("Moshe Binieli", "mmoshikoo@gmail.com");
        //    IAuthService authService = new JWTService(model.SecretKey);

        //    string token = authService.GenerateToken(model);

        //    if (!authService.IsTokenValid(token))
        //        throw new UnauthorizedAccessException();
        //    else
        //    {
        //        List<Claim> claims = authService.GetTokenClaims(token).ToList();

        //        Console.WriteLine(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);
        //        Console.WriteLine(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Email)).Value);
        //        Console.WriteLine(token);

        //    }

        //    Console.ReadKey();
        //}
        #endregion

    }
}
