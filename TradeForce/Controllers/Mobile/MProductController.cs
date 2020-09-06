using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeForce.Models;

namespace TradeForce.Controllers.Mobile
{
    public class MProductController : BaseController
    {
        // GET: MProduct
        tradeforceEntities ef = new tradeforceEntities();
        public ActionResult Index(string type = "0")
        {
            ViewBag.type = type;
            var model = ef.classify.Where(w => w.IsDel != 0 && w.Type == type).ToList();
            return View("~/Views/Mobile/Product.cshtml",model);
        }
        public ActionResult Type(int classify = 0)
        {
            string lang = ViewBag.lang;
            var cla = ef.classify.FirstOrDefault(f => f.Id == classify);
            ViewBag.classify = cla;
            ViewBag.type = cla.Type;
            var model = ef.product.Where(w => w.IsDel != 0 && w.Classify == classify && w.Lang == lang).OrderBy(o => o.Index).ToList();
            return View("~/Views/Mobile/ProductType.cshtml", model);
        }
        public ActionResult Detail(int id = 0)
        {
            var pro = ef.product.FirstOrDefault(f => f.Id == id);
            ViewBag.Props = ef.productproperty.Where(w => w.ProductId == pro.Id).ToList();
            if (pro != null)
            {
                ViewBag.classify = ef.classify.FirstOrDefault(f => f.Id == pro.Classify);
            }
            return View("~/Views/Mobile/ProductDetail.cshtml", pro);
        }
    }
}