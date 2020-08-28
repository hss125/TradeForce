using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TradeForce.Models;

namespace TradeForce.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        tradeforceEntities tf = new tradeforceEntities();
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.UrlReferrer = Request.UrlReferrer;
            return View();
        }
        public string Login(string Email, string password)
        {
            var res = new { success = true, msg = "" };
            password = Md5Hash(password);
            var user = tf.user.FirstOrDefault(w => w.Email == Email && w.PassWord == password);
            if (user != null)
            {
                var u = JsonConvert.SerializeObject(user);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                                    (1,
                                        Email,
                                        DateTime.Now,
                                        DateTime.Now.AddMinutes(60),
                                        true,
                                        u,
                                        "/"
                                    );
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                cookie.HttpOnly = true;
                HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                FormsAuthentication.SignOut();
                res = new { success = false, msg = "用户名或密码错误！" };
            }
            return JsonConvert.SerializeObject(res);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            if (HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                //Response.Cookies.Remove("user");
                HttpCookie mycookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                mycookie.Expires = DateTime.Now.AddDays(-1);
                mycookie.Value = null;
                HttpContext.Response.Cookies.Add(mycookie);
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
        [HttpGet]
        public ActionResult Reg()
        {
            return View();
        }
        public string Reg(user user)
        {
            Result res = new Result();
            res.Succ = true;
            string lang = Request.Cookies["lang"] == null ? "en" : Request.Cookies["lang"].Value;
            if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.PassWord))
            {
                var u = tf.user.FirstOrDefault(f => f.Email == user.Email);
                if (u == null)
                {
                    user.Lang = lang;
                    user.Type = 5;
                    user.PassWord = Md5Hash(user.PassWord);
                    tf.user.Add(user);
                    tf.SaveChanges();
                }
                else
                {
                    res.Succ = false;
                    res.Msg = "This mailbox is already registered！";
                }                
            }
            return JsonConvert.SerializeObject(res);
        }
        private static string Md5Hash(string str)
        {
            MD5 md5 = MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", null);
        }
    }
}