using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using Microsoft.AspNetCore.Identity;
using FY111.Areas.Identity.Data;
using FY111.Models;
using FY111.Models.Dto;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FY111.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassLogController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public ClassLogController(
            FY111Context context,
            UserManager<FY111User> userManager,
            SignInManager<FY111User> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("list_user/{id}")]
        public async Task<ActionResult<ClassLog>> GetClassLogByUserId(string id)
        {
            try
            {
                var classLogs = await _context.ClassLogs
                    .Where(x => x.MemberId == id)
                    .ToListAsync();
                if (!classLogs.Any())
                    return NotFound(new { 
                        success = true,
                        message = "Not found."
                    });
                return Ok(classLogs);
            }catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("list_Class/{id}")]
        public async Task<ActionResult<ClassLog>> GetClassLogByClassId(int id)
        {
            try
            {
                var classLogs = await _context.ClassLogs
                    .Where(x => x.ClassId == id)
                    .ToListAsync();
                if (!classLogs.Any())
                    return NotFound(new
                    {
                        success = true,
                        message = "Not found."
                    });
                return Ok(classLogs);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("list_user_Class/{user_id}/Class_id")]
        public async Task<ActionResult<ClassLog>> GetClassLogByClassId(string user_id, int class_id)
        {
            try
            {
                var classLogs = await _context.ClassLogs
                    .Where(x => x.ClassId == class_id && x.MemberId ==user_id)
                    .ToListAsync();
                if (!classLogs.Any())
                    return NotFound(new
                    {
                        success = true,
                        message = "Not found."
                    });
                return Ok(classLogs);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }



        [HttpPost("Enter")]
        public async Task<ActionResult<ClassLog>> EnterClass(ClassLog classLog)
        {
            if (!_signInManager.IsSignedIn(User))
                return BadRequest(new {
                    success = false,
                    message = "You are not logged in yet."
                });
            var user = await _userManager.GetUserAsync(User);
            var user_id = user.Id;
            //var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            classLog.MemberId = user_id;
            classLog.StartTime = DateTime.Now;
            _context.ClassLogs.Add(classLog);
            await _context.SaveChangesAsync();

            return Ok(new {
                success = true,
                message = "Create Log Successfully"
            });
        }

        [HttpPatch("Leave")]
        public async Task<ActionResult<ClassLog>> LeaveClass()
        {
            if (!_signInManager.IsSignedIn(User))
                return BadRequest(new
                {
                    success = false,
                    message = "You are not logged in yet."
                });
            var user = await _userManager.GetUserAsync(User);
            var user_id = user.Id;
            //var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ClassLog classLog = _context.ClassLogs.FirstOrDefault(x => x.MemberId == user_id && x.EndTime == null);
            if (classLog == null)
                return BadRequest(new
                {
                    success = false,
                    message = "You haven't entered."
                });
            _context.Entry(classLog).State = EntityState.Modified;
            classLog.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { 
                success = true,
                message = "Modify Log Successfully" 
            });
        }

        [HttpGet("GetAttendees/{ClassId}/{date}")]
        public async Task<IActionResult> GetClassAttendees(int ClassId, DateTime date)
        {
            var signUpList = await _context.ClassSignups.Where(x => x.ClassId == ClassId).Select(x => x.MemberId).ToListAsync();
            var attendees = await _context.ClassLogs.Where(x => x.ClassId == ClassId && DateTime.Compare(x.StartTime, date) >= 0).Select(x => x.MemberId).ToListAsync();
            var logs = new List<AttendeeLogDto>();
            for (int i = 0; i < signUpList.Count; i++)
            {
                bool checkin = false;
                string name = (await _userManager.FindByIdAsync(signUpList[i])).UserName;
                if (attendees.Contains(signUpList[i])) checkin = true;
                var log = new AttendeeLogDto(signUpList[i], name, checkin);
                logs.Add(log);
            }
            return Ok(new
            {
                data = logs
            });
        }

        private bool ClassLogExists(int id)
        {
            return _context.ClassLogs.Any(e => e.ClassId == id);
        }


    }
}
