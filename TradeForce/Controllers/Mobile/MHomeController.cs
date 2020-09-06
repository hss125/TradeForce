using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeForce.Models;

namespace TradeForce.Controllers.Mobile
{
    public class MHomeController : BaseController
    {
        // GET: Home
        tradeforceEntities ef = new tradeforceEntities();
        public ActionResult Index()
        {
            var news = ef.news.Where(w => w.IsDelete != 0).Take(4).ToList();
            return View("~/Views/Mobile/Index.cshtml", news);
        }
        public ActionResult About()
        {
            return View("~/Views/Mobile/About.cshtml");
        }
    }
}