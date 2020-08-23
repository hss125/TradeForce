using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TradeForce.Models;

namespace TradeForce.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var defLang = (int)Lang.english;
            ViewBag.lang=Request.Cookies["lang"] == null? defLang.ToString() : Request.Cookies["lang"].Value;
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = null;
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (!authTicket.Expired)
                {
                    ViewData["UserName"] = authTicket.Name;
                    ViewData["User"] = authTicket.UserData;
                }                
            }
            ViewBag.AdminUrl = ConfigurationManager.AppSettings["AdminUrl"];
        }
    }
}