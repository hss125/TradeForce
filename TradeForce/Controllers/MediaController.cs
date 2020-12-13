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
        tradeforceEntities tf = new tradeforceEntities();
        // GET: Media
        public ActionResult News()
        {
            var news = tf.news.Where(w => w.IsDelete != 0).OrderByDescending(o=>o.InsertDate).Take(9).ToList();
            return View(news);
        }
        public ActionResult DownLoad()
        {
            return View();
        }
        public ActionResult Terms()
        {
            return View();
        }
        public ActionResult Privacy()
        {
            return View();
        }
        public string GetDownLoad()
        {
            string lang = ViewBag.lang;
            var model = tf.download.Where(w => w.IsDelete != 0 && w.Lang == lang).OrderByDescending(o => o.Id).ToList();
            return JsonConvert.SerializeObject(model);
        }
        public ActionResult NewsDetail(int id=0)
        {
            var news=tf.news.FirstOrDefault(f => f.Id == id);
            return View(news);
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