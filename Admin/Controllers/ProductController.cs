using Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class ProductController : BaseController
    {
        tradeforceEntities tf = new tradeforceEntities();
        public ActionResult Index(int classifyId=0)
        {
            string lang = ViewBag.Lang;
            var classify = tf.classify.Where(w => w.IsDel != 0).ToList();
            classifyId=classifyId == 0 ? classify[0].Id : classifyId;
            ViewBag.Classify = classify;
            ViewBag.classifyId = classifyId;
            var query =
            (from pro in tf.product
             join cla in tf.classify on pro.Classify equals cla.Id
             where pro.Classify== classifyId && pro.IsDel != 0  &&pro.Lang==lang
             orderby pro.Index
            select new ProductPage
            {
                pro=pro,
                classify=cla.Name
            }).ToList();
            var props = tf.productproperty.ToList();
            foreach (var p in query)
            {
                p.pros = props.Where(w=>w.ProductId==p.pro.Id).ToList();
            }           
            return View(query);
        }
        [ValidateInput(false)]
        public string ProductAdd(product model,List<productproperty> pros)
        {
            string lang = ViewBag.Lang;
            var res = new { success = true, msg = "" };
            model.InsertDate = DateTime.Now;
            model.Lang = lang;
            using (var transaction = tf.Database.BeginTransaction())
            {
                try
                {
                    if (model.Id != 0)
                    {
                        var cfy = tf.product.FirstOrDefault(f => f.Id == model.Id);
                        cfy.Name = model.Name;
                        cfy.ImgUrl = model.ImgUrl;
                        cfy.Classify = model.Classify;
                        cfy.PDF = model.PDF;
                        cfy.PDFSoureName = model.PDFSoureName;
                        cfy.Features = model.Features;
                        cfy.Quality = model.Quality;
                        cfy.Describe = model.Describe;
                        cfy.ProductID = model.ProductID;
                        cfy.ProductCode = model.ProductCode;
                        var pr = tf.productproperty.Where(w => w.ProductId == cfy.Id).ToList();
                        tf.productproperty.RemoveRange(pr);
                    }
                    else
                    {
                        model.Index = 99;
                        tf.product.Add(model);
                        tf.SaveChanges();
                    }
                    if (pros != null)
                    {
                        foreach (var p in pros)
                        {
                            p.ProductId = model.Id;
                        }
                        tf.productproperty.AddRange(pros);
                    }                 
                    tf.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    res = new { success = false, msg = ex.Message };
                }
            }
                

            return JsonConvert.SerializeObject(res);
        }
        public string ProductDel(int Id)
        {
            var res = new { success = true, msg = "" };
            try
            {
                var cfy = tf.product.FirstOrDefault(f => f.Id == Id);
                cfy.IsDel = 0;
                var pros = tf.productproperty.Where(w=>w.ProductId==Id).ToList();
                tf.productproperty.RemoveRange(pros);
                tf.SaveChanges();
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }

            return JsonConvert.SerializeObject(res);
        }
        public string ProductSort(List<int> ids)
        {
            var res = new { success = true, msg = "" };
            try
            {
                var pros = tf.product.Where(w => ids.Contains(w.Id)).ToList();
                foreach (var pro in pros)
                {
                    pro.Index=ids.IndexOf(pro.Id);
                }
                tf.SaveChanges();
                res = new { success = true, msg = string.Join(",", ids) };
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }

            return JsonConvert.SerializeObject(res);
        }
    }
}