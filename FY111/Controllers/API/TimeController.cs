using FY111.Areas.Identity.Data;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FY111.Controllers.API
{
    [Route("api/")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;
        private DateTime startTime, endTime;
        public TimeController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("startTimer")]
        public IActionResult TimerStart()
        {
            if (!_signInManager.IsSignedIn(User)) return BadRequest(new
            {
                errors = "Unauthorized"
            });
            startTime = DateTime.Now;
            return Ok(new
            {
                data = new
                {
                    StartDateTime = startTime.ToString("yyyy-MM-dd HH:mm:ss")
                }
            });
        }

        [HttpPost("endTimer")]
        public IActionResult TimerEnd()
        {
            if (!_signInManager.IsSignedIn(User)) return BadRequest(new
            {
                errors = "Unauthorized"
            });
            endTime = DateTime.Now;
            return Ok(new
            {
                data = new
                {
                    StartDateTime = startTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDateTime = endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    TestTimeLast = $"{endTime - startTime:hh\\:mm\\:ss}"
                }
            });
        }
    }
}
