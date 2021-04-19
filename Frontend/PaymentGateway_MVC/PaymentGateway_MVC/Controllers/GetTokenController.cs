using PaymentGateway_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentGateway_MVC.Controllers
{
    public class GetTokenController : Controller
    {
        //// GET: GetToken
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public ActionResult ShowToken(TokenAndCutomerDetails details)
        {
            return View(details);
        }
    }
}