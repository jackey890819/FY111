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

        //// POST: api/MetaverseLog
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<MetaverseLog>> PostMetaverseLog(MetaverseLog metaverseLog)
        //{
        //    _context.MetaverseLogs.Add(metaverseLog);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (MetaverseLogExists(metaverseLog.MetaverseId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetMetaverseLog", new { id = metaverseLog.MetaverseId }, metaverseLog);
        //}

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

        //private bool MetaverseLogExists(int id)
        //{
        //    return _context.MetaverseLogs.Any(e => e.MetaverseId == id);
        //}
    }
}
