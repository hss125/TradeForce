using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Admin.App_Start
{
    public static class Account
    {
        public static void Login()
        {
            
        }
        public static void Logout()
        {

        }
        public static void CurrUser()
        {
            var id = HttpContext.Current.Session["UserId"];
        }
    }
}