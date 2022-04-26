using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using System.Diagnostics;

namespace FY111.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginLogController : ControllerBase
    {
        private readonly FY111Context _context;

        public LoginLogController(FY111Context context)
        {
            _context = context;
        }

        //    // GET: api/Log
        //    [HttpGet]
        //    public async Task<ActionResult<IEnumerable<LoginLog>>> GetLogs()
        //    {
        //        return await _context.LoginLogs.ToListAsync();
        //    }

        //    // GET: api/Log/5
        [HttpGet("list/{id}")]
        public async Task<ActionResult<List<LoginLog>>> GetLog(string id)
        {
            var log = await _context.LoginLogs.Where(x => x.MemberId == id).ToListAsync();

            if (log == null)
            {
                return NotFound();
            }

            return log;
        }

        //    // PUT: api/Log/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to, for
        //    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> PutLog(int id, LoginLog log)
        //    {
        //        if (id != log.MemberId)
        //        {
        //            return BadRequest();
        //        }

        //        _context.Entry(log).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!LogExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return NoContent();
        //    }

        //    // POST: api/Log
        //    // To protect from overposting attacks, enable the specific properties you want to bind to, for
        //    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //    [HttpPost]
        //    public async Task<ActionResult<LoginLog>> PostLog(LoginLog log)
        //    {
        //        _context.LoginLogs.Add(log);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction("GetLog", new { id = log.MemberId }, log);
        //    }

        //    // DELETE: api/Log/5
        //    [HttpDelete("{id}")]
        //    public async Task<ActionResult<LoginLog>> DeleteLog(int id)
        //    {
        //        var log = await _context.LoginLogs.FindAsync(id);
        //        if (log == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.LoginLogs.Remove(log);
        //        await _context.SaveChangesAsync();

        //        return log;
        //    }

        //    private bool LogExists(int id)
        //    {
        //        return _context.LoginLogs.Any(e => e.MemberId == id);
        //    }


        //    [HttpGet("list_member/{member_id}")]
        //    public async Task<ActionResult<IEnumerable<LoginLog>>> ListMemberLog(int member_id)
        //    {
        //        var log = await _context.LoginLogs
        //                    .Where(e => e.MemberId == member_id)
        //                    .ToListAsync();

        //        if (log == null)
        //        {
        //            return NotFound();
        //        }

        //        return log;
        //    }
    }
}
