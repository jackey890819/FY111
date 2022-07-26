using FY111.Areas.Identity.Data;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FY111.Controllers.API
{
    [Route("api/")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;
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
        public async Task<IActionResult> TimerStartAsync()
        {
            if (!_signInManager.IsSignedIn(User)) return Unauthorized(new { errors = "Unauthorized" });
            try
            {
                Timer timer = new Timer();
                timer.StartTime = DateTime.Now;
                timer.MemberId = _userManager.GetUserId(User);
                _context.Timers.Add(timer);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    data = new
                    {
                        StartDateTime = timer.StartTime.ToString("yyyy-MM-dd HH:mm:ss")
                    }
                });
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(new { errors = "start timer failed"});
            }
        }

        [HttpPost("endTimer")]
        public async Task<IActionResult> TimerEndAsync()
        {
            if (!_signInManager.IsSignedIn(User)) 
                return Unauthorized( new { errors = "Unauthorized" });
            try
            {
                Timer timer = _context.Timers.OrderBy(x => x.StartTime).LastOrDefault(x => x.MemberId == _userManager.GetUserId(User));
                DateTime startTime = timer.StartTime;
                DateTime endTime = DateTime.Now;
                _context.Timers.Remove(timer);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    data = new
                    {
                        StartDateTime = startTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        EndDateTime = endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        TestTimeLast = $"{endTime - startTime:hh\\:mm\\:ss}"
                    }
                });
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(new { errors = "end timer failed" });
            }
        }
    }
}
