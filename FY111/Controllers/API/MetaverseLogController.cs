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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FY111.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaverseLogController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;

        public MetaverseLogController(
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
        public async Task<ActionResult<MetaverseLog>> GetMetaverseLogByUserId(string id)
        {
            try
            {
                var metaverseLogs = await _context.MetaverseLogs
                    .Where(x => x.MemberId == id)
                    .ToListAsync();
                if (!metaverseLogs.Any())
                    return NotFound(new { 
                        success = true,
                        message = "Not found."
                    });
                return Ok(metaverseLogs);
            }catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("list_metaverse/{id}")]
        public async Task<ActionResult<MetaverseLog>> GetMetaverseLogByMetaverseId(int id)
        {
            try
            {
                var metaverseLogs = await _context.MetaverseLogs
                    .Where(x => x.MetaverseId == id)
                    .ToListAsync();
                if (!metaverseLogs.Any())
                    return NotFound(new
                    {
                        success = true,
                        message = "Not found."
                    });
                return Ok(metaverseLogs);
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

        [HttpGet("list_user_metaverse/{user_id}/metaverse_id")]
        public async Task<ActionResult<MetaverseLog>> GetMetaverseLogByMetaverseId(string user_id, int metaverse_id)
        {
            try
            {
                var metaverseLogs = await _context.MetaverseLogs
                    .Where(x => x.MetaverseId == metaverse_id && x.MemberId ==user_id)
                    .ToListAsync();
                if (!metaverseLogs.Any())
                    return NotFound(new
                    {
                        success = true,
                        message = "Not found."
                    });
                return Ok(metaverseLogs);
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
        public async Task<ActionResult<MetaverseLog>> EnterMetaverse(MetaverseLog metaverseLog)
        {
            if (!_signInManager.IsSignedIn(User))
                return BadRequest(new {
                    success = false,
                    message = "You are not logged in yet."
                });
            var user = await _userManager.GetUserAsync(User);
            var user_id = user.Id;
            //var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            metaverseLog.MemberId = user_id;
            metaverseLog.StartTime = DateTime.Now;
            _context.MetaverseLogs.Add(metaverseLog);
            await _context.SaveChangesAsync();

            return Ok(new {
                success = true,
                message = "Create Log Successfully"
            });
        }

        [HttpPatch("Leave")]
        public async Task<ActionResult<MetaverseLog>> LeaveMetaverse()
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
            MetaverseLog metaverseLog = _context.MetaverseLogs.FirstOrDefault(x => x.MemberId == user_id && x.EndTime == null);
            if (metaverseLog == null)
                return BadRequest(new
                {
                    success = false,
                    message = "You haven't entered."
                });
            _context.Entry(metaverseLog).State = EntityState.Modified;
            metaverseLog.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { 
                success = true,
                message = "Modify Log Successfully" 
            });
        }



        private bool MetaverseLogExists(int id)
        {
            return _context.MetaverseLogs.Any(e => e.MetaverseId == id);
        }


    }
}
