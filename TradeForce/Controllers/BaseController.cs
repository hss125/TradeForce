using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TradeForce.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.lang=Request.Cookies["lang"] == null?"en":Request.Cookies["lang"].Value;
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