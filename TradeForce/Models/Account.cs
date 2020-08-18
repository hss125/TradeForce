using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace TradeForce.Models
{
    public class Account
    {
        public user GetUser()
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = null;
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (!authTicket.Expired)
                {
                    tradeforceEntities ef = new tradeforceEntities();
                    return ef.user.FirstOrDefault(f=>f.Email==authTicket.Name);      
                }
            }
            return new user();
        }
    }
}