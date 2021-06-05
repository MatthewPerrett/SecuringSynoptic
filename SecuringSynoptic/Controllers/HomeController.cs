﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecuringSynoptic.Models;
using SecuringSynoptic.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringSynoptic.Controllers
{
    public class HomeController : Controller
    {

        private readonly IWebHostEnvironment _host;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment host)
        {
            _host = host;
            _logger = logger;
        }

        public IActionResult Index(IFormFile file, IndexViewModel data)
        {
            return View();
        }

        public ActionResult EncryptImages(IFormFile file, IndexViewModel data)
        {
            if (System.IO.Path.GetExtension(file.FileName) == ".jpg" && file.Length < 1048576)
            {
                /*
                using (var f = file.OpenReadStream())
                {
                    string absolutePath = _host.WebRootPath + @"\pictures\" + file.FileName;
                    using (FileStream fsOut = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                    {
                        f.CopyTo(fsOut);
                        f.Close();
                    }
                }

                EncryptImage.Encrypt(data.Password, data.Text, _host.WebRootPath + @"\pictures\" + file.FileName);
                
                return RedirectToAction("Index", "Home");
                */
                
                Bitmap img;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    Image tempImg = Image.FromStream(memoryStream);
                    img = (Bitmap)tempImg;
                }
                img = EncryptImage.Encrypt(data.Password, data.Text, img);


                var stream = new MemoryStream();
                //  
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                //string format = MediaTypeNames.Application.Octet.ToString(); -> doing this leads to direct download
                string format = "image/png";
                stream.Seek(0, SeekOrigin.Begin);
                FileStreamResult res = base.File(stream, format); ;

                res.FileDownloadName = "image_with_hidden.png";
                return res;

            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DecryptImages(IFormFile file, IndexViewModel data)
        {
            if (System.IO.Path.GetExtension(file.FileName) == ".png" && file.Length < 1048576)
            {
                Bitmap img;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    Image tempImg = Image.FromStream(memoryStream);
                    img = (Bitmap)tempImg;
                }
                string text = DecryptImage.Decrypt(data.Password, img);

                ViewData["text"] = text;
                return View();
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
