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
    public class SupportController : BaseController
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
        public string DownloadFile(int id)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
            var res = new { isLogin = false, url = "" };
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = null;
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (!authTicket.Expired)
                {
                    var pdf = ef.download.FirstOrDefault(f => f.Id == id).Url;
                    res = new { isLogin = true, url = pdf };
                }
            }
            return JsonConvert.SerializeObject(res);
        }
    }
}