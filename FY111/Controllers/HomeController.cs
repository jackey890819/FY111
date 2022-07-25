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
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace FY111.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //if (User.IsInRole("NormalUser"))
            //    ViewData["UserRole"] = "NormalUser";
            //else if (User.IsInRole("GroupUser"))
            //    ViewData["UserRole"] = "GroupUser";
            //else if (User.IsInRole("ClassAdmin"))
            //    ViewData["UserRole"] = "ClassAdmin";
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
            var model = new Dtos.LogsDto();
            var userId = _userManager.GetUserId(User);
            model.UnitLog = await _context.OperationUnitLogs.Where(x => x.MemberId == userId).ToListAsync();
            for (int i = 0; i < model.UnitLog.Count; i++)
            {
                model.UnitLog[i].OperationLittleunitLogs = await _context.OperationLittleunitLogs.Where(x => x.OperationLogId == model.UnitLog[i].Id).ToListAsync();
                for (int j=0; j < model.UnitLog[i].OperationLittleunitLogs.Count; j++)
                {
                    model.UnitLog[i].OperationLittleunitLogs.ElementAt(j).OperationCheckpoints = await _context.OperationCheckpoints
                        .Where(x => x.OperationLittleunitLogId == model.UnitLog[i].OperationLittleunitLogs.ElementAt(j).Id).ToListAsync();
                    model.UnitLog[i].OperationLittleunitLogs.ElementAt(j).OperationOccdisasters = await _context.OperationOccdisasters
                        .Where(x => x.OperationLittleunitLogId == model.UnitLog[i].OperationLittleunitLogs.ElementAt(j).Id).ToListAsync();
                }
            }
            model.@class = await _context.Classes.ToListAsync();
            for(int i = 0; i < model.@class.Count; i++)
            {
                model.@class[i].ClassUnits = await _context.ClassUnits.Where(x => x.ClassId == model.@class[i].Id).ToListAsync();
                model.@class[i].ClassLogs = await _context.ClassLogs
                    .Where(x => x.ClassId == model.@class[i].Id && x.MemberId == userId).ToListAsync();
                for(int j = 0; j < model.@class[i].ClassUnits.Count; j++)
                {
                    model.@class[i].ClassUnits.ElementAt(j).ClassLittleunits = await _context.ClassLittleunits
                        .Where(x => x.ClassUnitId == model.@class[i].ClassUnits.ElementAt(j).Id).ToListAsync();
                }
            }
            model.LoginLog = await _context.LoginLogs.Where(x => x.MemberId == userId).ToListAsync();
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
