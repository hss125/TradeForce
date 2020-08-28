using Admin.Models;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class HomeController :BaseController
    {
        tradeforceEntities tf = new tradeforceEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }        
        
        public ActionResult Classify()
        {
            var model = tf.classify.Where(w=>w.IsDel!=0).ToList();
            return View(model);
        }
        public string ClassifyAdd(classify model)
        {
            var res = new { success = true,msg="" };
            model.InsertDate = DateTime.Now;
            try
            {
                if (model.Id != 0)
                {
                    var cfy = tf.classify.FirstOrDefault(f => f.Id == model.Id);
                    cfy.Name = model.Name;
                    cfy.Type = model.Type;
                    cfy.LongName = model.LongName;
                    cfy.Describe = model.Describe;
                    cfy.ImgUrl = model.ImgUrl;
                    cfy.ImgUrl2 = model.ImgUrl2;
                }
                else
                {
                    tf.classify.Add(model);
                }
                tf.SaveChanges();
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }
            
            return JsonConvert.SerializeObject(res);
        }
        public string ClassifyDel(int Id)
        {
            var res = new { success = true, msg = "" };
            try
            {
                var cfy = tf.classify.FirstOrDefault(f => f.Id == Id);
                cfy.IsDel = 0;
                tf.SaveChanges();
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }

            return JsonConvert.SerializeObject(res);
        }
        public ActionResult News()
        {
            string lang = ViewBag.Lang;
            var model = tf.news.Where(w => w.Lang == lang && w.IsDelete != 0).OrderByDescending(o=>o.Id).ToList();
            return View(model);
        }
        [ValidateInput(false)]
        public string NewsSave(news model)
        {
            string lang = ViewBag.Lang;
            var res = new { success = true, msg = "" };
            model.InsertDate = DateTime.Now;
            try
            {
                if (model.Id != 0)
                {
                    var cfy = tf.news.FirstOrDefault(f => f.Id == model.Id);
                    cfy.Title = model.Title;
                    cfy.LangTitle = model.LangTitle;
                    cfy.Content = model.Content;
                    cfy.Img = model.Img;
                }
                else
                {
                    model.Lang = lang;
                    tf.news.Add(model);
                }
                tf.SaveChanges();
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }

            return JsonConvert.SerializeObject(res);
        }
        public string NewsDel(int Id)
        {
            var res = new { success = true, msg = "" };
            try
            {
                var cfy = tf.news.FirstOrDefault(f => f.Id == Id);
                cfy.IsDelete = 0;
                tf.SaveChanges();
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }

            return JsonConvert.SerializeObject(res);
        }
        public ActionResult DownLoad(int type=0)
        {
            ViewBag.type = type;
            string lang = ViewBag.Lang;
            var model = tf.download.Where(w => w.Lang == lang && w.IsDelete != 0).OrderByDescending(o => o.Id).ToList();
            return View(model);
        }
        public string DownLoadSave(download model)
        {
            string lang = ViewBag.Lang;
            var res = new { success = true, msg = "" };
            try
            {
                if (model.Id != 0)
                {
                    var cfy = tf.download.FirstOrDefault(f => f.Id == model.Id);
                    cfy.UpdateDate = DateTime.Now;
                    cfy.Name = model.Name;
                    cfy.Url = model.Url;
                    cfy.Describe = model.Describe;
                }
                else
                {
                    model.InsertDate = DateTime.Now;
                    model.Lang = lang;
                    tf.download.Add(model);
                }
                tf.SaveChanges();
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }

            return JsonConvert.SerializeObject(res);
        }
        public string DownLoadDel(int Id)
        {
            var res = new { success = true, msg = "" };
            try
            {
                var cfy = tf.download.FirstOrDefault(f => f.Id == Id);
                cfy.IsDelete = 0;
                tf.SaveChanges();
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }

            return JsonConvert.SerializeObject(res);
        }
        public ActionResult Suggestions()
        {
            string lang = ViewBag.Lang;
            var model=tf.question.Where(w=>w.Country==lang&&w.IsDelete!=0).ToList();
            return View(model);
        }
        public string ExportExcel()
        {
            var res = new { success = true, msg = "" };
            try
            {
                var url=exportFile();
                res = new { success = true, msg = url };
            }
            catch (Exception ex)
            {
                res = new { success = false, msg = ex.Message };
            }
            return JsonConvert.SerializeObject(res);
        }
        public string exportFile()
        {
            string lang = ViewBag.Lang;
            var dicUrl = "/Upload/Excel/";
            var fileName = "Suggestions_"+ lang + ".xls";
            var name = "list";
            //创建工作薄
            var workbook = new HSSFWorkbook();
            //创建表
            var table = workbook.CreateSheet(name);
            // 添加表头
            var row1 = table.CreateRow(0);
            string[] head = { "Company", "Name", "Email", "Phone", "Message", "Date" };
            for (int j = 0; j < head.Count(); j++)
            {
                var cell = row1.CreateCell(j);
                cell.SetCellValue(head[j]);
            }
            table.SetColumnWidth(0, 5000);
            table.SetColumnWidth(1, 5000);
            table.SetColumnWidth(2, 5000);
            table.SetColumnWidth(3, 6000);
            table.SetColumnWidth(4, 19000);
            table.SetColumnWidth(5, 4000);
            var questiones = tf.question.Where(w => w.IsDelete != 0).ToList();
            for (var i = 1; i < questiones.Count() + 1; i++)
            {
                var row = table.CreateRow(i);
                row.Height = 400;
                var cell = row.CreateCell(0);
                cell.SetCellValue(questiones[i - 1].Company);
                var cell2 = row.CreateCell(1);
                cell2.SetCellValue(questiones[i - 1].Name);
                var cell3 = row.CreateCell(2);
                cell3.SetCellValue(questiones[i - 1].Email);
                var cell4 = row.CreateCell(3);
                cell4.SetCellValue(questiones[i - 1].Phone);
                var cell5 = row.CreateCell(4);
                cell5.SetCellValue(questiones[i - 1].Message);
                var cell6 = row.CreateCell(5);
                cell6.SetCellValue(questiones[i - 1].InsertDate?.ToString("yyyy/MM/dd HH:mm"));
            }
            // 写入 
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            workbook = null;
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(baseDirectory + dicUrl))
            {
                Directory.CreateDirectory(baseDirectory + dicUrl);
            }
            using (FileStream fs = new FileStream(baseDirectory + dicUrl + fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
            ms.Close();
            ms.Dispose();
            return dicUrl + fileName;
        }
    }
}