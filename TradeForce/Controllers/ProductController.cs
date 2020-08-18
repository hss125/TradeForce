using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TradeForce.Models;

namespace TradeForce.Controllers
{
    public class ProductController : BaseController
    {
        tradeforceEntities tf = new tradeforceEntities();
        public ActionResult Index(int type=0)
        {
            ViewBag.type = type;
            return View();
        }
        public ActionResult Product(int classify=0)
        {
            string lang = ViewBag.lang;
            var cla = tf.classify.FirstOrDefault(f => f.Id == classify);
            ViewBag.classify = cla;
            ViewBag.type = cla.Type;
            var model = tf.product.Where(w => w.IsDel != 0 && w.Classify == classify&&w.Lang==lang).OrderBy(o=>o.Index).ToList();
            return View(model);
        }
        public ActionResult Detail(int id=0)
        {
            var pro = tf.product.FirstOrDefault(f => f.Id == id);
            ViewBag.Props = tf.productproperty.Where(w => w.ProductId == pro.Id).ToList();
            if (pro != null)
            {
                ViewBag.classify = tf.classify.FirstOrDefault(f => f.Id == pro.Classify);
            }
            return View(pro);
        }
        public ActionResult GetClassify(string type)
        {
            var model = new List<classify>();
            try
            {
                model = tf.classify.Where(w => w.IsDel != 0 && w.Type == type).ToList();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            
            return PartialView("~/Views/Product/PVClassify.cshtml",model);
        }
        public string DownloadPdf(int id)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
            var res = new { isLogin = false, url = "" };
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = null;
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (!authTicket.Expired) {
                    var pdf = tf.product.FirstOrDefault(f => f.Id == id).PDF;
                    res = new { isLogin = true, url = pdf };
                }
            }
            return JsonConvert.SerializeObject(res);
        }
        public ActionResult LeftMenu()
        {
            var model = new List<classify>();
            try
            {
                model = tf.classify.Where(w => w.IsDel != 0).ToList();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return PartialView("~/Views/Product/LeftMenu.cshtml", model);
        }
    }
}