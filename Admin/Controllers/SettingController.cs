using Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class SettingController : BaseController
    {
        // GET: Setting
        tradeforceEntities ef = new tradeforceEntities();
        public ActionResult CountrySelect()
        {
            var model = ef.country.ToList();
            return View(model);
        }
        public string CountrySave(country model)
        {
            string lang = ViewBag.Lang;
            var res = new { success = true, msg = "" };
            try
            {
                if (model.Id != 0)
                {
                    var cfy = ef.country.FirstOrDefault(f => f.Id == model.Id);
                    cfy.Country1 = model.Country1;
                    cfy.Url = model.Url;
                }
                else
                {
                    //model.Lang = lang;
                    ef.country.Add(model);
                }
                ef.SaveChanges();
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }

            return JsonConvert.SerializeObject(res);
        }
        public string CountryDel(int Id)
        {
            var res = new { success = true, msg = "" };
            try
            {
                var cfy = ef.country.FirstOrDefault(f => f.Id == Id);
                ef.country.Remove(cfy);
                ef.SaveChanges();
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }

            return JsonConvert.SerializeObject(res);
        }
    }
}