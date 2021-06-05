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
            if (System.IO.Path.GetExtension(file.FileName) == ".jpg" && file.Length < 1048576)
            {
                Bitmap image;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    Image tempImg = Image.FromStream(memoryStream);
                    image = (Bitmap)tempImg;
                }
                if (data.Checked == true)
                {
                    image = EncryptTextAndEmbed.EncryptAndEmbedText(data.Password, data.Text, image);
                }
                else
                {
                    image = Steganography.embedText(data.Text, image);
                }
                var stream = new MemoryStream();
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                string format = "image/png";
                stream.Seek(0, SeekOrigin.Begin);
                FileStreamResult img = base.File(stream, format); ;

                img.FileDownloadName = "embeddedImage.png";
                return img;

            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DecryptImages(IFormFile file, IndexViewModel data)
        {
            if (System.IO.Path.GetExtension(file.FileName) == ".png" && file.Length < 1048576)
            {
                Bitmap image;
                string text;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    Image tempImg = Image.FromStream(memoryStream);
                    image = (Bitmap)tempImg;
                }
                if (data.Checked == true)
                {
                    text = DecryptTextAndExtract.DecryptAndExtractText(data.Password, image);
                    ViewData["text"] = text;
                }
                else
                {
                    text = Steganography.extractText(image);
                    ViewData["text"] = text;
                }
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
