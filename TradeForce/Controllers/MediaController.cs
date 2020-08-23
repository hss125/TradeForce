using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeForce.Models;

namespace TradeForce.Controllers
{
    public class MediaController : BaseController
    {
        // GET: Media
        public ActionResult News()
        {
            return View();
        }
        public ActionResult DownLoad()
        {
            return View();
        }
        tradeforceEntities tf = new tradeforceEntities();
        public string GetDownLoad()
        {
            string lang = ViewBag.lang;
            var model = tf.download.Where(w => w.IsDelete != 0 && w.Lang == lang).OrderByDescending(o => o.Id).ToList();
            return JsonConvert.SerializeObject(model);
        }
        public ActionResult NewsDetail()
        {
            return View();
        }
        public ActionResult NewsDetail2()
        {
            return View();
        }
        public ActionResult NewsDetail3()
        {
            return View();
        }
        public ActionResult NewsDetail4()
        {
            return View();
        }
    }
}