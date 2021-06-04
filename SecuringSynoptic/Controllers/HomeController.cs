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
            if (System.IO.Path.GetExtension(file.FileName) == ".jpg"
                &&
                file.Length < 1048576)
            {
                //FF D8 >>>>> 255 216

                byte[] whitelist = new byte[] { 255, 216 };

                if (file != null)
                {
                    MemoryStream userFile = new MemoryStream();

                    using (var f = file.OpenReadStream())
                    {
                        //int byte1 = f.ReadByte();
                        //int byte2 = f.ReadByte();

                        byte[] buffer = new byte[2];  //how to read an x amount of bytes at 1 go
                        f.Read(buffer, 0, 2); //offset - how many bytes you would lke the pointer to skip

                        for (int i = 0; i < whitelist.Length; i++)
                        {
                            if (whitelist[i] == buffer[i])
                            {

                            }
                            else
                            {
                                //the file is not acceptable
                                ModelState.AddModelError("file", "File is not valid and acceptable");
                                return View();
                            }

                        }
                        //...other reading of bytes happening
                        f.Position = 0;

                        //uploading the file
                        //correctness
                        uniqueFilename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        data.ImageUrl = uniqueFilename;

                        string absolutePath = _host.WebRootPath + @"\pictures\" + uniqueFilename;
                        try
                        {
                            using (FileStream fsOut = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                            {
                                // throw new Exception();
                                f.CopyTo(fsOut);
                            }
                            //   f.CopyTo(userFile); //this goes instead writing the file into a folder
                            f.Close();
                            EncryptImage.Encrypt(data.Password, data.Text);
                            return RedirectToAction("Index", "Home");
                        }
                        catch (Exception ex)
                        {
                            //log
                            _logger.LogError(ex, "Error happend while saving file");
                            return RedirectToAction("Index", "Home");
                        }

                    }
                }
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
