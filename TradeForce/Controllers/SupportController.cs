using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeForce.Models;

namespace TradeForce.Controllers
{
    public class SupportController : Controller
    {
        tradeforceEntities ef = new tradeforceEntities();
        // GET: Support
        public ActionResult Quality()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Where()
        {
            var model = ef.country.ToList();
            return View(model);
        }
    }
}