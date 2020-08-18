using Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        tradeforceEntities tf = new tradeforceEntities();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public string Login(string email, string password)
        {
            password=Md5Hash(password);
            var res = new { success = true, msg = "" };
            var user = tf.user.FirstOrDefault(w => w.Email == email && w.PassWord == password);
            if (user != null)
            {
                var u = JsonConvert.SerializeObject(user);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                                    (1,
                                        email,
                                        DateTime.Now,
                                        DateTime.Now.AddMinutes(1),
                                        true,
                                        u,
                                        "/"
                                    );
                var cookie = new HttpCookie("user", FormsAuthentication.Encrypt(ticket));
                cookie.HttpOnly = true;
                HttpContext.Response.Cookies.Add(cookie);
                res = new { success = true, msg = user.Type.ToString() };
            }
            else
            {
                FormsAuthentication.SignOut();
                res = new { success = false, msg = "用户名或密码错误！" };
            }
            return JsonConvert.SerializeObject(res);
        }
        private static string Md5Hash(string str)
        {
            MD5 md5 = MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", null);
        }   
        public ActionResult LogOut()
        {
            //FormsAuthentication.SignOut();
            if (HttpContext.Request.Cookies["user"] != null)
            {
                //Response.Cookies.Remove("user");
                HttpCookie mycookie = Request.Cookies["user"];
                mycookie.Expires = DateTime.Now.AddDays(-1);
                mycookie.Value = null;
                HttpContext.Response.Cookies.Add(mycookie);
            }
            return RedirectToAction("Login");
        }
        public ActionResult Reg()
        {
            
            return View();
        }
        public string RegSave(user user)
        {
            var res = new { success = true, msg = "" };
            user.Type = 1;
            user.PassWord = Md5Hash(user.PassWord);
            tf.user.Add(user);
            tf.SaveChanges();
            return JsonConvert.SerializeObject(res);
        }
    }
}