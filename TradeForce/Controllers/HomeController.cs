using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeForce.Models;

namespace TradeForce.Controllers
{
    public class HomeController : BaseController
    {
        tradeforceEntities ef = new tradeforceEntities();
        public ActionResult Index()
        {
            if (Agent.CheckAgent())
            {
                return Redirect("/mHome/Index");
            }
            var news = ef.news.Where(w=>w.IsDelete!=0).Take(4).ToList();
            return View(news);
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Customers()
        {
            return View();
        }
        public string AddQuestions(question ques)
        {
            Result res = new Result();
            res.Succ = false;
            res.Msg = "Login timeout";
            try
            {
                //smtp.163.com
                //string senderServerIp = "123.125.50.133";
                //smtp.gmail.com
                //string senderServerIp = "74.125.127.109";
                //smtp.qq.com
                string senderServerIp = "smtp.qq.com";
                //string senderServerIp = "smtp.sina.com";
                string toMailAddress = "2515434850@qq.com";
                string fromMailAddress = "274852493@qq.com";
                string subjectInfo = "TradeForce questions or suggestions";
                string bodyInfo = $"Company:{ques.Company}<br/>Country:{ques.Country}<br/>Name:{ques.Name}<br/>Email:{ques.Email}<br/>Phone:{ques.Phone}<br/>Content:{ques.Message}<br/>";
                string mailUsername = "274852493@qq.com";
                string mailPassword = "ugxypxsdsykmbgba"; //发送邮箱的密码（）
                string mailPort = "25";

                MyEmail email = new MyEmail(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);
                email.Send();
                var u = new Account().GetUser();
                if (!string.IsNullOrEmpty(u.Email))
                {
                    ques.InsertUserId = u.Id;
                }
                ques.InsertDate = DateTime.Now;
                ef.question.Add(ques);
                ef.SaveChanges();
                res.Succ = true;
            }
            catch (Exception ex)
            {
                res.Succ = false;
                res.Msg = ex.Message;
            }
            return JsonConvert.SerializeObject(res);
        }
    }
}