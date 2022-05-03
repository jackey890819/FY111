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

namespace FY111.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaverseLogController : ControllerBase
    {
        private readonly FY111Context _context;
        private UserManager<FY111User> _userManager;
        private SignInManager<FY111User> _signInManager;
        private readonly ApplicationSettings _appSettings;

        public MetaverseLogController(FY111Context context)
        {
            _context = context;
        }

        //// GET: api/MetaverseLog
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<MetaverseLog>>> GetMetaverseLogs()
        //{
        //    return await _context.MetaverseLogs.ToListAsync();
        //}

        // GET: api/MetaverseLog/5
        [HttpGet("list/{id}")]
        public async Task<ActionResult<MetaverseLog>> GetMetaverseLog(int id)
        {
            var metaverseLog = await _context.MetaverseLogs.FindAsync(id);

            if (metaverseLog == null)
            {
                return NotFound();
            }

            return metaverseLog;
        }

        //// PUT: api/MetaverseLog/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMetaverseLog(int id, MetaverseLog metaverseLog)
        //{
        //    if (id != metaverseLog.MetaverseId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(metaverseLog).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MetaverseLogExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/MetaverseLog
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Enter")]
        public async Task<ActionResult<MetaverseLog>> EnterMetaverse(MetaverseLog metaverseLog)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            metaverseLog.MemberId = user_id;
            metaverseLog.StartTime = DateTime.Now;
            _context.MetaverseLogs.Add(metaverseLog);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Create Log Successfully"});
        }

        [HttpPatch("Leave")]
        public async Task<ActionResult<MetaverseLog>> LeaveMetaverse()
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            MetaverseLog metaverseLog = _context.MetaverseLogs.FirstOrDefault(x => x.MemberId == user_id && x.EndTime == null);
            _context.Entry(metaverseLog).State = EntityState.Modified;
            metaverseLog.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Modify Log Successfully" });
        }

        //// DELETE: api/MetaverseLog/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<MetaverseLog>> DeleteMetaverseLog(int id)
        //{
        //    var metaverseLog = await _context.MetaverseLogs.FindAsync(id);
        //    if (metaverseLog == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.MetaverseLogs.Remove(metaverseLog);
        //    await _context.SaveChangesAsync();

        //    return metaverseLog;
        //}

        private bool MetaverseLogExists(int id)
        {
            return _context.MetaverseLogs.Any(e => e.MetaverseId == id);
        }
    }
}
