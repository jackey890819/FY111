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

namespace FY111.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FY111Context _context;

        public HomeController(ILogger<HomeController> logger, FY111Context context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("NormalUser"))
                ViewData["UserRole"] = "NormalUser";
            else if (User.IsInRole("GroupUser"))
                ViewData["UserRole"] = "GroupUser";
            else if (User.IsInRole("MetaverseAdmin"))
                ViewData["UserRole"] = "MetaverseAdmin";
            else if (User.IsInRole("SuperAdmin"))
                ViewData["UserRole"] = "SuperAdmin";
            else
                ViewData["UserRole"] = null;
            return View(await _context.Metaverses.ToListAsync());
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
