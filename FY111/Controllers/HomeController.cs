using FY111.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FY111.Models.FY111;

namespace FY111.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //public IActionResult Register()
        //{
        //    return View("Privacy");
        //}


        public IActionResult Test()
        {
            return View();
        }



        // 登入驗驗證測試
        public IActionResult Login()
        {
            //帳密輸入
            return View();
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Member member)
        {
            if (!ModelState.IsValid)
            {
                if (member.Account !="123" && member.Password != "123")
                {
                    ViewData["ErrorMessage"] = "帳號與密碼有錯";
                    return View();
                }
            }
        }
        */
        [Authorize]
        public IActionResult Index2()
        {
            // 登入成功可見
            /*
            if (User.Identity.IsAuthenticated) { return RedirectToPage("/Home/Index2") }
             */
            return View();
        }










        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
