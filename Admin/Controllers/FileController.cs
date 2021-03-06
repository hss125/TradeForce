﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public string FileUpload(string Path)
        {
            HttpPostedFileBase file = Request.Files[0];
            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + file.FileName.Substring(file.FileName.LastIndexOf("."));
            string savePath = AppDomain.CurrentDomain.BaseDirectory + "Upload/" + Path + "/";
            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }
            file.SaveAs(savePath + filename);
            return JsonConvert.SerializeObject(new { code = 0, msg = "", data = new { src = filename } });
        }
        public string ImgUpload(string Path)
        {
            HttpPostedFileBase file = Request.Files[0];
            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + file.FileName.Substring(file.FileName.LastIndexOf("."));
            string savePath = AppDomain.CurrentDomain.BaseDirectory + "Upload/Temp/";
            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }
            file.SaveAs(savePath + filename);
            string savePath2 = AppDomain.CurrentDomain.BaseDirectory + "Upload/" + Path + "/"+ filename;
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Upload/" + Path) == false)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Upload/" + Path);
            }
            try
            {
                if (CompressImage(savePath + filename, savePath2, 100, 600, true))
                {
                    return JsonConvert.SerializeObject(new { code = 0, msg = "", data = new { src = filename } });
                }
            }
            catch (Exception ex)
            {
                
            }
            return JsonConvert.SerializeObject(new { code = 1, msg = "保存失败！" });
        }
        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片地址</param>
        /// <param name="dFile">压缩后保存图片地址</param>
        /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
        /// <param name="size">压缩后图片的最大大小</param>
        /// <param name="sfsc">是否是第一次调用</param>
        /// <returns></returns>
        public static bool CompressImage(string sFile, string dFile, int flag = 90, int size = 300, bool sfsc = true)
        {
            //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
            FileInfo firstFileInfo = new FileInfo(sFile);
            if (sfsc == true && firstFileInfo.Length < size * 1024)
            {
                firstFileInfo.CopyTo(dFile);
                return true;
            }
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int dHeight = iSource.Height / 2;
            int dWidth = iSource.Width / 2;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                    FileInfo fi = new FileInfo(dFile);
                    if (fi.Length > 1024 * size)
                    {
                        flag = flag - 10;
                        CompressImage(sFile, dFile, flag, size, false);
                    }
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
        public string ImgUpload2()
        {
            string Path = "News";
            HttpPostedFileBase file = Request.Files[0];
            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + file.FileName.Substring(file.FileName.LastIndexOf("."));
            string savePath = AppDomain.CurrentDomain.BaseDirectory + "Upload/Temp/";
            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }
            file.SaveAs(savePath + filename);
            string savePath2 = AppDomain.CurrentDomain.BaseDirectory + "Upload/" + Path + "/" + filename;
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Upload/" + Path) == false)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Upload/" + Path);
            }
            try
            {
                if (CompressImage(savePath + filename, savePath2, 100, 600, true))
                {
                    List<string> files = new List<string>();
                    
                    files.Add("/Upload/" + Path + "/" + filename);
                    return JsonConvert.SerializeObject(new { errno = 0, data = files });
                }
            }
            catch (Exception ex)
            {

            }
            return JsonConvert.SerializeObject(new { errno = 1, msg = "保存失败！" });
        }
    }
}