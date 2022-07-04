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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FY111.Areas.Identity.Data;

namespace FY111.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<FY111User> userManager, FY111Context context)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //if (User.IsInRole("NormalUser"))
            //    ViewData["UserRole"] = "NormalUser";
            //else if (User.IsInRole("GroupUser"))
            //    ViewData["UserRole"] = "GroupUser";
            //else if (User.IsInRole("MetaverseAdmin"))
            //    ViewData["UserRole"] = "MetaverseAdmin";
            //else if (User.IsInRole("SuperAdmin"))
            //    ViewData["UserRole"] = "SuperAdmin";
            //else
            //    ViewData["UserRole"] = null;
            return View(await _context.Classes.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Download()
        {
            return View();
        }
        public IActionResult Analysis()
        {
            return View();
        }
        public async Task<IActionResult> Log()
        {
            return View(await _context.ClassLogs.Where(x => x.MemberId == _userManager.GetUserId(User)).ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
