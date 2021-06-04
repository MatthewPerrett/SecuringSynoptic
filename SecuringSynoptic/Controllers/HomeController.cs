using Microsoft.AspNetCore.Hosting;
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
            string uniqueFilename;
            if (System.IO.Path.GetExtension(file.FileName) == ".jpg" && file.Length < 1048576)
            {
                using (var f = file.OpenReadStream())
                {
                    string absolutePath = _host.WebRootPath + @"\pictures\" + file.FileName;
                    using (FileStream fsOut = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                    {
                        // throw new Exception();
                        f.CopyTo(fsOut);
                        f.Close();
                    }
                }

                   EncryptImage.Encrypt(data.Password, data.Text, _host.WebRootPath + @"\pictures\" + file.FileName);
                
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DecryptImages(IndexViewModel data)
        {
            DecryptImage.Decrypt();
            return RedirectToAction("Index", "Home");
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
