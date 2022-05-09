using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FY111.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MetaverseCheckinController : ControllerBase
    {
        private readonly FY111Context _context;

        public MetaverseCheckinController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/MetaverseSignUp
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<MetaverseSignUp>>> GetMetaverseSignUps()
        //{
        //    return await _context.MetaverseSignUps.ToListAsync();
        //}

        // GET: api/MetaverseSignUp/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<MetaverseSignUp>> GetMetaverseSignUp(string id)
        //{
        //    var metaverseSignUp = await _context.MetaverseSignUps.FindAsync(id);

        //    if (metaverseSignUp == null)
        //    {
        //        return NotFound();
        //    }

        //    return metaverseSignUp;
        //}

        // PUT: api/MetaverseSignUp/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMetaverseSignUp(string id, MetaverseSignUp metaverseSignUp)
        //{
        //    if (id != metaverseSignUp.MemberId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(metaverseSignUp).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MetaverseSignUpExists(id))
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

        // POST: api/MetaverseSignUp
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MetaverseCheckin>> PostMetaverseSignUp(MetaverseCheckin metaverseSignUp)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            metaverseSignUp.MemberId = user_id;
            metaverseSignUp.Time = DateTime.Now;
            _context.MetaverseCheckins.Add(metaverseSignUp);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Create Successfully" });
        }

        // DELETE: api/MetaverseSignUp/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MetaverseCheckin>> DeleteMetaverseSignUp(string id)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var metaverseSignUp = await _context.MetaverseCheckins.FindAsync(id, user_id);
            if (metaverseSignUp == null)
            {
                return NotFound();
            }

            _context.MetaverseCheckins.Remove(metaverseSignUp);
            await _context.SaveChangesAsync();

            return metaverseSignUp;
        }

        private bool MetaverseSignUpExists(string id)
        {
            return _context.MetaverseCheckins.Any(e => e.MemberId == id);
        }
    }
}
