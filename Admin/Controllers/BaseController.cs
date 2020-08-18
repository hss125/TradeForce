using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (!string.IsNullOrEmpty(Request.QueryString["lang"]))
            {
                var cookie = new HttpCookie("lang_a", Request.QueryString["lang"]);
                HttpContext.Response.Cookies.Add(cookie);
            }
            HttpCookie authCookie = HttpContext.Request.Cookies["user"];
            if (authCookie == null)
            {
                Response.Redirect("/Account/Login");
            }
            else
            {

                FormsAuthenticationTicket authTicket = null;
                try
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch (Exception ex)
                {

                }
                //ViewData["UserName"] = HttpContext.User.Identity.Name;
                ViewData["User"] = authTicket.UserData;
                tradeforceEntities tf = new tradeforceEntities();
                var user = tf.user.FirstOrDefault(f=>f.Email== authTicket.Name);
                if (user != null)
                {
                    ViewBag.Lang = user.Type == 0 ? HttpContext.Request.Cookies["lang_a"].Value : user.Lang;
                }
                else
                {
                    Response.Redirect("/Account/Login");
                }
            }
        }
    }


    
}