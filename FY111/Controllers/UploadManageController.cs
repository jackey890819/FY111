using FY111.Models.FY111;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FY111.Controllers
{
    public class UploadManageController : Controller
    {
        private readonly FY111Context context;
        public UploadManageController(FY111Context context)
        {
            this.context = context;
        }
        public async Task<IActionResult> UploadManage()
        {
            return View(await context.LoginApps.ToListAsync());
        }
        public async Task<IActionResult> Download()
        {
            return View(await context.LoginApps.ToListAsync());
        }
        public async Task<IActionResult> ClassDownload()
        {
            return View(await context.ClassUnitApps.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, string content)
        {
            foreach (var file in files)
            {
                var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Application\\");
                var fileName = file.FileName;
                var filePath = Path.Combine(dirPath, fileName);
                var extension = Path.GetExtension(file.FileName);
                var application = Path.GetFileName(file.FileName);
                //if (!System.IO.File.Exists(filePath))
                if (!System.IO.File.Exists(filePath) && extension == ".exe")
                {
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    var filemodel = new LoginApp
                    {
                        Application = application,
                        Name = fileName,
                        Content = content
                    };
                    context.LoginApps.Add(filemodel);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("UploadManage");
        }
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await context.LoginApps.Where(x => x.Id == id).FirstOrDefaultAsync();
            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Application\\");
            var fileName = file.Name;
            var filePath = Path.Combine(dirPath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            context.LoginApps.Remove(file);
            context.SaveChanges();
            return RedirectToAction("UploadManage");
        }
    }
}
